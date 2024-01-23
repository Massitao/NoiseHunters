using System.Collections;
using UnityEngine;

public class CharacterMeshAnimator : MonoBehaviour
{
    [Header("Animator Components")]
    private Animator c_CharacterAnimator;
    private ThirdPersonController c_CharacterController;
    private ThirdPersonCamera c_CharacterCamera;
    private CharacterCombat c_CharacterCombat;

    private FootStep c_leftFoot;
    private FootStep c_rightFoot;

    [Header("Character Controller Values")]
    [SerializeField] private string _movSpeed;
    [SerializeField] private string _rotationDirection;


    [Header("Character States Values")]
    [SerializeField] private string _groundedState;
    [SerializeField] private string _fallingState;
    [SerializeField] private string _aimingState;
    [SerializeField] private string _crouchingState;
    [SerializeField] private string _runningState;

    [SerializeField] private string _throwTrigger;
    [SerializeField] private string _assasinTrigger;


    [Header("Input Values")]
    [SerializeField] private string _inputDirectionX;
    [SerializeField] private string _inputDirectionY;
    [SerializeField] private float _inputSpeedDamp;


    [Header("Walking Values")]
    [SerializeField] private float _walkSpeedMultiplier;
    [SerializeField] private float _walkSpeedDamp;


    [Header("Crouching / Tiptoeing / Running Values")]
    [SerializeField] private string _crouchingSpeedValue;
    [SerializeField] private float _normalCrouchSpeedMultiplier;
    [SerializeField] private float _runningCrouchSpeedMultiplier;
    [SerializeField] private float _crouchSpeedDamp;


    [Header("Rotate Mesh Values")]
    [SerializeField] private string _mouseLookDirectionX;
    [SerializeField] private string _mouseLookDirectionY;

    [SerializeField] private float _rotationSpeedDamp;
    [SerializeField] private float _angleThreshold;


    [Header("Assasination Values")]
    [SerializeField] private string _assasinationAnimationName;
    Coroutine onAssasination;


    [Header("Magnet Values")]
    [SerializeField] private string _magnetStartTrigger;
    [SerializeField] private string _magnetEndTrigger;

    [Header("Interact Values")]
    [SerializeField] private string _pickableInteractTrigger;

    [Header("Camera Values")]
    [SerializeField] private float _cameraLookSpeedDamp;
    [HideInInspector] public Vector3 cameraForward;


