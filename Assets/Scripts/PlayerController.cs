using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{

    [Header("Ground Check")]
    [SerializeField] private Transform _sensor;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _sensorRadius = 2;

    [Header("Movimiento")]
    [SerializeField] private float _playerSpeed = 6;
    [SerializeField] private float _jumpForce = 6;
    [SerializeField] private float _smoothTime = 0.2f;
    private float _turnSmoothVelocity;

    [Header("Inputs")]

    private InputAction _moveInput;
    [SerializeField] private Vector2 _moveAction;
    private InputAction _jumpInput;
    [SerializeField] private Vector2 _jumpAction;
    [SerializeField] private Animator _animator;

    [Header("Gravedad")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;
    
    [Header("Otros")]
    private CharacterController characterController;
    private Transform _mainCamera;
    




    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        _moveInput = InputSystem.actions["Move"];
        _jumpInput = InputSystem.actions["Jump"];
        _animator = GetComponent<Animator>();

        _mainCamera = Camera.main.transform;
    }
    

    void Update()
    {
        _moveAction = _moveInput.ReadValue<Vector2>();

        if (_jumpInput.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        Movement();

        Gravity();
    }


   /* private void OnEnable()
    {
        _moveInput.Enable();
        _jumpInput.Enable();
    }
    private void OnDisable()
    {
        _moveInput.Disable();
        _jumpInput.Disable();
    }*/

    void Gravity()
    {
        if (!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
            _animator.SetBool("IsJumping", false);
        }
        

        characterController.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()
    {
        _animator.SetBool("IsJumping", true);
        _playerGravity.y = Mathf.Sqrt(_jumpForce * -2 * _gravity);
        characterController.Move(_playerGravity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensor.position, _sensorRadius, _groundLayer);

    }


    

    void Movement()
    {
        Vector3 direction = new Vector3(_moveAction.x, 0, _moveAction.y);

        _animator.SetFloat("Vertical", _moveAction.y);
        _animator.SetFloat("Horizontal", _moveAction.x);
        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensor.position, _sensorRadius);
    }
}
