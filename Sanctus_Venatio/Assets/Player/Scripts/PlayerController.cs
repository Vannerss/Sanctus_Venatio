using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    [SerializeField]
    private float currentSpeed = 8.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4;
    [SerializeField]
    private float smoothInputSpeed = .2f;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    private bool groundedPlayer;
    private Vector3 playerVelocity;

    private InputManager inputManager;
    private CharacterController controller;

    private Transform cameraTransform;
    private Transform cameraRefTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraRefTransform = new GameObject().transform;
        inputManager = GetComponent<InputManager>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        cameraRefTransform.eulerAngles = new Vector3(0f, cameraTransform.eulerAngles.y, 0f); //a way to only get Y value from camera. prevents slowdown when looking up or down./
        groundedPlayer = controller.isGrounded;
        Vector2 movement = inputManager.GetPlayerMovement();

        HandleMovement(movement);
        HandleRotation(movement);
        HandleGravity();
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
}
