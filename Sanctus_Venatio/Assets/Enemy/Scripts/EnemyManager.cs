using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int enemyHp = 1000;

    public void TakeDamage(int damage)
    {
        enemyHp -= damage;
        if(enemyHp <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
            
    }

    public void Attack()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
