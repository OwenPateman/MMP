using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class Player_Input_Handler : NetworkBehaviour
{
    Rigidbody2D m_RB;
    InputSystem_Actions m_PlayerActions;
    Vector2 moveInput;

    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;


    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_PlayerActions = new InputSystem_Actions();

        m_PlayerActions.Enable();

        m_PlayerActions.Player.Jump.performed += Handle_JumpPressed;
        m_PlayerActions.Player.Move.performed += Handle_MovePerformed;
        m_PlayerActions.Player.Move.canceled += Handle_MoveCanceled;
    }

    private void OnDisable()
    {
        m_PlayerActions.Player.Jump.performed -= Handle_JumpPressed;
        m_PlayerActions.Player.Move.performed -= Handle_MovePerformed;
        m_PlayerActions.Player.Move.canceled -= Handle_MoveCanceled;
        m_PlayerActions.Disable();
    }

    void Handle_JumpPressed(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        m_RB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Handle_MovePerformed(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        moveInput = ctx.ReadValue<Vector2>(); 
    }

    private void Handle_MoveCanceled(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        moveInput = Vector2.zero; 
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        // Apply horizontal movement
        Vector2 velocity = m_RB.linearVelocity;
        velocity.x = moveInput.x * moveSpeed;
        m_RB.linearVelocity = velocity;
    }

}
