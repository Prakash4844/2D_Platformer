using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class which sets an "isDead" trigger on the attatched animator
/// </summary>
[RequireComponent(typeof(Animator))]
public class DeathEffectAnimationHandler : MonoBehaviour
{
    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first update
    /// Calls SetIsDead to notify the animator that this is a death effect
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetIsDead();
    }

    /// <summary>
    /// Description:
    /// Sets a trigger in an attatched animator necessary for this script
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void SetIsDead()
    {
        GetComponent<Animator>().SetTrigger("isDead");
    }
}
