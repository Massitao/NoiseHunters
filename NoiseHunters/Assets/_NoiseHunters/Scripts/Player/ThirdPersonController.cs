using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    #region Controller Components
    [Header("Controller Components")]
    [HideInInspector] public CharacterController c_CharacterController;
    [HideInInspector] public ThirdPersonCamera c_CharacterCamera;
    [HideInInspector] public CharacterMeshAnimator c_CharacterAnimator;
    [HideInInspector] public CharacterCombat c_CharacterCombat;
    [HideInInspector] public InteractUser c_InteractUser;
    [HideInInspector] public PlayerHealth c_PlayerHealth;
    [HideInInspector] public PauseUser c_PauseUser;
    [HideInInspector] public GameObject c_CharacterMesh;
    [HideInInspector] public CapsuleCollider c_CharacterCapsuleCollider;
    [HideInInspector] public PlayerInput c_PlayerInput;
    [HideInInspector] public PlayerControls playerControl;
    #endregion

    #region Player Input
    [Header("Player Input")]
    [HideInInspector] public Vector3 _moveInput;
    [HideInInspector] public Vector3 _controllerMovementInput;

    [HideInInspector] public Vector3 _moveInputNormalized;
    [HideInInspector] public Vector3 _controllerMovementInputNormalized;

    private bool _aimHold;
    private bool _shiftHold;
    #endregion

    #region Camera Values
    [Header("Camera Values")]
    [HideInInspector] public Vector3 cameraForward;
    [HideInInspector] public Vector3 cameraRight;
    #endregion

    #region Controller Constrains
    [Header("Constrains")]
    public bool constrain_Move;
    public bool constrain_Rotate;
    public bool constrain_Crouch;
    public bool constrain_Run;
    public bool constrain_Aim;
    #endregion

    #region Character Mesh Rotation Values
    [Header("Character Mesh Values")]
    [SerializeField] private float rotationFreeLookSpeed;
    [SerializeField] private float rotationAimLookSpeed;
    #endregion

    #region Character States
    [Header("Character States")]
    public MovementState MovementCurrentState;
    public CrouchingState CrouchingCurrentState;
    public AimingState AimingCurrentState;

    public enum MovementState { Idle, Walking, Crouching, Running };
    public enum CrouchingState { None, Idle, Normal };
    public enum AimingState { NotAiming, Aiming };

    private bool _isGrounded;
    public bool isGrounded
    {
        get { return _isGrounded; }
        set
        {
            if (value != _isGrounded)
            {
                _isGrounded = value;
                GroundStateChange();
            }
        }
    }
    [HideInInspector] public bool isIdle = true;
    [HideInInspector] public bool isAiming;
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool isRunning;
    #endregion

    #region Global Controller Speed
    [Header("Global Controller Speed")]
    [HideInInspector] public Vector3 controllerVelocity;

    private Vector3 _lastRecordedGroundDirection;

    private float m_currentControllerXZSpeed;
    private float m_currentControllerYSpeed;
    #endregion

    #region Character Speed
    [Header("Character Ground Speed")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float crouchingNormalSpeed;
    [SerializeField] private float runningSpeed;

    private float currentAcceleration;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    [Range(0f, 1f)]
    [SerializeField] private float aimSpeedReductionPercentage;
    #endregion

    #region Crouch Values
    [Header("Crouch Values")]
    [SerializeField] private float crouchControllerHeight;
    [SerializeField] private float crouchControllerCenter;

    [SerializeField] private float crouchRoofDetectionRayLength;

    private float baseControllerHeight;
    private Vector3 baseControllerCenter;
    #endregion

    #region Gravity Values
    [Header("Gravity Values")]
    [SerializeField] private float _maxFallingSpeed;
    [SerializeField] private float _gravityMultiplier;

    private float _gravity => Physics.gravity.y;
    #endregion


    #region Awake & Start
    private void Awake()
    {
        c_CharacterCamera = FindObjectOfType<ThirdPersonCamera>();
        c_CharacterCamera.c_CharacterController = this;

        c_CharacterController = GetComponent<CharacterController>();
        c_CharacterAnimator = GetComponent<CharacterMeshAnimator>();
        c_CharacterCombat = GetComponent<CharacterCombat>();
        c_PlayerHealth = GetComponent<PlayerHealth>();
        c_PauseUser = GetComponent<PauseUser>();

        c_CharacterMesh = (c_CharacterAnimator) ? c_CharacterAnimator.gameObject : GetComponent<CapsuleCollider>().gameObject;

        c_CharacterCapsuleCollider = GetComponent<CapsuleCollider>();

        c_PlayerInput = GetComponent<PlayerInput>();
        playerControl = new PlayerControls();

        c_InteractUser = GetComponent<InteractUser>();


        playerControl.PlayerMap.Aim.started += aiming => _aimHold = true;
        playerControl.PlayerMap.StopAim.performed += aiming => _aimHold = false;
        playerControl.PlayerMap.Crouch.started += crouching => Crouch();
        playerControl.PlayerMap.Sprint.started += sprinting => _shiftHold = true;
        playerControl.PlayerMap.StopSprint.performed += sprinting => _shiftHold = false;

        playerControl.PlayerMap.ThrowAndMagnet.started += attack => c_CharacterCombat.Combat_InputActions();
        playerControl.PlayerMap.Interact.started += attack => c_InteractUser.CanUse();

        playerControl.PlayerMap.Pause.started += pause => c_PauseUser.DoPause();
        playerControl.PlayerMap.Select.started += select => c_CharacterCombat.currentMissionManagerUI?.ShowHideMissionPanel();
    }

    public void OnEnable()
    {
        c_PlayerInput.enabled = true;
        playerControl.Enable();
    }
    public void OnDisable()
    {
        c_PlayerInput.enabled = false;
        _aimHold = false;
        _shiftHold = false;
        playerControl.Disable();
    }

    void Start() => StartValuesSet();
    void StartValuesSet()
    {
        baseControllerHeight = c_CharacterController.height;
        baseControllerCenter = c_CharacterController.center;

        Physics.IgnoreCollision(c_CharacterController, c_CharacterCapsuleCollider, true);
    }
    #endregion

    void Update()
    {
        InputUpdate();
        HandleMovement();
    }


    void InputUpdate()
    {
        // Movement Input
        if (!constrain_Move)
        {
            _moveInput = new Vector3(playerControl.PlayerMap.Movement.ReadValue<Vector2>().x, 0f, playerControl.PlayerMap.Movement.ReadValue<Vector2>().y);
            _moveInputNormalized = _moveInput.normalized;

            _controllerMovementInput = (cameraForward * _moveInputNormalized.z) + (cameraRight * _moveInputNormalized.x);
            _controllerMovementInputNormalized = _controllerMovementInput.normalized;
        }
    }


    void HandleMovement()
    {
        MoveController();

        SpeedSelection();

        Gravity();
        GroundDetection();

        MovementStateSelection();
        AimState();

        RotateCharacterMesh();
    }


    void MoveController()
    {
        Vector3 XZMovement = _controllerMovementInputNormalized;

        if (_moveInputNormalized != Vector3.zero && isGrounded)
        {
            _lastRecordedGroundDirection = XZMovement;
        }

        Vector3 YMovement = m_currentControllerYSpeed * Vector3.up;

        c_CharacterController.Move((_lastRecordedGroundDirection * m_currentControllerXZSpeed + YMovement) * Time.deltaTime);
    }
    void GroundDetection()
    {
        bool grounded = c_CharacterController.isGrounded & CollisionFlags.Below != 0;

        if (!grounded)
        {
            Physics.Raycast(new Vector3(c_CharacterController.bounds.center.x, c_CharacterController.bounds.min.y, c_CharacterController.bounds.center.z), -Vector3.up, out RaycastHit hit, 0.5f);
            Debug.DrawRay(new Vector3(c_CharacterController.bounds.center.x, c_CharacterController.bounds.min.y, c_CharacterController.bounds.center.z), -Vector3.up * 0.5f);

            grounded = (hit.transform != null);
        }

        isGrounded = grounded;
    }


    public void ResetVelocity()
    {
        m_currentControllerXZSpeed = 0f;
    }
    void SpeedSelection()
    {
        controllerVelocity = c_CharacterController.velocity;

        float aimSpeedReduction = (AimingCurrentState == AimingState.Aiming) ? aimSpeedReductionPercentage : 1f;

        float accOrDecc = (_moveInput != Vector3.zero) ? acceleration : decceleration;
        accOrDecc *= Time.deltaTime;

        float speedToChoose = 0;
        if (_moveInput != Vector3.zero)
        {
            speedToChoose =
                (isCrouching)
                ? crouchingNormalSpeed
                : (isRunning) ? runningSpeed : walkingSpeed;
        }

        speedToChoose = speedToChoose * aimSpeedReduction;

        m_currentControllerXZSpeed = Mathf.MoveTowards(m_currentControllerXZSpeed, speedToChoose, accOrDecc);
    }
    void Gravity()
    {
        // If Controller is in Ground AND not Jumping, apply constant gravity
        if (isGrounded)
        {
            m_currentControllerYSpeed = _gravity * _gravityMultiplier;
        }

        // Else, keep adding acceleration
        else
        {
            m_currentControllerYSpeed += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        // Clamping Controller Y Speed Values to prevent high falling speeds that might cause clipping
        m_currentControllerYSpeed = Mathf.Clamp(m_currentControllerYSpeed, -_maxFallingSpeed, float.MaxValue);
    }


    void MovementStateSelection()
    {        
        if ((_shiftHold && !isCrouching || _shiftHold && isCrouching && !CheckForRoof()) && AimingCurrentState != AimingState.Aiming && _moveInput.z > 0)
        {
            isRunning = true;
            isCrouching = false;
        }
        else
        {
            isRunning = false;
        }

        MovementCurrentState = (isCrouching) ? MovementState.Crouching : MovementState.Idle;

        c_CharacterController.height = (isCrouching) ? crouchControllerHeight : baseControllerHeight;
        c_CharacterController.center = (isCrouching) ? new Vector3(c_CharacterController.center.x, crouchControllerCenter, c_CharacterController.center.z) : baseControllerCenter;

        c_CharacterCapsuleCollider.height = (isCrouching) ? crouchControllerHeight : baseControllerHeight;
        c_CharacterCapsuleCollider.center = (isCrouching) ? new Vector3(c_CharacterController.center.x, crouchControllerCenter, c_CharacterController.center.z) : baseControllerCenter;

        if (m_currentControllerXZSpeed > 0f)
        {
            if (!isCrouching)
            {
                CrouchingCurrentState = CrouchingState.None;
                MovementCurrentState = (m_currentControllerXZSpeed <= walkingSpeed) ? MovementState.Walking : MovementState.Running; 
            }
            else
            {
                CrouchingCurrentState = CrouchingState.Normal;
            }
        }
        else
        {
            if (!isCrouching)
            {
                MovementCurrentState = MovementState.Idle;
                CrouchingCurrentState = CrouchingState.None;
            }
            else CrouchingCurrentState = CrouchingState.Idle;
        }
        
    }
    void Crouch()
    {
        if (!constrain_Crouch)
        {
            if (!_shiftHold)
            {
                if (!CheckForRoof())
                {
                    isCrouching = !isCrouching;
                }
            }
        }
    }
    bool CheckForRoof()
    {
        if (isCrouching)
        {
            Physics.Raycast(new Vector3(c_CharacterController.bounds.center.x, c_CharacterController.bounds.max.y, c_CharacterController.bounds.center.z), Vector3.up, out RaycastHit hit, crouchRoofDetectionRayLength);
            Debug.DrawRay(new Vector3(c_CharacterController.bounds.center.x, c_CharacterController.bounds.max.y, c_CharacterController.bounds.center.z), Vector3.up * crouchRoofDetectionRayLength, Color.red, 10f);
            if (hit.transform != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void AimState()
    {
        // Aim State
        if (_aimHold && !constrain_Aim)
        {
            AimingCurrentState = AimingState.Aiming;
            isAiming = true;
        }

        if (!_aimHold || constrain_Aim || !isGrounded)
        {
            AimingCurrentState = AimingState.NotAiming;
            isAiming = false;
        }
    }


    void GroundStateChange()
    {
        if (c_PlayerHealth.IsAlive)
        {
            if (isGrounded)
            {
                c_CharacterAnimator.Grounded();
            }
            else
            {
                m_currentControllerYSpeed = -2f;
                c_CharacterAnimator.Falling();
            }
        }
    }
    public void AddFallingConstrains()
    {
        constrain_Move = true;
        constrain_Crouch = true;
        constrain_Run = true;
        constrain_Rotate = true;
    }
    public void RemoveFallingConstraint()
    {
        constrain_Move = false;
        constrain_Crouch = false;
        constrain_Run = false;
        constrain_Rotate = false;

        isCrouching = false;
    }


    void RotateCharacterMesh()
    {
        if (c_CharacterMesh != null)
        {
            if ((_controllerMovementInputNormalized != Vector3.zero && !constrain_Move && isGrounded) || (AimingCurrentState == AimingState.Aiming) || (c_CharacterCombat.IsThrowing))
            {
                c_CharacterMesh.transform.rotation = Quaternion.Slerp(c_CharacterMesh.transform.rotation, Quaternion.LookRotation(cameraForward, Vector3.up), rotationAimLookSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.gameObject.TryGetComponent(out AIBrain touchedAI);

        if (touchedAI != null)
        {
            if (touchedAI is RangedEnemyBrain)
            {
                RangedEnemyBrain rangedEnemyBrain = touchedAI as RangedEnemyBrain;
                rangedEnemyBrain.RangedEnemy_Collider_Touched();
            }
        }
    }
}