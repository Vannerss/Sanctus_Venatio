using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PlayerController : MonoBehaviour
{   
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference attackControl;
    [SerializeField]
    private InputActionReference sprintControl;
    [SerializeField]
    private float walkSpeed = 8.0f;
    //[SerializeField]
    //private float sprintSpeed = 12.0f;
    //[SerializeField]
    //private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4;
    [SerializeField]
    private float smoothInputSpeed = .2f;
    [SerializeField]
    private float currentSpeed = 0f;
    
    private int attackOrder = 0;

    private CharacterController controller;
    private BoxCollider swordBoxCollider;

    public Animator animator;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Transform cameraTransform;
    private Transform cameraRefTransform;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    private void OnEnable()
    {
        movementControl.action.Enable();
        attackControl.action.Enable();
        sprintControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        attackControl.action.Disable();
        sprintControl.action.Disable();

    }

    private void Start()
    {

        swordBoxCollider = GameObject.Find("Broken Sword").GetComponent<BoxCollider>();
        swordBoxCollider.enabled = false;
        controller = GetComponent<CharacterController>();
        cameraRefTransform = new GameObject().transform;
        cameraTransform = Camera.main.transform;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        cameraRefTransform.eulerAngles = new Vector3(0f, cameraTransform.eulerAngles.y, 0f); //a way to only get Y value from camera. prevents slowdown when looking up or down./
        groundedPlayer = controller.isGrounded;
        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        if (animator.GetBool("isAttacking") != true)
        {
            HandleMovement(movement);
            HandleRotation(movement);
        }
        else if(animator.GetBool("isAttacking") == true)
        { 
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Stable Sword Outward Slash") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stable Sword Inward Slash")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                animator.SetBool("isAttacking", false);
                animator.SetInteger("attackValue", 0);
                swordBoxCollider.enabled = false;
            }
        }
        HandleGravity();
        HandleAnimation(movement);
    }
    void HandleMovement(Vector2 movement)
    {
        currentInputVector = Vector2.SmoothDamp(currentInputVector, movement, ref smoothInputVelocity, smoothInputSpeed); //smooth acceleration/de-acceleration
        Vector3 move = new Vector3(currentInputVector.x, 0, currentInputVector.y);
        move = cameraRefTransform.forward * move.z + cameraRefTransform.right.normalized * move.x;
        move.y = 0;
       
        controller.Move(move * Time.deltaTime * currentSpeed);
    }
    void HandleGravity()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        /*
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }*/
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    void HandleRotation(Vector2 movement)
    {
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraRefTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
    void HandleAnimation(Vector2 movement)
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isAttacking = animator.GetBool("isAttacking");
        if(movement == Vector2.zero && isAttacking == false)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
        if(movement != Vector2.zero && isAttacking == false)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
        if(attackControl.action.triggered)
        {
            Debug.Log(attackOrder);
            attackOrder += 1;
            if(attackOrder > 2)
            {
                attackOrder = 1;
            }
            animator.SetBool("isAttacking", true);
            Debug.Log(attackOrder);
            animator.SetInteger("attackValue", attackOrder);
            movement = Vector2.zero;
            swordBoxCollider.enabled = true;

        }
    }
}
