using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for "Flying Enemies" or waypoint mover objects acting as enemies
/// </summary>
[RequireComponent(typeof(WaypointMover))]
public class FlyingEnemy : EnemyBase
{
    [Header("References")]
    [Tooltip("The waypoint mover component which does the work of moving this enemy")]
    public WaypointMover waypointMover = null;

    // The sprite renderer associated with this enemy
    private SpriteRenderer spriteRenderer = null;

    /// <summary>
    /// Description:
    /// Sets up this script (it's reference to a waypoint mover)
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected override void Setup()
    {
        base.Setup();
        waypointMover = GetComponent<WaypointMover>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Description:
    /// Overrides the base enemy update function, to avoid the base class controlling this script's movement
    /// Also sets enemy state according to the waypoint mover
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected override void Update()
    {
        CheckFlipSprite();
        SetStateInformation();
    }

    /// <summary>
    /// Description:
    /// Determines if it is necessary to flip this sprite horizontally
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void CheckFlipSprite()
    {
        if (waypointMover != null && spriteRenderer != null)
        {
            spriteRenderer.flipX = (Vector3.Dot(waypointMover.travelDirection, Vector3.right) < 0);
        }
    }

    /// <summary>
    /// Description:
    /// Sets the state of this enemy according to the waypoint mover component associated with it
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected virtual void SetStateInformation()
    {
        if (waypointMover != null)
        {
            if (waypointMover.stopped)
            {
                enemyState = EnemyState.Idle;
            }
            else
            {
                enemyState = EnemyState.Walking;
            }
        }
        else
        {
            enemyState = EnemyState.Idle;
        }
    }
}
