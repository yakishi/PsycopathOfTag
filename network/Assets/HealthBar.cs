using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour {

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    Text Life;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.ToString() == "bullet")
        {
            TakeDamage(10);
        }
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            Life = GameObject.Find("Life").GetComponent<Text>();
            Life.text = "Life:" + currentHealth.ToString();
        }
    }

    private void Update()
    {
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }
    }

    void OnChangeHealth(int health)
    {
        Life = GameObject.Find("Life").GetComponent<Text>();
        Debug.Log("ChangeHealth");
        if(Life != null)
        {
            if (isLocalPlayer)
            {
                Life.text = "Life:" + health.ToString();
            }
            
        }
        
    }
}
