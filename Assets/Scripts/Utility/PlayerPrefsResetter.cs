using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class uses the game manager's reset game player prefs function to reset the score, health, and lives player prefs
/// </summary>
public class PlayerPrefsResetter : MonoBehaviour
{

    /// <summary>
    /// Description:
    /// Calls the GameManger Reset Score function to reset the score player preference data
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    public void ResetGamePlayerPrefs()
    {
        GameManager.ResetGamePlayerPrefs();
    }
}
