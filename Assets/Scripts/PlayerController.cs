using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [Header("Movimiento")]
    [SerializeField] private float _playerSpeed = 6;
    [SerializeField] private float _jumpForce = 6;

    [Header("Inputs")]

    private InputAction _moveInput;
    [SerializeField] private Vector2 _moveAction;
    private InputAction _jumpInput;
    [SerializeField] private Vector2 _jumpAction;



    void Awake()
    {
        _moveInput = InputSystem.actions["Move"];
        _jumpInput = InputSystem.actions["Jump"];
    }
    

    void Update()
    {
        _moveAction = _moveInput.ReadValue<Vector2>();
    }


}
