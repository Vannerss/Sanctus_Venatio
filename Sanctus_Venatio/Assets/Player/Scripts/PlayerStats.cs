using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int maxHealth = 1000;
    private int maxStamina = 500;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int currentStamina;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }


    private void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Trigger Death Screen (restart and quit options).
    }
}
