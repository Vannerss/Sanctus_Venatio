using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    int damage = 500;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Enemy")
        {
            Debug.Log("Was Triggered.");
            EnemyStats enemy = other.gameObject.GetComponent<EnemyStats>();
            enemy.ReceiveDamage(damage);
        }
    }
}
