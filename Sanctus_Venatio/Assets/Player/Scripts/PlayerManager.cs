using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private float currentHp;
    [SerializeField]
    private float attackDamage = 500;

    public CharacterController charcontroller;
    public GameObject deathUI;

    private float maxHp = 10000;
    private bool dead = false;

    private Image hpBar;


    void Start()
    {
        currentHp = maxHp;
        hpBar = GameObject.Find("Bar").GetComponent<Image>();
    }

    void Update()
    {
        UpdateUI();
        PassiveHeal(1.3f);
        if (dead)
        {
            Die();
        }
    }

    public float GetHp()
    {
        return currentHp;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && other.gameObject.layer == 7)
        {
            GameObject enemy = other.gameObject.gameObject.transform.parent.gameObject;
            //Debug.Log("Player Manager OTE (Player Takes Damage)\n" + "other ref: " + other + "\n other gO: " + other.gameObject + "\n other name: " + other.tag + "\n other layer: " + other.gameObject.layer );
            TakeDamage(enemy.GetComponent<EnemyManager>().GetAttackDamage());
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        if(currentHp <= 0)
        {
            dead = true;
        }
    }

    private void UpdateUI()
    {
        float temp = currentHp / maxHp;
        hpBar.fillAmount = currentHp / maxHp;
        //Debug.Log(temp);
    }
    public void PassiveHeal(float healAmount)
    {
        currentHp += healAmount;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }

    private void Die()
    {
        deathUI.SetActive(true);
        charcontroller.Move(new Vector3(0, 1, 0) * 2);
    }
}
