using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles behavior for jumping on top of something with a head
/// </summary>
public class Head : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The health component associated with this head")]
    public Health associatedHealth;
    [Tooltip("The amount of damage to deal when jumped on")]
    public int damage = 1;

    /// <summary>
    /// Description:
    /// Standard Unity function that is called when a collider enters a trigger
    /// Input:
    /// Collision2D collision
    /// Return:
    /// void (no return)
    /// </summary>
    /// <param name="collision">The collision information of what has collided with the attached collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Feet")
        {
            associatedHealth.TakeDamage(damage);
            BouncePlayer();
        }
    }

    /// <summary>
    /// Description:
    /// Tells the player controller attached to the player object collided with to bounce
    /// Input: 
    /// GameObject playerObject
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="playerObject">The gameobject that collided with the head (with the player tag on it)</param>
    private void BouncePlayer()
    {
        PlayerController playerController = GameManager.instance.player.GetComponentInChildren<PlayerController>();
        if (playerController != null)
        {
            playerController.Bounce();
        }
    }
}
