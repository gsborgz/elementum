using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [Header("Player Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float mana = 100f;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private Status status = Status.None;

    [Header("Player Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float crouchingWalkSpeed = 1f;
    [SerializeField] private float crouchingJumpSpeed = 6f;
    [SerializeField] private float crouchHeight = 0.8f;
    [SerializeField] private float gravity = 15f;

    [Header("State Settings")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isCrouching = false;
    [SerializeField] private bool hasSomethingAbove = false;
    [SerializeField] private bool isCastingSpell = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource footstepSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landSound;
    [SerializeField] private AudioSource crouchSound;

    private float originalHeight;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private PlayerListenerController listenerController;

    public bool IsCrouching
    {
        get => isCrouching;
        private set => isCrouching = value;
    }

    public bool IsCastingSpell
    {
        get => isCastingSpell;
        set => isCastingSpell = value;
    }

    public float Health => health;
    public float MaxHealth => maxHealth;
    public float Mana => mana;
    public float MaxMana => maxMana;
    public Status CurrentStatus => status;

    public void TakeDamage(float damage, float duration = 0)
    {
        float actualDamage = Mathf.Abs(damage);
        
        if (duration > 0)
        {
            StartCoroutine(TakeDamageOverTime(actualDamage, duration));
        }
        else
        {
            UpdateHealth(-actualDamage);
        }
    }

    public void ReduceMana(float amount)
    {
        float actualMana = Mathf.Abs(amount);

        mana -= actualMana;
        mana = Mathf.Clamp(mana, 0, maxMana);
    }

    public void Heal(float amount)
    {
        float actualHeal = Mathf.Abs(amount);

        UpdateHealth(actualHeal);
    }

    public void ApplyStatus(Status newStatus, float duration)
    {
        status = newStatus;

        switch (newStatus)
        {
            case Status.Burning:
                StartCoroutine(TakeDamageOverTime(1f, duration));
                break;
        }

        if (duration > 0)
        {
            StartCoroutine(RemoveStatusAfterDuration(duration));
        }
    }

    private void RestoreMana(float value)
    {
        float actualMana = Mathf.Abs(value);

        mana += actualMana;
        mana = Mathf.Clamp(mana, 0, maxMana);
    }

    private void UpdateHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    private IEnumerator RemoveStatusAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        status = Status.None;
    }

    private IEnumerator TakeDamageOverTime(float damage, float duration)
    {
        float timeElapsed = 0f;
        float interval = 1f;

        while (timeElapsed < duration)
        {
            UpdateHealth(-damage); // Negativo para reduzir health
            timeElapsed += interval;
            yield return new WaitForSeconds(interval);
        }
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalHeight = characterController.height;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            if (!hasSomethingAbove)
            {
                hasSomethingAbove = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasSomethingAbove)
        {
            hasSomethingAbove = false;
        }
    }

    private void Update()
    {
        UpdatePlayerStates();
        PlayerMovement();

        if (!IsCastingSpell && mana < maxMana)
        {
            RestoreMana(0.1f * Time.deltaTime);
        }
    }

    private void PlayerMovement()
    {
        WalkingRunningAndJumping();
        Crouching();
        PlaySounds();
    }

    private void WalkingRunningAndJumping()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        isSprinting = Input.GetButton("Sprint");
        isJumping = Input.GetButtonDown("Jump") && canJump;

        float speed = isSprinting ? runSpeed : (IsCrouching ? crouchingWalkSpeed : walkSpeed);
        float curSpeedX = canMove ? verticalAxis * speed : 0;
        float curSpeedY = canMove ? horizontalAxis * speed : 0;
        float movementDirectionY = moveDirection.y;

        isWalking = (verticalAxis != 0 || horizontalAxis != 0) && canMove && characterController.isGrounded;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (isJumping && canJump && characterController.isGrounded)
        {
            moveDirection.y = IsCrouching ? crouchingJumpSpeed : jumpSpeed;

            IsCrouching = false;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= (gravity + curSpeedY) * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Crouching()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            IsCrouching = !IsCrouching;
        }

        float targetHeight = IsCrouching ? crouchHeight : originalHeight;
        float currentHeight = characterController.height;

        // Só altera altura se necessário
        if (Mathf.Abs(currentHeight - targetHeight) > 0.01f)
        {
            if (IsCrouching)
            {
                // Agachando
                characterController.height = Mathf.MoveTowards(currentHeight, crouchHeight, 3f * Time.deltaTime);
            }
            else if (!hasSomethingAbove)
            {
                // Levantando (só se não houver obstáculo)
                characterController.height = Mathf.MoveTowards(currentHeight, originalHeight, 3f * Time.deltaTime);
            }
        }
    }

    private void UpdatePlayerStates()
    {
        if (!characterController.isGrounded)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void PlaySounds()
    {
        if (isWalking && !footstepSound.isPlaying)
        {
            if (!isSprinting && !IsCrouching)
            {
                // Walking normally
                SetFootstepSound(0.5f, 1f, 10f);
            }
            else if (IsCrouching)
            {
                // Crouching
                SetFootstepSound(0.1f, 0.7f, 1f);
            }
            else if (isSprinting && characterController.isGrounded)
            {
                // Sprinting
                SetFootstepSound(1f, 1.3f, 25f);
            }
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouchSound.Stop();
            crouchSound.Play();
        }

        if (isJumping && !jumpSound.isPlaying)
        {
            jumpSound.Play();
        }
    }

    private void SetFootstepSound(float volume, float pitch, float maxDistance)
    {
        footstepSound.volume = volume;
        footstepSound.pitch = pitch;
        footstepSound.panStereo = footstepSound.panStereo == 0.4f ? -0.4f : 0.4f;
        footstepSound.maxDistance = maxDistance;
        footstepSound.Play();
    }

}
