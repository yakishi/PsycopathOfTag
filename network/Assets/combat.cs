using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class combat : NetworkBehaviour {

    public const int maxHealth = 100;

    public int health = maxHealth;

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            if (!isServer)
            {
                return;
            }

            health = 0;  
        }        
    }
}
