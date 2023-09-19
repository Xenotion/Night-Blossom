using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    
    public void TakeDamage(int damageAmount)
    {   
        Debug.Log("Player attacked");

        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("Player died");
    }
}

