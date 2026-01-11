using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 500.0f;
    [SerializeField] float rotationSpeed = 10.0f;
    [SerializeField] float jumpHeight = 2.0f;
    [Header("Floor Check Properties")]
    [SerializeField] Vector3 floorCheckOffset;
    [SerializeField] float floorCheckRadius;
    [SerializeField] LayerMask floorLayer;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Animator animator;
    private CharacterController characterController;
    private bool isOnFloor;
    private float verticalSpeed;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfIsOnFloor();
        if (isOnFloor)
        {
            verticalSpeed = -0.5f;
        } else {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }
        HandleJump();
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveAmount = moveInput.magnitude;
        Vector3 movementDirection = (new Vector3(moveInput.x, 0, moveInput.y)).normalized;
        velocity = movementDirection * movementSpeed;
        velocity.y = verticalSpeed;
        characterController.Move(velocity * Time.deltaTime);
        if (moveAmount > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        animator.SetFloat("moveAmount", moveAmount, 0.1f, Time.deltaTime);
        animator.SetBool("isGrounded", isOnFloor);
    }
    private void CheckIfIsOnFloor()
    {
        isOnFloor = Physics.CheckSphere(transform.TransformPoint(floorCheckOffset), floorCheckRadius, floorLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.7f, 0.5f, 0.2f, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(floorCheckOffset), floorCheckRadius);
    }
    private void HandleJump()
    {
        if (jumpAction.triggered && isOnFloor)
        {
            verticalSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
            animator.SetTrigger("Jump");
        }
    }
}
