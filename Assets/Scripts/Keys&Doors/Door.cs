﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which handles a door's locking and unlocking mechanism
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Door : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The ID of the door used to determine which keys unlock it.\n" +
        "A door ID of 0 is unlocked by default.")]
    public int doorID = 0;
    [Tooltip("Whether this door is open right now.")]
    public bool isOpen = false;
    [Tooltip("Events to call when opening the door.")]
    public UnityEvent openEvent = new UnityEvent();
    [Tooltip("Events to call when closing the door.")]
    public UnityEvent closeEvent = new UnityEvent();
    [Tooltip("The effect to play when the door opens or closes")]
    public GameObject doorOpenAndCloseEffect;
    [Tooltip("The effect to play when the door is attempted to open but can not")]
    public GameObject doorLockedEffect;

    /// <summary>
    /// Description:
    /// Standard Unity function called when a Collision2D collides with the collider on this gameobject
    /// Input: 
    /// Collision2D collision
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="collision">The collider that caused the collision and other collision data</param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            AttemptToOpen();
        }
    }


    /// <summary>
    /// Description:
    /// Standard Unity function called when a Collider2D enters an attached trigger
    /// Input: 
    /// Collider2D collision
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="collision">The collider that entered the trigger</param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AttemptToOpen();
        }
    }

    /// <summary>
    /// Description:
    /// Attempts to open the door with the keys at the player's disposal
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    public void AttemptToOpen()
    {
        if (CheckPlayerHasKey() && !isOpen)
        {
            Open();
        }
        else if (doorLockedEffect && !isOpen)
        {
            Instantiate(doorLockedEffect, transform.position, Quaternion.identity, null);
        }
    }

    /// <summary>
    /// Description:
    /// Tests whether the player has the key necessary to open this door
    /// Input: 
    /// none
    /// Return: 
    /// bool
    /// </summary>
    /// <returns>bool: Whether or not the player has the necessary key</returns>
    public bool CheckPlayerHasKey()
    {
        return KeyRing.HasKey(this);
    }

    /// <summary>
    /// Description:
    /// Opens the door
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected virtual void Open()
    {
        isOpen = true;
        openEvent.Invoke();
        if (doorOpenAndCloseEffect)
        {
            Instantiate(doorOpenAndCloseEffect, transform.position, Quaternion.identity, null);
        }
    }

    /// <summary>
    /// Description:
    /// Closes the door
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected virtual void Close()
    {
        isOpen = false;
        closeEvent.Invoke();
        if (doorOpenAndCloseEffect)
        {
            Instantiate(doorOpenAndCloseEffect, transform.position, Quaternion.identity, null);
        }
    }
}
