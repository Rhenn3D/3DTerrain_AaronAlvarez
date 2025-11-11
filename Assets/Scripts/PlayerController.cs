using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    private CharacterController _characterController;

    [Header("Ground Sensor")]
    [SerializeField] private Transform _sensor;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _sensorRadius = 2f;

    [Header("Gravedad")]
    [SerializeField] private float _gravityScale = 9.81f;
    [SerializeField] private Vector3 _playerGravity;

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

    [Header("Gravedad")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;

    [Header("Otros")]
    private CharacterController characterController;
    private Transform _mainCamera;





    [Header("Fuerzas y Velocidades")]
    [SerializeField] private float _playerSpeed = 6;
    [SerializeField] private float _jumpForce = 6;
    [SerializeField] private float _smoothTime = 0.1f;
    private float _turnSmoothVelocity;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();


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

        Gravity();

        Movement();
    }

    void FixedUpdate()
    {
        
    }


    void Gravity()
    {
        bool grounded = IsGrounded();

        if (grounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -2f; // mantiene al jugador en el suelo
            if (_animator.GetBool("IsJumping")) 
                _animator.SetBool("IsJumping", false);
        }
        else
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }

        _characterController.Move(_playerGravity * Time.deltaTime);
    }



    /* private void OnEnable()

     {
         Vector3 direction = new Vector3(_moveAction.x, 0, _moveAction.y);

         _animator.SetFloat("Vertical", direction.magnitude);
         _animator.SetFloat("Horizontal", 0);
         if (direction != Vector3.zero)
         {
             float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
             float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
             transform.rotation = Quaternion.Euler(0, angle, 0);

             Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
             _characterController.Move(moveDir.normalized * _playerSpeed * Time.deltaTime);
         }
     }

     void Gravity()
     {
         bool grounded = IsGrounded();

         if (grounded && _playerGravity.y < 0)
         {
             _playerGravity.y = -2f; // mantiene al jugador en el suelo
             if (_animator.GetBool("IsJumping")) 
                 _animator.SetBool("IsJumping", false);
         }
         else
         {
             _playerGravity.y -= _gravityScale * Time.deltaTime;
         }

         _characterController.Move(_playerGravity * Time.deltaTime);
         _moveInput.Disable();
         _jumpInput.Disable();
     }*/




    bool IsGrounded()
    {
        if(Physics.Raycast(_sensor.position, -transform.up, _sensorRadius, _groundLayer))
        {
            Debug.DrawRay(_sensor.position, -transform.up * _sensorRadius, Color.green, 0.1f);
            _animator.SetBool("IsJumping", false);
            return true;
        }
        else 
        {
            Debug.DrawRay(_sensor.position, -transform.up * _sensorRadius, Color.red, 0.1f);
            return false;
        }
    }

    void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(_sensor.position, _sensorRadius);
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
