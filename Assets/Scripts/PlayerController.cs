using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 500.0f;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Animator animator;
    private CharacterController characterController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float moveAmount = moveInput.magnitude;
        Vector3 moveDirection = (new Vector3(moveInput.x, 0, moveInput.y)).normalized;
        if (moveAmount > 0.0f)
        {
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
        animator.SetFloat("moveAmount", moveAmount, 0.1f, Time.deltaTime);
    }
}
