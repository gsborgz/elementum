using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    CrouchingIdle,
    CrouchingWalk,
    CrouchingRun,
    Jumping,
}

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float crouchingWalkSpeed = 1f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float gravity = 15f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 3f;

    [Header("State Settings")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private PlayerState currentState = PlayerState.Idle;

    private float originalHeight;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private bool isCrouchingInput = false;

    public bool IsCrouching
    { 
        get
        {
            return isCrouchingInput;
        }
    }

    public PlayerState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        originalHeight = characterController.height;
    }

    private void Update()
    {
        HandleInput();
        UpdateMovement();
        UpdateCrouchingHeight();
        SetPlayerState();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouchingInput = !isCrouchingInput;
        }

        if (Input.GetButtonDown("Jump") && canJump && isCrouchingInput)
        {
            isCrouchingInput = false;
        }
    }

    private void UpdateMovement()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        bool isSprinting = Input.GetButton("Sprint");
        bool isJumping = Input.GetButtonDown("Jump") && canJump && characterController.isGrounded;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float currentSpeed = GetCurrentSpeed(verticalAxis != 0 || horizontalAxis != 0, isSprinting);
        
        float curSpeedX = canMove ? verticalAxis * currentSpeed : 0;
        float curSpeedY = canMove ? horizontalAxis * currentSpeed : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (isJumping)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private float GetCurrentSpeed(bool isMoving, bool isSprinting)
    {
        if (!isMoving) return 0f;

        if (isCrouchingInput)
        {
            return isSprinting ? runSpeed * 0.7f : crouchingWalkSpeed;
        }
        
        return isSprinting ? runSpeed : walkSpeed;
    }

    private void UpdateCrouchingHeight()
    {
        float targetHeight = isCrouchingInput ? crouchHeight : originalHeight;
        float currentHeight = characterController.height;

        if (Mathf.Abs(currentHeight - targetHeight) > 0.01f)
        {
            characterController.height = Mathf.MoveTowards(currentHeight, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        }
    }

    private void SetPlayerState()
    {
        canJump = characterController.isGrounded;

        if (!characterController.isGrounded)
        {
            currentState = PlayerState.Jumping;
            return;
        }

        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        bool isMoving = (verticalAxis != 0 || horizontalAxis != 0) && canMove;
        bool isSprinting = Input.GetButton("Sprint");

        if (isCrouchingInput)
        {
            if (!isMoving)
            {
                currentState = PlayerState.CrouchingIdle;
            }
            else if (isSprinting)
            {
                currentState = PlayerState.CrouchingRun;
            }
            else
            {
                currentState = PlayerState.CrouchingWalk;
            }
        }
        else
        {
            if (!isMoving)
            {
                currentState = PlayerState.Idle;
            }
            else if (isSprinting)
            {
                currentState = PlayerState.Running;
            }
            else
            {
                currentState = PlayerState.Walking;
            }
        }
    }

}
