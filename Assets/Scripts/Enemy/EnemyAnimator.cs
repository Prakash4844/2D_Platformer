using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which handles enemy state translation to Animations
/// </summary>
public class EnemyAnimator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The enemy component responsible for tracking the enemy's state")]
    public EnemyBase enemyComponent = null;
    [Tooltip("The animator to use to animate the enemy")]
    public Animator enemyAnimator = null;

    [Header("Animator Parameter Names")]
    [Tooltip("The name of the boolean parameter in the animator which causes a transition to the idle state")]
    public string IdleAnimatorParameter = "isIdle";
    [Tooltip("The name of the boolean parameter in the animator which causes a transition to the walking/moving state")]
    public string MovingAnimatorParameter = "isWalking";
    [Tooltip("The name of the trigger parameter in the animator which causes a transition to the dead state")]
    public string DeadAnimatorParameter = "isDead";

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first update
    /// Used here to setup the animator on start
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void Start()
    {
        SetAnimatorState();
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
        SetAnimatorState();
    }

    /// <summary>
    /// Description:
    /// Sets the animator's state according to the enemy component specified
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    private void SetAnimatorState()
    {
        if (enemyComponent != null && enemyAnimator != null)
        {
            // Handle idle state
            if (enemyComponent.enemyState == EnemyBase.EnemyState.Idle)
            {
                enemyAnimator.SetBool(IdleAnimatorParameter, true);
            }
            else
            {
                enemyAnimator.SetBool(IdleAnimatorParameter, false);
            }

            // Handle moving state
            if (enemyComponent.enemyState == EnemyBase.EnemyState.Walking)
            {
                enemyAnimator.SetBool(MovingAnimatorParameter, true);
            }
            else
            {
                enemyAnimator.SetBool(MovingAnimatorParameter, false);
            }

            // Handle dead state
            if (enemyComponent.enemyState == EnemyBase.EnemyState.Dead)
            {
                enemyAnimator.SetBool(DeadAnimatorParameter, true);
            }
            else
            {
                enemyAnimator.SetBool(DeadAnimatorParameter, false);
            }
        }
    }
}
