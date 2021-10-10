using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which handles player movement
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    #region GameObject references
    [Header("Game Object and Component References")]
    [Tooltip("The Input Manager component used to gather player input.")]
    public InputManager inputManager = null;
    [Tooltip("The Ground Check component used to check whether this player is grounded currently.")]
    public GroundCheck groundCheck = null;
    [Tooltip("The sprite renderer that represents the player.")]
    public SpriteRenderer spriteRenderer = null;
    [Tooltip("The health component attached to the player.")]
    public Health playerHealth;

    // The rigidbody used to move the player (necessary for this component, so not made public)
    private Rigidbody2D playerRigidbody = null;
    #endregion

    #region Getters (primarily from other components)
    #region Directional facing
    /// <summary>
    /// Enum to help determine which direction the player is facing.
    /// </summary>
    public enum PlayerDirection
    {
        Right,
        Left
    }

    // Which way the player is facing right now
    public PlayerDirection facing
    {
        get
        {
            if (horizontalMovementInput > 0)
            {
                return PlayerDirection.Right;
            }
            else if (horizontalMovementInput < 0)
            {
                return PlayerDirection.Left;
            }
            else
            {
                if (spriteRenderer != null && spriteRenderer.flipX == true)
                    return PlayerDirection.Left;
                return PlayerDirection.Right;
            }
        }
    }
    #endregion

    // Whether this player is grounded false if no ground check component assigned
    public bool grounded
    {
        get
        {
            if (groundCheck != null)
            {
                return groundCheck.CheckGrounded();
            }
            else
            {
                return false;
            }
        }
    }

    #region Input from Input Manager
    // The horizontal movement input collected from the input manager
    public float horizontalMovementInput
    {
        get
        {
            if (inputManager != null)
                return inputManager.horizontalMovement;
            else
                return 0;
        }
    }
    // the jump input collected from the input manager
    public bool jumpInput
    {
        get
        {
            if (inputManager != null)
                return inputManager.jumpStarted;
            else
                return false;
        }
    }
    #endregion
    #endregion

    #region Movement Variables
    [Header("Movement Settings")]
    [Tooltip("The speed at which to move the player horizontally")]
    public float movementSpeed = 4.0f;

    [Header("Jump Settings")]
    [Tooltip("The force with which the player jumps.")]
    public float jumpPower = 10.0f;
    [Tooltip("The number of jumps that the player is alowed to make.")]
    public int allowedJumps = 1;
    [Tooltip("The duration that the player spends in the \"jump\" state")]
    public float jumpDuration = 0.1f;
    [Tooltip("The effect to spawn when the player jumps")]
    public GameObject jumpEffect = null;
    [Tooltip("Layers to pass through when moving upwards")]
    public List<string> passThroughLayers = new List<string>();

    // The number of times this player has jumped since being grounded
    private int timesJumped = 0;
    // Whether the player is in the middle of a jump right now
    private bool jumping = false;
    #endregion

    #region Player State Variables
    /// <summary>
    /// Enum used for categorizing the player's state
    /// </summary>
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Dead
    }

    // The player's current state (walking, idle, jumping, or falling)
    public PlayerState state = PlayerState.Idle;
    #endregion
    #endregion

    #region Functions
    #region GameObject Functions
    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first update
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void Start()
    {
        SetupRigidbody();
        SetUpInputManager();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame after update
    /// Every frame, process input, move the player, determine which way they should face, and choose which state they are in
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void LateUpdate()
    {
        ProcessInput();
        HandleSpriteDirection();
        DetermineState();
    }
    #endregion

    #region Input Handling and Movement Functions
    /// <summary>
    /// Description:
    /// Collects input from the input manager, then calls functions to move the player accordingly
    /// Input: none
    /// Return: void (no return)
    /// </summary>
    private void ProcessInput()
    {
        HandleMovementInput();
        HandleJumpInput();
    }

    /// <summary>
    /// Description:
    /// Handles movement input
    /// Input: none
    /// Return: void (no return)
    /// </summary>
    private void HandleMovementInput()
    {
        Vector2 movementForce = Vector2.zero;
        if (Mathf.Abs(horizontalMovementInput) > 0 && state != PlayerState.Dead)
        {
            movementForce = transform.right * movementSpeed * horizontalMovementInput;
        }
        MovePlayer(movementForce);
    }

    /// <summary>
    /// Description:
    /// Moves the player with a specified force
    /// Input: 
    /// Vector2 movementForce
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="movementForce">The force with which to move the player</param>
    private void MovePlayer(Vector2 movementForce)
    {
        if (grounded && !jumping)
        {
            float horizontalVelocity = movementForce.x;
            float verticalVelocity = 0;
            playerRigidbody.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        }
        else
        {
            float horizontalVelocity = movementForce.x;
            float verticalVelocity = playerRigidbody.velocity.y;
            playerRigidbody.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        }
        if (playerRigidbody.velocity.y > 0)
        {
            foreach (string layerName in passThroughLayers)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(layerName), true);
            } 
        }
        else
        {
            foreach (string layerName in passThroughLayers)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(layerName), false);
            }
        }
    }

    /// <summary>
    /// Description:
    /// Handles jump input
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            StartCoroutine("Jump", 1.0f);
        }
    }

    /// <summary>
    /// Description:
    /// Coroutine which causes the player to jump.
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <returns>IEnumerator: makes coroutine possible</returns>
    private IEnumerator Jump(float powerMultiplier = 1.0f)
    {
        if (timesJumped < allowedJumps && state != PlayerState.Dead)
        {
            jumping = true;
            float time = 0;
            SpawnJumpEffect();
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
            playerRigidbody.AddForce(transform.up * jumpPower * powerMultiplier, ForceMode2D.Impulse);
            timesJumped++;
            while (time < jumpDuration)
            {
                yield return null;
                time += Time.deltaTime;
            }
            jumping = false;
        }
    }

    /// <summary>
    /// Description:
    /// Spawns the effect that occurs when the player jumps
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void SpawnJumpEffect()
    {
        if (jumpEffect != null)
        {
            Instantiate(jumpEffect, transform.position, transform.rotation, null);
        }
    }

    /// <summary>
    /// Description:
    /// Bounces the player upwards, refunding jumps.
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    public void Bounce()
    {
        timesJumped = 0;
        if (inputManager.jumpHeld)
        {
            StartCoroutine("Jump", 1.5f);
        }
        else
        {
            StartCoroutine("Jump", 1.0f);
        }
    }

    /// <summary>
    /// Description:
    /// Determines which way the player should be facing, then makes them face in that direction
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void HandleSpriteDirection()
    {
        if (spriteRenderer != null)
        {
            if (facing == PlayerDirection.Left)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }
    #endregion

    #region State Functions
    /// <summary>
    /// Description:
    /// Gets and returns the player's current state
    /// Input: 
    /// none
    /// Return: 
    /// PlayerState
    /// </summary>
    /// <returns>PlayerState: The player's current state (idle, walking, jumping, falling</returns>
    private PlayerState GetState()
    {
        return state;
    }

    /// <summary>
    /// Description:
    /// Sets the player's current state
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="newState">The PlayerState to set the current state to</param>
    private void SetState(PlayerState newState)
    {
        state = newState;
    }

    /// <summary>
    /// Description:
    /// Determines which state is appropriate for the player currently
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void DetermineState()
    {
        if (playerHealth.currentHealth <= 0)
        {
            SetState(PlayerState.Dead);
        }
        else if (grounded)
        {
            if (playerRigidbody.velocity.magnitude > 0)
            {
                SetState(PlayerState.Walk);
            }
            else
            {
                SetState(PlayerState.Idle);
            }
            if (!jumping)
            {
                timesJumped = 0;
            }
        }
        else
        {
            if (jumping)
            {
                SetState(PlayerState.Jump);
            }
            else
            {
                SetState(PlayerState.Fall);
            }
        }
    }
    #endregion

    /// <summary>
    /// Description:
    /// Sets up the player's rigidbody
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void SetupRigidbody()
    {
        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
        }
    }

    /// <summary>
    /// Description:
    /// Gets the reference to the input manager
    /// Throws an error if none exists
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void SetUpInputManager()
    {
        inputManager = InputManager.instance;
        if (inputManager == null)
        {
            Debug.LogError("There is no InputManager set up in the scene for the PlayerController to read from");
        }
    }
    #endregion
}
