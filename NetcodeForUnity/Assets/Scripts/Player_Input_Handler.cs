using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[SelectionBase]
public class Player_Input_Handler : NetworkBehaviour
{
    Rigidbody m_RB;
    InputSystem_Actions m_PlayerActions;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_PlayerActions = new InputSystem_Actions();

        m_PlayerActions.Enable();

        m_PlayerActions.Player.Jump.performed += Handle_JumpPressed;
    }

    private void OnDisable()
    {
        m_PlayerActions.Player.Jump.performed -= Handle_JumpPressed;
    }

    void Handle_JumpPressed(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        m_RB.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }


}
