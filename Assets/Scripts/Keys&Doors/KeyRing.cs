using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class which manages which keys are held by the player
/// </summary>
public static class KeyRing
{
    // The IDs of the keys held by the player
    private static HashSet<int> keyIDs = new HashSet<int>() { 0 };

    /// <summary>
    /// Description:
    /// Adds a key to the player's key ring
    /// Input: 
    /// int keyID
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="keyID">The key id to add</param>
    public static void AddKey(int keyID)
    {
        keyIDs.Add(keyID);
    }

    /// <summary>
    /// Description:
    /// Tests whether the player has the key they need to open a door
    /// Input: 
    /// Door door
    /// Return: 
    /// bool
    /// </summary>
    /// <param name="door">The door being opened</param>
    /// <returns>bool: Whether or not the plyer has the door's key</returns>
    public static bool HasKey(Door door)
    {
        return keyIDs.Contains(door.doorID);
    }

    /// <summary>
    /// Description:
    /// Removes all keys from the keyring
    /// Input:
    /// none
    /// Return:
    /// void
    /// </summary>
    public static void ClearKeyRing()
    {
        keyIDs.Clear();
    }
}
