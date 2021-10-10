using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the animation of platforms
/// </summary>
[RequireComponent(typeof(WaypointMover))]
[RequireComponent(typeof(Animator))]
public class PlatformAnimator : MonoBehaviour
{
    // The waypoint mover controlling the platform's motion
    WaypointMover mover = null;
    // The animator that determines what animation to play for the platform
    Animator animator = null;

    /// <summary>
    /// Description:
    /// Standard Unity function that is called when this script is first loaded
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void Awake()
    {
        // Get the mover and animator from the game object this script is attached to
        mover = GetComponent<WaypointMover>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once per frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    private void Update()
    {
        // Set the isMoving bool of the animator according to the waypoint mover's state
        if (mover != null && animator != null)
        {
            animator.SetBool("isMoving", !mover.stopped);
        }
    }
}
