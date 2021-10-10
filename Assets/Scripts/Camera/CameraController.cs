using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class which handles camera movement
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    // The camera being controlled by this script
    [HideInInspector] private Camera playerCamera = null;

    [Header("GameObject References")]
    [Tooltip("The target to follow with this camera")]
    public Transform target = null;

    /// <summary>
    /// Enum to determine camera movement styles
    /// </summary>
    public enum CameraStyles
    {
        Locked,
        Overhead,
        DistanceFollow,
        OffsetFollow,
        BetweenTargetAndMouse
    }

    [Header("CameraMovement")]
    [Tooltip("The way this camera moves:\n" +
        "\tLocked: Camera does not move\n" +
        "\tOverhead: Camera stays over that target\n" +
        "\tDistanceFollow: Camera stays within [Max distance From Target] away from the target.\n" +
        "\tOffsetFollow: Camera follows the target at an offset" +
        "\tBetweenTargetAndMouse: Camera stays directly between the mouse position and the target position")]
    public CameraStyles cameraMovementStyle = CameraStyles.Locked;

    [Tooltip("The maximum distance away from the target that the camera can move")]
    public float maxDistanceFromTarget = 5.0f;
    [Tooltip("The offset from the computed camera position to move the camera to in Offset Follow mode.")]
    public Vector2 cameraOffset = Vector2.zero;
    [Tooltip("The z coordinate to use for the camera position")]
    public float cameraZCoordinate = -10.0f;
    [Tooltip("The percentage distance between the target position and the\n" +
        "mouse position to move the camera to in BetweenTargetAndMouse camera mode.")]
    public float mouseTracking = 0.5f;
    [Tooltip("The input manager used to collect input information")]
    public InputManager inputManager;

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first update
    /// Input: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    void Start()
    {
        InitilalSetup();
    }

    /// <summary>
    /// Description:
    /// Handles the initial setup of this script and its required components
    /// Input:
    /// none
    /// Returns:
    /// void
    /// </summary>
    void InitilalSetup()
    {
        playerCamera = GetComponent<Camera>();
        SetUpInputManager();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function that is called every frame
    /// Input: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    void Update()
    {
        SetCameraPosition();
    }

    /// <summary>
    /// Description:
    /// Gets the reference to the input manager
    /// Throws an error if none exists
    /// Input:
    /// none
    /// Returns:
    /// none
    /// </summary>
    private void SetUpInputManager()
    {
        inputManager = InputManager.instance;
        if (inputManager == null)
        {
            Debug.LogError("There is no InputManager set up in the scene for the CamaeraController to read from");
        }
    }

    /// <summary>
    /// Description:
    /// Sets the camera's position according to the settings
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void SetCameraPosition()
    {
        if (target != null)
        {
            Vector3 targetPosition = GetTargetPosition();
            Vector3 mousePosition = GetPlayerMousePosition();
            Vector3 desiredCameraPosition = ComputeCameraPosition(targetPosition, mousePosition);

            transform.position = desiredCameraPosition;
        }      
    }

    /// <summary>
    /// Description:
    /// Gets the follow target's position
    /// Input: 
    /// none
    /// Returns: 
    /// Vector3
    /// </summary>
    /// <returns>Vector3: The position of the target assigned to this camera controller.</returns>
    public Vector3 GetTargetPosition()
    {
        if (target != null)
        {
            return target.position;
        }
        return transform.position;
    }

    /// <summary>
    /// Description:
    /// Finds and returns the mouse position
    /// Input: 
    /// none
    /// Returns: 
    /// Vector3
    /// </summary>
    /// <returns>Vector3: The position of the player's mouse in world coordinates</returns>
    public Vector3 GetPlayerMousePosition()
    {
        return playerCamera.ScreenToWorldPoint(new Vector2(inputManager.horizontalLookAxis, inputManager.verticalLookAxis));
    }

    /// <summary>
    /// Description:
    /// Takes the target's position and mouse position, and returns the desired position of the camera
    /// Input: 
    /// Vector3 targetPosition, Vector3 offsetPosition
    /// Returns:
    /// Vector3
    /// </summary>
    /// <param name="targetPosition"> The position of the target the camera is following. </param>
    /// <param name="mousePosition"> The position of the mouse in world space used to determine distance from the target. </param>
    /// <returns>Vector3: The position the camera should be at</returns>
    public Vector3 ComputeCameraPosition(Vector3 targetPosition, Vector3 mousePosition)
    {
        Vector3 result = Vector3.zero;
        switch (cameraMovementStyle)
        {
            case CameraStyles.Locked:
                result = transform.position;
                break;
            case CameraStyles.Overhead:
                result = targetPosition;
                break;
            case CameraStyles.DistanceFollow:
                result = transform.position;
                if ((targetPosition - result).magnitude > maxDistanceFromTarget)
                {
                    result = targetPosition + (result - targetPosition).normalized * maxDistanceFromTarget;
                }
                break;
            case CameraStyles.OffsetFollow:
                result = targetPosition + (Vector3)cameraOffset;
                break;
            case CameraStyles.BetweenTargetAndMouse:
                Vector3 desiredPosition = Vector3.Lerp(targetPosition, mousePosition, mouseTracking);
                Vector3 difference = desiredPosition - targetPosition;
                difference = Vector3.ClampMagnitude(difference, maxDistanceFromTarget);
                result = targetPosition + difference;
                break;
        }
        result.z = cameraZCoordinate;
        return result;
    }
}
