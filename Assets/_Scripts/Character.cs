using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Fields, Enums

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

    // Coin
    public int coin;

    // combo VFX
    public float attackAnimDuration;

    // Player Slides
    private float attackStartTime;
    public float attackSlideDuration = 0.4f;
    public float attackSlideSpeed = 0.06f;

    // Damage Caster
    private DamageCaster damageCaster;

    // Material Animation
    private MaterialPropertyBlock materialPropertyBlock;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    // Item To Drop
    public GameObject itemToDrop;

    // Invincible Player
    public bool IsInvincible;
    public float invincibleDuration = 2f;

    // Sliding
    public float slideSpeed = 9f;

    // Enums
    public enum CharacterState
    {
        NORMAL, ATTACKING, DEAD, BEINGHIT, SLIDE
    }
    #endregion

    void Awake()
    {
        // Initializing Fields
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();    

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(materialPropertyBlock);
        damageCaster = GetComponentInChildren<DamageCaster>();

        characterController = GetComponent<CharacterController>();

        if (!isPlayer)
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            navMeshAgent.speed = moveSpeed;
            targetPos = GameObject.FindWithTag("Player").transform;
        }
        else
        {
            playerInput = GetComponent<PlayerInput>();
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
                    // Performing DASH Feature
                    if (Time.time < (attackStartTime + attackSlideDuration))
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;

                        movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                    }

                    if(playerInput.mouseBtnDown && characterController.isGrounded)
                    {
                        // Grab animation name
                        string currentAnimClipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        // Grab current animation duration
                        // NormalizedTime value will be 0 at the start of the animation and 1 at the end
                        attackAnimDuration = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                        if(currentAnimClipName != "LittleAdventurerAndie_ATTACK_03" && attackAnimDuration > 0.5f && attackAnimDuration < 0.7f)
                        {
                            playerInput.mouseBtnDown = false;
                            // Will play the attack animation, this is how the designing done in the state machine
                            SwitchStateTo(CharacterState.ATTACKING);

                            CalculatePlayerMovment();
                        }
                    }
                }
                break;

            case CharacterState.DEAD:
                return;

            case CharacterState.BEINGHIT:
                break;

            case CharacterState.SLIDE:
                movementVelocity = transform.forward * slideSpeed * Time.deltaTime;
                break;
        }

        if (isPlayer)
        {
            CheckPlayerFall();                              // Change the animation state to falling
            characterController.Move(movementVelocity);     // Finally moves the player in the desired direction
            movementVelocity = Vector3.zero;
        }
    }

    #region Movement(Run, Jump, Fall)
    private void CalculatePlayerMovment()
    {
        if (playerInput.mouseBtnDown && characterController.isGrounded)
        {
            SwitchStateTo(CharacterState.ATTACKING);
            return;
        }
        else if (playerInput.spaceKeyDown && characterController.isGrounded)
        {
            SwitchStateTo(CharacterState.SLIDE);
            return;
        }

        // Grab axis movement
        movementVelocity.Set(playerInput.horizontalInput, 0f, playerInput.verticalInput);

        // Normalize the vectors
        movementVelocity.Normalize();

        // Check if player is running
        ChangeAnimState<object>("Speed", movementVelocity.magnitude);

        // Dont know what this is
        movementVelocity = Quaternion.Euler(0f, -45f, 0f) * movementVelocity;

        // Add some speed
        movementVelocity = movementVelocity * moveSpeed * Time.deltaTime;

        CalculateRotation();                            // Calculates Rotation 
    }

    private void CalculateEnemyMovement()
    {
        // Calculate the distance between the enemy and the target position
        float distance = Vector3.Distance(targetPos.position, transform.position);

        // Check if the enemy is far from the target
        if (distance > navMeshAgent.stoppingDistance)
        {
            // Move the enemy towards the target position
            navMeshAgent.SetDestination(targetPos.position);

            // Change the animation state to indicate movement
            ChangeAnimState<object>("Speed", 0.2f);
        }
        else
        {
            // Stop the enemy's movement since it is close enough to the target
            navMeshAgent.SetDestination(transform.position);

            // Stop the movement animation
            ChangeAnimState<object>("Speed", 0f);

            // Switch the character state to attacking mode
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
        ChangeAnimState<object>("Airborne", !characterController.isGrounded);
    }
    #endregion

    public void SwitchStateTo(CharacterState newState)
    {
        if (isPlayer)
        {
            // Clear cache
            playerInput.ClearCache();
        }

        // Exiting State
        switch (currentState)
        {
            case CharacterState.NORMAL:
                break;

            case CharacterState.ATTACKING:

                if (damageCaster != null)
                    DisableDamageCaster();

                if (isPlayer)
                    GetComponent<PlayerFXManager>().StopBlade();
                break;
            
            case CharacterState.DEAD:
                return;

            case CharacterState.BEINGHIT:
                break;

            case CharacterState.SLIDE:
                break;
        }

        // Entering State
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

                // Change the animation state to attack
                ChangeAnimState<object>("Attack", null);

                if (isPlayer)
                {
                    attackStartTime = Time.time;
                }
                break;

            case CharacterState.DEAD:
                characterController.enabled = false;

                ChangeAnimState<object>("Dead", null);
                StartCoroutine(MaterialDissolve());
                break;

            case CharacterState.BEINGHIT:
                ChangeAnimState<object>("BeingHit", null);
                if (isPlayer)
                {
                    IsInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
                }
                break;

            case CharacterState.SLIDE:
                ChangeAnimState<object>("Slide");
                break;

        }

        currentState = newState;
    }

    #region Animation
    private void ChangeAnimState<T>(string stateName, T value = null) where T : class
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
                if (value == null)
                {
                    animator.SetTrigger(stateName);
                }
                break;
        }
    }

    public void AttackAnimationEnds() => SwitchStateTo(CharacterState.NORMAL);
    #endregion

    #region Health/Damage
    public void ApplyDamage(int damage, Vector3 attackPos = new Vector3())
    {
        if (IsInvincible)
            return;

        if (health != null)
        {
            health.ApplyDamage(damage);
        }

        if(!isPlayer)
        {
            GetComponent<EnemyVFXManager>().BeingHit(attackPos);
        }

        if (isPlayer)
            SwitchStateTo(CharacterState.BEINGHIT);

        StartCoroutine(MaterialBlink());
    }

    public void EnableDamageCaster() => damageCaster.EnableDamageCaster();

    public void DisableDamageCaster() => damageCaster.DisableDamageCaster();
    #endregion

    public void BeingHitAnimationEnds() => SwitchStateTo(CharacterState.NORMAL);

    IEnumerator MaterialBlink()
    {
        materialPropertyBlock.SetFloat("_blink", 0.8f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);

        yield return new WaitForSeconds(0.2f);

        materialPropertyBlock.SetFloat("_blink", 0);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float dissolveHeight_start = 20f;
        float dissolveHeight_target = -10f;
        float dissolveHeight;

        materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);

        while(currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHeight = Mathf.Lerp(dissolveHeight_start, dissolveHeight_target, currentDissolveTime / dissolveTimeDuration);

            materialPropertyBlock.SetFloat("_dissolve_height", dissolveHeight);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }

        DropItem();
    }

    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(2f);
        IsInvincible = false;
    }

    // Drop Healing ORB
    public void DropItem()
    {
        if (itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }

    public void PickUpItem(PickUp item)
    {
        switch(item.type)
        {
            case PickUp.PickUpType.HEAL:
                AddHealth(item.value);
                break;

            case PickUp.PickUpType.COIN:
                AddCoin(item.value);
                break;
        }
    }

    public void AddHealth(int health)
    {
        this.health.AddHealth(health);
        // Play heal orb vfx
        GetComponent<PlayerFXManager>().Heal();
    }

    public void AddCoin(int coin)
    {
        this.coin += coin;
    }

    public void SlideAnimationEnds()
    {
        SwitchStateTo(CharacterState.NORMAL);
    }

    public void RotateToTarget()
    {
        if(currentState != CharacterState.DEAD)
        {
            transform.LookAt(targetPos, Vector3.up);
        }
    }

}
