using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to allow buttons to interact with the static Level Manager
/// </summary>
public class LevelSwitcher : MonoBehaviour
{
    /// <summary>
    /// Description:
    /// Passes a string to the Level Manager, which loads a scene using that name
    /// Input:
    /// string sceneName
    /// Return:
    /// void (no return)
    /// </summary>
    /// <param name="sceneName">The name of the scene to be loaded</param>
    public void LoadScene(string sceneName)
    {
        LevelManager.LoadScene(sceneName);
    }
}
