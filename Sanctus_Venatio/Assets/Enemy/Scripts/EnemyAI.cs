using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{   
    public Animator animator;
    public BoxCollider bColl;
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Transform[] movePositions;
    private Transform playerPos;

    private Vector3 target;

    public int movePointIndex;
    private float attackCD;
    private float collDisablerTime = 0f;
    private bool justChased = false;

    private void Start()
    {
        bColl = this.transform.GetChild(0).GetComponent<BoxCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.Find("Player").transform;
        //UpdateDestination();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 10);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, 1.2f);
    }

    private void Update()
    {
        if(attackCD > 0f)
        {
            attackCD -= Time.deltaTime;
        }

        if(bColl.enabled && !animator.GetCurrentAnimatorStateInfo(0).IsName("ArmSwing"))
        {
            if(collDisablerTime > 0f)
            {
                collDisablerTime -= Time.deltaTime;
            } 
            else if (collDisablerTime <= 0f)
            {
                bColl.enabled = false;
            }
        }
        
        if(Vector3.Distance(this.transform.position, playerPos.position) <= 10)
        {
            if(Vector3.Distance(this.transform.position, playerPos.position) <= 1.5f && attackCD <= 0)
            {
                Attack();
            }
            else if ((animator.GetCurrentAnimatorStateInfo(0).IsName("ArmSwing") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.1f) || animator.GetCurrentAnimatorStateInfo(0).IsName("NoAnim"))
            {
                animator.SetBool("canAttack", false);
                if(Vector3.Distance(this.transform.position, playerPos.position) > 1.5f) Chase();
            }
        }
        else if(justChased)
        {
            //UpdateDestination();
            //Patrolling();
        }
        else
        {
            //Patrolling();
        }
    }

    #region Patrolling
    private void Patrolling()
    {
        if (Vector3.Distance(this.transform.position, target) < 0.1)
        {
            justChased = false;
            IterateMovePointIndex();
            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        target = movePositions[movePointIndex].transform.position;
        navMeshAgent.destination = target;
    }

    private void IterateMovePointIndex()
    {
        movePointIndex++;
        if(movePointIndex == movePositions.Length)
        {
            movePointIndex = 0;
        }
    }
    #endregion
    private void Chase()
    {
        navMeshAgent.destination = playerPos.position;
        justChased = true;
    }

    private void Attack()
    {
        if(attackCD <= 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("ArmSwing"))
        {
            //Debug.Log("I attacked");

            bColl.enabled = true;

            animator.SetBool("canAttack", true);
            

            attackCD = 3;
            collDisablerTime = 0.2f;
            navMeshAgent.destination = this.transform.position;
        }
    }
}
