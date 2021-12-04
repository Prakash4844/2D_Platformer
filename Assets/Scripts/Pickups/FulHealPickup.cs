using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FulHealPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        //Only heal when collided with Player
        if (collision.tag == "Player")
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            playerHealth.ReceiveHealing(playerHealth.maximumHealth);
            Destroy(this.gameObject);
        }
    }
}
