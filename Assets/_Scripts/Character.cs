using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 movementVelocity;
    public Health health;

    private CharacterController characterController;
    private PlayerInput playerInput;
    private Animator animator;
    private float verticalSpeed;
    private const float _GRAVITY = -20f;
    private float gravityModifier = 0.3f;

    // Enemy
    public bool isPlayer = true;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform targetPos;
    private CharacterState currentState;

    // Player Slides
    private float attackStartTime;
    public float attackSlideDuration = 0.4f;
    public float attackSlideSpeed = 0.06f;

    // Enums
    public enum CharacterState
    {
        NORMAL, ATTACKING
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Initializing Fields
        animator = GetComponent<Animator>();

        if (!isPlayer)
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            navMeshAgent.speed = moveSpeed;
            targetPos = GameObject.FindWithTag("Player").transform;
        }
        else
        {
            playerInput = GetComponent<PlayerInput>();
            characterController = GetComponent<CharacterController>();
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.NORMAL:
                if (isPlayer)
                {
                    CalculatePlayerMovment();                       // Calculates player horizontal movement
                }
                else
                {
                    CalculateEnemyMovement();
                }
                break;

            case CharacterState.ATTACKING:
                if (isPlayer)
                {
                    movementVelocity = Vector3.zero;

                    if (Time.time < (attackStartTime + attackSlideDuration))
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;

                        movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                    }
                }
                else
                {
                    //Quaternion newRot = Quaternion.LookRotation(GameObject.FindWithTag("Player").transform.position - transform.position);
                    //transform.rotation = newRot;
                }
                break;
        }

        if (isPlayer)
        {
            CheckPlayerFall();                              // Change the animation state to falling
            characterController.Move(movementVelocity);     // Finally moves the player in the desired direction
        }
    }

    private void CalculatePlayerMovment()
    {
        if (playerInput.mouseBtnDown && characterController.isGrounded)
        {
            SwitchStateTo(CharacterState.ATTACKING);
            return;
        }

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

        CalculateRotation();                            // Calculates Rotation 
    }

    private void CalculateEnemyMovement()
    {
        float distance = Vector3.Distance(targetPos.position, transform.position);

        if (distance > navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(targetPos.position);
            ChangeAnimState("Speed", 0.2f);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
            ChangeAnimState("Speed", 0f);

            SwitchStateTo(CharacterState.ATTACKING);
        }
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
        movementVelocity = movementVelocity + (verticalSpeed * Vector3.up * Time.deltaTime);

        // Changing animation state when required
        ChangeAnimState("Airborne", !characterController.isGrounded);
    }

    private void SwitchStateTo(CharacterState newState)
    {
        if (isPlayer)
        {
            // Clear cache
            playerInput.mouseBtnDown = false;
        }

        // Current State
        //switch(currentState)
        //{
        //    case CharacterState.NORMAL:
        //        break;
        //    case CharacterState.ATTACKING:
        //        break;
        //}

        // New State
        switch (newState)
        {
            case CharacterState.NORMAL:
                break;
            case CharacterState.ATTACKING:

                if (!isPlayer)
                {
                    Quaternion newRot = Quaternion.LookRotation(targetPos.position - transform.position);
                    transform.rotation = newRot;
                }

                animator.SetTrigger("Attack");

                if (isPlayer)
                {
                    attackStartTime = Time.time;
                }
                break;
        }

        currentState = newState;
    }

    // Animation
    private void ChangeAnimState<T>(string stateName, T value = default) where T : struct
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
                animator.SetTrigger(stateName);
                break;
        }
    }

    public void AttackAnimationEnds() => SwitchStateTo(CharacterState.NORMAL);

    public void ApplyDamage(int damage, Vector3 attackPos = new Vector3())
    {
        if(health != null)
        {
            health.ApplyDamage(damage);
        }
    }
}
