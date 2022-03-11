using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private Transform[] movePositions;
    private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    private Transform playerPos;

    [SerializeField]
    private Animator animator;

    private float attackCD;

    private Vector3 target;

    private BoxCollider bColl;
    private bool justChased = false;

    public int movePointIndex;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.Find("Player").transform;
        UpdateDestination();
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 10);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, 1.5f);
    }
    private void Update()
    {
        if(attackCD > 0f)
        {
            attackCD -= Time.deltaTime;
        }
        //navMeshAgent.destination = movePositionTransform.position;
        if(Vector3.Distance(this.transform.position, playerPos.position) <= 10)
        {
            if(Vector3.Distance(this.transform.position, playerPos.position) <= 1.5f && attackCD <= 0)
            {
                Attack();
            }
            else if
                (
                (animator.GetCurrentAnimatorStateInfo(0).IsName("NoAnim") || 
                (animator.GetCurrentAnimatorStateInfo(0).IsName("ArmSwing") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1))
                )
            {
                animator.SetBool("canAttack", false);
                bColl.enabled = false;
                if(Vector3.Distance(this.transform.position, playerPos.position) > 1.5f) Chase();
            }
        }
        else if(justChased)
        {
            UpdateDestination();
            Patrolling();
        }
        else
        {
            Patrolling();
        }
    }

    private void Patrolling()
    {
        if (Vector3.Distance(this.transform.position, target) < .1)
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


    private void Chase()
    {
        navMeshAgent.destination = playerPos.position;
        justChased = true;
    }

    private void Attack()
    {
        if(attackCD <= 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("ArmSwing"))
        {
            Debug.Log("I attacked");
            animator.SetBool("canAttack", true);
            bColl.enabled = true;

            attackCD = 3;
            navMeshAgent.destination = this.transform.position;
        }
    }
}