    private void Awake()
    {
        c_CharacterController = GetComponent<ThirdPersonController>();
        c_CharacterAnimator = GetComponent<Animator>();

        c_leftFoot = GetComponentsInChildren<FootStep>()[0];
        c_rightFoot = GetComponentsInChildren<FootStep>()[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        c_CharacterCombat = c_CharacterController.c_CharacterCombat;
        c_CharacterCamera = c_CharacterController.c_CharacterCamera;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationValues();        
    }

    void UpdateAnimationValues()
    {
        ControllerInput();
        ControllerSpeed();
        RotateMeshToInput();
        RotateMeshToCameraView();

        Aiming();
        Crouching();
        Running();      
    }

    void ControllerInput()
    {
        float xDirection = c_CharacterController._moveInput.normalized.x;
        float yDirection = c_CharacterController._moveInput.normalized.z;

        c_CharacterAnimator.SetFloat(_inputDirectionX, xDirection, _inputSpeedDamp, Time.deltaTime);
        c_CharacterAnimator.SetFloat(_inputDirectionY, yDirection, _inputSpeedDamp, Time.deltaTime);
    }
    void ControllerSpeed()
    {
        c_CharacterAnimator.SetFloat(_movSpeed, new Vector3(c_CharacterController.controllerVelocity.x, 0f, c_CharacterController.controllerVelocity.z).magnitude);
    }

    void RotateMeshToInput()
    {
        float meshRotToInputRot = Mathf.Round(Vector3.SignedAngle(c_CharacterAnimator.transform.forward, c_CharacterController._controllerMovementInput, Vector3.up));
        meshRotToInputRot = (meshRotToInputRot != 0f && Mathf.Abs(meshRotToInputRot) > _angleThreshold) ? Mathf.Sign(meshRotToInputRot) : 0f;

        c_CharacterAnimator.SetFloat(_rotationDirection, meshRotToInputRot, _rotationSpeedDamp, Time.deltaTime);
    }
    void RotateMeshToCameraView()
    {
        float meshRotToInputRotX = Mathf.Round(Vector3.SignedAngle(c_CharacterAnimator.transform.forward, cameraForward, Vector3.up));
        float meshRotToInputRotY = Mathf.Round(c_CharacterCamera.transform.eulerAngles.x);
        meshRotToInputRotY = (meshRotToInputRotY > 180f) ? meshRotToInputRotY - 360 : meshRotToInputRotY;

        c_CharacterAnimator.SetFloat(_mouseLookDirectionX, meshRotToInputRotX, _cameraLookSpeedDamp, Time.deltaTime);
        c_CharacterAnimator.SetFloat(_mouseLookDirectionY, meshRotToInputRotY, _cameraLookSpeedDamp, Time.deltaTime);
    }


    public void Grounded()
    {
        c_CharacterAnimator.SetBool(_groundedState, c_CharacterController.isGrounded);
    }
    public void Falling()
    {
        Grounded();
        c_CharacterAnimator.SetTrigger(_fallingState);
    }
    void Crouching()
    {
        float crouchingMultiplier;

        if (c_CharacterController.isRunning)
        {
            crouchingMultiplier = _runningCrouchSpeedMultiplier;
        }
        else
        {
            crouchingMultiplier = _normalCrouchSpeedMultiplier;
        }

        c_CharacterAnimator.SetBool(_crouchingState, c_CharacterController.isCrouching);
        c_CharacterAnimator.SetFloat(_crouchingSpeedValue, crouchingMultiplier, _crouchSpeedDamp, Time.deltaTime);
    }
    void Running()
    {
        c_CharacterAnimator.SetBool(_runningState, c_CharacterController.MovementCurrentState == ThirdPersonController.MovementState.Running);
    }
    void Aiming()
    {
        c_CharacterAnimator.SetBool(_aimingState, c_CharacterController.isAiming);
    }


    void LeftStep(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            c_leftFoot.OnStep();
        }
    }
    void RightStep(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            c_rightFoot.OnStep();
        }
    }

    void LeftStepLanding()
    {
        c_leftFoot.OnLand();
    }
    void RightStepLanding()
    {
        c_rightFoot.OnLand();
    }


    public void Throw()
    {
        c_CharacterAnimator.SetTrigger(_throwTrigger);
    }

    public void Assasinate()
    {
        if (onAssasination != null)
        {
            StopCoroutine(onAssasination);
        }

        onAssasination = StartCoroutine(Assasinating());
    }
    IEnumerator Assasinating()
    {
        c_CharacterAnimator.SetTrigger(_assasinTrigger);
        c_CharacterAnimator.applyRootMotion = false;

        yield return new WaitForSeconds(0.3f);

        while (c_CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName(_assasinationAnimationName))
        {
            yield return null;
        }

        c_CharacterCombat.Combat_EndOfAssassination();

        c_CharacterAnimator.applyRootMotion = true;
        onAssasination = null;

        yield break;
    }

    public void MagnetStart()
    {
        c_CharacterAnimator.SetTrigger(_magnetStartTrigger);
    }
    public void MagnetEnd()
    {
        c_CharacterAnimator.SetTrigger(_magnetEndTrigger);
    }


    public void PickableInteract()
    {
        c_CharacterController.ResetVelocity();
        c_CharacterController.AddFallingConstrains();
        c_CharacterAnimator.SetTrigger(_pickableInteractTrigger);
    }
    public void InteractEnd()
    {
        c_CharacterController.RemoveFallingConstraint();
    }

    public void Death()
    {
        c_CharacterAnimator.SetTrigger("Death");
        c_CharacterAnimator.SetLayerWeight(1, 0f);
    }
}
