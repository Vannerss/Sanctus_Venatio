using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private float enemyHp = 1000;
    [SerializeField]
    private float attackDamage = 2000;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword" && other.gameObject.layer == 6)
        {
            Debug.Log("Enemy Manager OTE (Enemy Takes Damage)\n" + "other ref: " + other + "\n other gO: " + other.gameObject + "\n other name: " + other.tag + "\n other layer: " + other.gameObject.layer);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            TakeDamage(player.GetComponent<PlayerManager>().GetAttackDamage());
        }
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public void TakeDamage(float damage)
    {
        enemyHp -= damage;
        if(enemyHp <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
