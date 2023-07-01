using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 movementVelocity;

    private CharacterController characterController;
    private PlayerInput playerInput;
    private Animator animator;
    private float verticalSpeed;
    private const float _GRAVITY = -20f;
    private float gravityModifier = 0.3f;

    // Start is called before the first frame update
    void Awake()
    {
        // Initializing Fields
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovment();                       // Calculates player horizontal movement
        CalculateRotation();                            // Calculates Rotation 
        CheckPlayerFall();                              // Change the animation state to falling

        characterController.Move(movementVelocity);     // Finally moves the player in the desired direction
    }

    private void CalculatePlayerMovment()
    {
        // Grab axis movement
        movementVelocity.Set(playerInput.horizontalInput, 0f, playerInput.verticalInput);

        // Normalize the vectors
        movementVelocity.Normalize();

        // Check if player is running
        ChangeAnimState("Speed", movementVelocity.magnitude);

        // Dont know what this is
        movementVelocity = Quaternion.Euler(0f, -45f, 0f) * movementVelocity;

        // Add some speed
        movementVelocity = movementVelocity * moveSpeed * Time.deltaTime;
    }

    private void CalculateRotation()
    {
        // Only rotate when player is moving or not in idel position
        if (movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movementVelocity);
    }

    private void CheckPlayerFall()
    {
        // Assign gravity
        verticalSpeed = characterController.isGrounded ? 
            _GRAVITY * gravityModifier : _GRAVITY;

        // Storing the Y axis value in the global movement velocity field
        movementVelocity += verticalSpeed * Vector3.up * Time.deltaTime;

        // Changing animation state when required
        ChangeAnimState("Airborne", !characterController.isGrounded);
    }

    private void ChangeAnimState<T>(string stateName, T value)
    {
        switch (value)
        {
            case float floatValue:
                animator.SetFloat(stateName, floatValue);
                break;
            case bool boolValue:
                animator.SetBool(stateName, boolValue);
                break;
            case int intValue:
                animator.SetInteger(stateName, intValue);
                break;
            default:
                Debug.LogError("Unsupported parameter type");
                break;
        }
    }
}
