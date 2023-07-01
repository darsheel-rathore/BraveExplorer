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
    private const float _GRAVITY = -9.8f;

    // Start is called before the first frame update
    void Awake()
    {
        
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovment();

        // Move the player
        characterController.Move(movementVelocity);
    }

    private void CalculatePlayerMovment()
    {
        // Grab axis movement
        movementVelocity.Set(playerInput.horizontalInput, 0f, playerInput.verticalInput);
        // Normalize the vectors
        movementVelocity.Normalize();

        // Change the animation type
        animator.SetFloat("Speed", movementVelocity.magnitude);

        // Dont know what this is
        movementVelocity = Quaternion.Euler(0f, -45f, 0f) * movementVelocity;

        // Add some speed
        movementVelocity = movementVelocity * moveSpeed * Time.deltaTime;

        // Rotation
        // Check if player is moving
        if (movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementVelocity);
           
        }

        // Caculate player vertical movement GRAVITY
        verticalSpeed = characterController.isGrounded ? _GRAVITY * 0.3f : _GRAVITY;

        movementVelocity += verticalSpeed * Vector3.up * Time.deltaTime;
    }


}
