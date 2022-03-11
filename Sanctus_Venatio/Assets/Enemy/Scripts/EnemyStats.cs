using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private int HP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveDamage(int dmg)
    {
        HP = Manager.instance.enemyHp;
        HP -= dmg;
        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("EnemyDied");
    }
}
