using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController controller;

    public Transform cam;

    private InputAction moveAction;

    public float playerSpeed = 5;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    public Vector2 currentInputVector;
    public Vector2 smoothInputVelocity;
    public float smoothInputSpeed = 0.2f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];
    }

    void Update()
    {
        //print(moveAction.ReadValue<Vector2>());

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 direction = new Vector3(currentInputVector.x, 0, currentInputVector.y);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            print(direction);
            controller.Move(moveDir * Time.deltaTime * playerSpeed);
        }
    }
}
