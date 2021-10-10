using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component on gameobjects with colliders which determines if there is
/// a collider overlapping them which is on a specific layer.
/// Used to check for ground.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class GroundCheck : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The layers which are considered \"Ground\".")]
    public LayerMask groundLayers = new LayerMask();
    [Tooltip("The collider to check with. (Defaults to the collider on this game object.)")]
    public Collider2D groundCheckCollider = null;

    [Header("Effect Settings")]
    [Tooltip("The effect to create when landing")]
    public GameObject landingEffect;

    // Whether or not the player was grounded last check
    [HideInInspector]
    public bool groundedLastCheck = false;

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
        // When this component starts up, ensure that the collider is not null, if possible
        GetCollider();
    }

    /// <summary>
    /// Description:
    /// Attempts to setup the collider to be used with ground checking,
    /// if one is not already set up.
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    public void GetCollider()
    {
        if (groundCheckCollider == null)
        {
            groundCheckCollider = gameObject.GetComponent<Collider2D>();
        }
    }

    /// <summary>
    /// Description:
    /// Checks whether there is a collider overlapping the checking collider which is on a "ground" layer.
    /// Input: 
    /// none
    /// Return: 
    /// bool
    /// </summary>
    /// <returns>bool: Whether or not this collider counts as grounded</returns>
    public bool CheckGrounded()
    {
        // Ensure the collider is assigned
        if (groundCheckCollider == null)
        {
            GetCollider();
        }

        // Find the colliders that overlap this one
        Collider2D[] overlaps = new Collider2D[5];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = groundLayers;
        groundCheckCollider.OverlapCollider(contactFilter, overlaps);

        // Check if one of the overlapping colliders is on the "ground" layer
        foreach (Collider2D overlapCollider in overlaps)
        {
            if (overlapCollider != null)
            {
                // This line determines if the collider found is on a layer in the ground layer mask
                // sorry it is so math-heavy
                int match = contactFilter.layerMask.value & (int)Mathf.Pow(2, overlapCollider.gameObject.layer);
                if (match > 0)
                {
                    if (landingEffect && !groundedLastCheck)
                    {
                        Instantiate(landingEffect, transform.position, Quaternion.identity, null);
                    }
                    groundedLastCheck = true;
                    return true;
                }
            }
        }
        groundedLastCheck = false;
        return false;
    }
}
