using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemyBase-derived enemy class which walks in a direction until it hits a wall
/// </summary>
public class WalkingEnemy : EnemyBase
{
    /// <summary>
    /// Enum to track which direction this enemy is walking
    /// </summary>
    public enum WalkDirections { Right, Left, None }
    


    [Header("References")]
    [Tooltip("The ground check component used to test whether this enemy has hit a wall to the left.")]
    public GroundCheck wallTestLeft;
    [Tooltip("The ground check component used to test whether this enemy has hit a wall to the right.")]
    public GroundCheck wallTestRight;
    [Tooltip("Left ground check component used to determine when the enemy has reached an edge on its left.")]
    public GroundCheck leftEdge;
    [Tooltip("Right ground check component used to determine when the enemy has reached an edge on its right.")]
    public GroundCheck rightEdge;

    [Header("Settings")]
    [Tooltip("The direction that this enemy walks in until it hits a wall")]
    public WalkDirections walkDirection = WalkDirections.None;
    [Tooltip("Whether this enemy will turn around if it detects an edge.")]
    public bool willTurnAroundAtEdge = false;

    // The sprite renderer for this enemy
    private SpriteRenderer spriteRenderer = null;

    /// <summary>
    /// Description:
    /// Sets up this enemy
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected override void Setup()
    {
        base.Setup();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Description:
    /// Override of base class Update function
    /// Determines walking direction in addition to moving
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected override void Update()
    {
        DetermineWalkDirection();
        base.Update();
    }

    /// <summary>
    /// Description:
    /// Override of EnemyBase.GetMovement
    /// Moves in a direction until a wall is hit, then switches direction
    /// Input: 
    /// none
    /// Return: 
    /// Vector3
    /// </summary>
    /// <returns>Vector3: The movement for this frame</returns>
    protected override Vector3 GetMovement()
    {
        // Determine the movement vector based on the direction that the enemy is currently moving in
        switch (walkDirection)
        {
            case WalkDirections.None:
                enemyState = EnemyState.Idle;
                return Vector3.zero;
            case WalkDirections.Left:
                enemyState = EnemyState.Walking;
                return Vector3.left * moveSpeed * Time.deltaTime;
            case WalkDirections.Right:
                enemyState = EnemyState.Walking;
                return Vector3.right * moveSpeed * Time.deltaTime;
            default:
                return base.GetMovement();
        }
    }

    /// <summary>
    /// Description:
    /// Determines whether a change in direction is needed, and changes it if necessary
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void DetermineWalkDirection()
    {
        if (TestWall() || GetIsNearEdge())
        {
            TurnAround();
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (walkDirection == WalkDirections.Right);
        }
    }

    /// <summary>
    /// Description:
    /// Turns the enemy around
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void TurnAround()
    {
        if (walkDirection == WalkDirections.Left)
        {
            walkDirection = WalkDirections.Right;
        }
        else if (walkDirection == WalkDirections.Right)
        {
            walkDirection = WalkDirections.Left;
        }
    }

    /// <summary>
    /// Description:
    /// Tests whether this enemy has hit a wall in the direction that it is walking in
    /// Input: 
    /// none
    /// Return:
    /// bool
    /// </summary>
    /// <returns>bool: Whether or not this enemy has hit a wall in the direction it is walking</returns>
    protected virtual bool TestWall()
    {
        switch (walkDirection)
        {
            case WalkDirections.Left:
                if (wallTestLeft != null)
                {
                    return wallTestLeft.CheckGrounded();
                }
                break;
            case WalkDirections.Right:
                if (wallTestRight != null)
                {
                    return wallTestRight.CheckGrounded();
                }
                break;
        }
        return false;
    }

    /// <summary>
    /// Description:
    /// Tests each edge check in the list "edgeTesters" to see if the enemy must change directions.
    /// Input: 
    /// none
    /// Return: 
    /// bool
    /// </summary>
    /// <returns>bool: Whether or not this enemy is near an edge and must turn around</returns>
    private bool GetIsNearEdge()
    {
        GroundCheck check = null;
        if (walkDirection == WalkDirections.Left)
        {
            check = leftEdge; 
        }
        else if (walkDirection == WalkDirections.Right)
        {
            check = rightEdge;
        }
        if (check != null && !check.CheckGrounded())
        {
            return willTurnAroundAtEdge;
        }
        return false;
    }
}
