using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    private InputManager inputManager;

    private static int Clicks = 0;

    private float lastClickedTime = 0;

    public BoxCollider bcoll;


    void Start()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        MovementAnimation();

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7 && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
        {
            animator.SetBool("Attack1", false);
            bcoll.enabled = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7 && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_2"))
        {
            animator.SetBool("Attack2", false);
            bcoll.enabled = false;
            Clicks = 0;
        }

        if (Time.time - lastClickedTime > 1)
        {
            Clicks = 0;
        }
        if (Time.time > 0)
        {
            if (inputManager.PlayerAttackedThisFrame() == true)
            {
                AttackQueued();
            }
        }
    }

    void AttackQueued()
    {
        Clicks++;
        lastClickedTime = Time.time;

        if (Clicks == 1)
        {
            animator.SetBool("Attack1", true);
            bcoll.enabled = true;
        }

        Clicks = Mathf.Clamp(Clicks, 0, 2);

        if(Clicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3 && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
        {
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", true);
            bcoll.enabled = true;
        }
    }

    void MovementAnimation()
    {
        Vector2 movement = inputManager.GetPlayerMovement();
        if (movement != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}
