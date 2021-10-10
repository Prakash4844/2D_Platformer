using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains settings for and handles the control of an enemy
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How fast this enemy moves")]
    public float moveSpeed = 2f;

    /// <summary>
    /// Enum to track which state the enemy is in
    /// </summary>
    public enum EnemyState { Walking, Dead, Idle }

    [Tooltip("The state the enemy is in for animation playback")]
    public EnemyState enemyState;

    /// <summary>
    /// Description:
    /// Standard Unity function called once before update
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected virtual void Start()
    {
        Setup();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected virtual void Update()
    {
        // Every frame, get the desired movement of this enemy, then move it.
        Vector3 movement = GetMovement();
        MoveEnemy(movement);
    }

    /// <summary>
    /// Description:
    /// Sets up this enemy
    /// Input: 
    /// none
    /// Return: 
    /// void (no return)
    /// </summary>
    protected virtual void Setup()
    {

    }

    /// <summary>
    /// Description:
    /// Returns the desired movement for this frame
    /// Input: none
    /// Return: 
    /// Vector3
    /// </summary>
    /// <returns>Vector3: The vector representing the movement this enemy will take this frame</returns>
    protected virtual Vector3 GetMovement()
    {
        return Vector3.zero;
    }

    /// <summary>
    /// Description:
    /// Moves the enemy according to a movement vector
    /// Input: 
    /// Vector3 movement
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="movement">The vector representing the movement this enemy will make.</param>
    protected virtual void MoveEnemy(Vector3 movement)
    {
        transform.position = transform.position + movement;
    }
}
