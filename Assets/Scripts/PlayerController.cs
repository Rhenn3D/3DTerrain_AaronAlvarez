using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    private CharacterController _characterController;
    private Animator _animator;
    private Transform _mainCamera;

    [Header("Ground Sensor")]
    [SerializeField] private Transform _sensor;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _sensorRadius = 0.3f;

    [Header("Movimiento")]
    [SerializeField] private float _playerSpeed = 6f;
    [SerializeField] private float _jumpForce = 6f;
    [SerializeField] private float _smoothTime = 0.1f;
    private float _turnSmoothVelocity;

    [Header("Gravedad")]
    [SerializeField] private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    [Header("Inputs")]
    private InputAction _moveInput;
    private Vector2 _moveAction;
   private InputAction _jumpInput;

    void Awake()
    {
        _moveInput = InputSystem.actions["Move"];
        _jumpInput = InputSystem.actions["Jump"];
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main.transform;
        _animator = GetComponent<Animator>();
    }





    void Update()
    {

        _moveInput = _moveAction.ReadValue<Vector2>();
        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        Movement();
        Gravity();

    }


     void Movement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        _animator.SetFloat("Vertical", direction.magnitude);
        _animator.SetFloat("Horizontal", 0);
        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            _characterController.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        _animator.SetBool("IsJumping", true);
        _playerGravity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
    }

    void Gravity()
    {
        bool grounded = IsGrounded();

        if (grounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2f;
            _animator.SetBool("IsJumping", false);
        }
        else
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }

        _characterController.Move(_playerGravity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensor.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos()
    {
        if (_sensor != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_sensor.position, _sensorRadius);
        }
    }
}