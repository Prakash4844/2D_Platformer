using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pickup-derived component which addes to the player's score when collected
/// </summary>
public class ScorePickup : Pickup
{
    [Header("Score Settings")]
    [Tooltip("Amount of score to add when picked up")]
    public int scoreAmount = 1;

    /// <summary>
    /// Description:
    /// When picked up, adds to the player's score
    /// Input:
    /// Collider2D collision
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="collision">The collision that touched the pickup</param>
    public override void DoOnPickup(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<Health>() != null)
        {
            GameManager.AddScore(scoreAmount);
        }
        base.DoOnPickup(collision);
    }
}
