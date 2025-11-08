using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    [Header ("Physics")]
    [SerializeField] private float _gravityScale = 1;

    [Header("Movimiento")]
    [SerializeField] private float _playerSpeed = 6;
    [SerializeField] private float _jumpForce = 6;

    [Header("Inputs")]

    private InputAction _moveInput;
    [SerializeField] private Vector2 _moveAction;
    private InputAction _jumpInput;
    [SerializeField] private Vector2 _jumpAction;
    [SerializeField] private Animator _animator;
    
    




    void Awake()
    {
        _moveInput = InputSystem.actions["Move"];
        _jumpInput = InputSystem.actions["Jump"];
    }
    

    void Update()
    {
        _moveAction = _moveInput.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(_moveAction.x, 0, _moveAction.y);
        transform.Translate(move * _playerSpeed * Time.fixedDeltaTime, Space.World);
        _animator.SetFloat("Speed", move.magnitude);
    }

    private void OnEnable()
    {
        _moveInput.Enable();
        _jumpInput.Enable();
    }
    private void OnDisable()
    {
        _moveInput.Disable();
        _jumpInput.Disable();
    }

    void Jump()
    {
        if ()
        {
            if (hit.collider.CompareTag("Ground"))
            {

                _animator.SetBool("isJumping", true);
            }
        }
    }






}
