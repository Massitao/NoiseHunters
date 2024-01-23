using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Cinemachine;
using Kino;

public class ThirdPersonCamera : MonoBehaviour
{
    #region Camera Components
    [Header("Camera Components")]
    [SerializeField] private CinemachineFreeLook c_CinemachineFreeLook;
    [SerializeField] private CinemachineFreeLook c_CinemachineAimLook;
    [SerializeField] private CinemachineFreeLook c_CinemachineCrouchedFreeLook;
    [SerializeField] private CinemachineFreeLook c_CinemachineCrouchedAimLook;

    [Space(5)]

    [SerializeField] private AudioSource c_GlitchAudioSource;
    [SerializeField] private AMLink c_GlitchAMLink;

    private CinemachineBrain c_CinemachineBrain;
    [HideInInspector] public ThirdPersonController c_CharacterController;
    [HideInInspector] public Camera c_CharacterCamera;
    private CharacterMeshAnimator c_CharacterAnimator;
    private PlayerControls c_PlayerControls;

    private AnalogGlitch glitchEffect;
    #endregion

    [Space(10)]

    #region Input Values
    [Header("Input Values")]
    [SerializeField] public float cameraSensibility;
    private float currentCameraSensibility;


    [Space(5)]

    [SerializeField] private float cameraSpeedX;
    [SerializeField] private float cameraSpeedY;

    [SerializeField] private float joystickSpeedX;
    [SerializeField] private float joystickSpeedY;

    [HideInInspector] public Vector2 cameraInput;
    #endregion

    [Space(10)]

    #region Camera Values
    [Header("Camera Values")]
    [SerializeField] private LayerMask lm;
    Vector3 cameraForwardVector;
    Vector3 cameraRightVector;
    Vector3 currentRotation;

    Vector3 moveSmoothVelocity;
    Vector3 rotationSmoothVelocity;
    #endregion

    #region Camera Raycast
    [Header("Camera Raycast")]
    public RaycastHit cameraRayHit;
    #endregion

    [Space(10)]

    #region Glitch Values
    [Header("Glitch Values")]
    private Coroutine startAnalogGlitchCoroutine;
    #endregion




    #region Awake & Start
    private void Awake()
    {
        GetCameraComponents();
    }
    void GetCameraComponents()
    {
        c_CharacterCamera = GetComponent<Camera>();

        c_CinemachineBrain = GetComponent<CinemachineBrain>();

        glitchEffect = c_CharacterCamera.GetComponent<AnalogGlitch>();
    }



    void Start()
    {
        c_CharacterAnimator = c_CharacterController.GetComponent<CharacterMeshAnimator>();
        SetInputs();
        ChangeSpeed(c_CharacterController.c_PlayerInput.currentControlScheme.ToString());

        SetCameraFollow();
        AimTargetChange();
    }
    void SetInputs()
    {
        c_PlayerControls = c_CharacterController.playerControl;
        InputUser.onChange += ControlSchemeChange;

        float cameraSensibilityToUse = 0.5f;
        if (SaveInstance._Instance != null)
        {
            cameraSensibilityToUse = (c_CharacterController.isAiming) ? SaveInstance._Instance.currentLoadedConfig.userAimingSensibility : SaveInstance._Instance.currentLoadedConfig.userSensibility;
        }
        currentCameraSensibility = (SaveInstance._Instance != null) ? cameraSensibility * cameraSensibilityToUse : cameraSensibility;
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public void ChangeSpeed(string schemeName)
    {
        // assuming you have only 2 schemes: keyboard and gamepad
        if (schemeName.Equals("Keyboard & Mouse"))
        {
            c_CinemachineFreeLook.m_XAxis.m_MaxSpeed = cameraSpeedX;
            c_CinemachineCrouchedFreeLook.m_XAxis.m_MaxSpeed = cameraSpeedX;
            c_CinemachineAimLook.m_XAxis.m_MaxSpeed = cameraSpeedX;
            c_CinemachineCrouchedAimLook.m_XAxis.m_MaxSpeed = cameraSpeedX;

            c_CinemachineFreeLook.m_YAxis.m_MaxSpeed = cameraSpeedY;
            c_CinemachineCrouchedFreeLook.m_YAxis.m_MaxSpeed = cameraSpeedY;
            c_CinemachineAimLook.m_YAxis.m_MaxSpeed = cameraSpeedY;
            c_CinemachineCrouchedAimLook.m_YAxis.m_MaxSpeed = cameraSpeedY;
        }
        else
        {
            c_CinemachineFreeLook.m_XAxis.m_MaxSpeed = joystickSpeedX;
            c_CinemachineCrouchedFreeLook.m_XAxis.m_MaxSpeed = joystickSpeedX;
            c_CinemachineAimLook.m_XAxis.m_MaxSpeed = joystickSpeedX;
            c_CinemachineCrouchedAimLook.m_XAxis.m_MaxSpeed = joystickSpeedX;

            c_CinemachineFreeLook.m_YAxis.m_MaxSpeed = joystickSpeedY;
            c_CinemachineCrouchedFreeLook.m_YAxis.m_MaxSpeed = joystickSpeedY;
            c_CinemachineAimLook.m_YAxis.m_MaxSpeed = joystickSpeedY;
            c_CinemachineCrouchedAimLook.m_YAxis.m_MaxSpeed = joystickSpeedY;
        }
    }
    void SetCameraFollow()
    {
        c_CinemachineFreeLook.m_Follow = c_CharacterController.transform;
        c_CinemachineFreeLook.m_LookAt = c_CharacterController.transform;

        c_CinemachineAimLook.m_Follow = c_CharacterController.transform;
        c_CinemachineAimLook.m_LookAt = c_CharacterController.transform;

        c_CinemachineCrouchedFreeLook.m_Follow = c_CharacterController.transform;
        c_CinemachineCrouchedFreeLook.m_LookAt = c_CharacterController.transform;

        c_CinemachineCrouchedAimLook.m_Follow = c_CharacterController.transform;
        c_CinemachineCrouchedAimLook.m_LookAt = c_CharacterController.transform;

        c_CinemachineFreeLook.m_XAxis.Value = c_CharacterController.transform.eulerAngles.y;
        c_CinemachineFreeLook.m_YAxis.Value = 0.5f;
    }
    void SetAnalogGlitchToZero()
    {
        glitchEffect.scanLineJitter = 0f;
        glitchEffect.colorDrift = 0f;
    }
    #endregion

    #region Update & Late Update
    void Update()
    {
        CheckActiveCamera();
        SensibilityChange();
        AimTargetChange();
        CheckCameraRaycastCollisions();
    }

    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Camera X")
        {
            return cameraInput.x * currentCameraSensibility;
        }
        else if (axisName == "Camera Y")
        {
            return cameraInput.y * currentCameraSensibility;
        }
        return 0;
    }
    void CheckActiveCamera()
    {
        if (c_CinemachineBrain.IsLive(c_CinemachineFreeLook))
        {
            c_CinemachineAimLook.m_XAxis.Value = c_CinemachineFreeLook.m_XAxis.Value;
            c_CinemachineAimLook.m_YAxis.Value = c_CinemachineFreeLook.m_YAxis.Value;

            c_CinemachineCrouchedFreeLook.m_XAxis.Value = c_CinemachineFreeLook.m_XAxis.Value;
            c_CinemachineCrouchedFreeLook.m_YAxis.Value = c_CinemachineFreeLook.m_YAxis.Value;

            c_CinemachineCrouchedAimLook.m_XAxis.Value = c_CinemachineFreeLook.m_XAxis.Value;
            c_CinemachineCrouchedAimLook.m_YAxis.Value = c_CinemachineFreeLook.m_YAxis.Value;
        }
        else if (c_CinemachineBrain.IsLive(c_CinemachineAimLook))
        {
            c_CinemachineFreeLook.m_XAxis.Value = c_CinemachineAimLook.m_XAxis.Value;
            c_CinemachineFreeLook.m_YAxis.Value = c_CinemachineAimLook.m_YAxis.Value;

            c_CinemachineCrouchedFreeLook.m_XAxis.Value = c_CinemachineAimLook.m_XAxis.Value;
            c_CinemachineCrouchedFreeLook.m_YAxis.Value = c_CinemachineAimLook.m_YAxis.Value;

            c_CinemachineCrouchedAimLook.m_XAxis.Value = c_CinemachineAimLook.m_XAxis.Value;
            c_CinemachineCrouchedAimLook.m_YAxis.Value = c_CinemachineAimLook.m_YAxis.Value;
        }
        else if (c_CinemachineBrain.IsLive(c_CinemachineCrouchedFreeLook))
        {
            c_CinemachineFreeLook.m_XAxis.Value = c_CinemachineCrouchedFreeLook.m_XAxis.Value;
            c_CinemachineFreeLook.m_YAxis.Value = c_CinemachineCrouchedFreeLook.m_YAxis.Value;

            c_CinemachineAimLook.m_XAxis.Value = c_CinemachineCrouchedFreeLook.m_XAxis.Value;
            c_CinemachineAimLook.m_YAxis.Value = c_CinemachineCrouchedFreeLook.m_YAxis.Value;

            c_CinemachineCrouchedAimLook.m_XAxis.Value = c_CinemachineCrouchedFreeLook.m_XAxis.Value;
            c_CinemachineCrouchedAimLook.m_YAxis.Value = c_CinemachineCrouchedFreeLook.m_YAxis.Value;
        }
        else if (c_CinemachineBrain.IsLive(c_CinemachineCrouchedAimLook))
        {
            c_CinemachineFreeLook.m_XAxis.Value = c_CinemachineCrouchedAimLook.m_XAxis.Value;
            c_CinemachineFreeLook.m_YAxis.Value = c_CinemachineCrouchedAimLook.m_YAxis.Value;

            c_CinemachineAimLook.m_XAxis.Value = c_CinemachineCrouchedAimLook.m_XAxis.Value;
            c_CinemachineAimLook.m_YAxis.Value = c_CinemachineCrouchedAimLook.m_YAxis.Value;

            c_CinemachineCrouchedFreeLook.m_XAxis.Value = c_CinemachineCrouchedAimLook.m_XAxis.Value;
            c_CinemachineCrouchedFreeLook.m_YAxis.Value = c_CinemachineCrouchedAimLook.m_YAxis.Value;
        }

    }
    void AimTargetChange()
    {
        if (c_CharacterController.AimingCurrentState == ThirdPersonController.AimingState.Aiming)
        {
            c_CinemachineFreeLook.Priority = 0;
            c_CinemachineCrouchedFreeLook.Priority = 0;

            if (c_CharacterController.MovementCurrentState == ThirdPersonController.MovementState.Crouching)
            {
                c_CinemachineCrouchedAimLook.Priority = 2;
                c_CinemachineAimLook.Priority = 1;
            }
            else
            {
                c_CinemachineAimLook.Priority = 2;
                c_CinemachineCrouchedAimLook.Priority = 1;
            }
        }
        else
        {
            c_CinemachineAimLook.Priority = 0;
            c_CinemachineCrouchedAimLook.Priority = 0;

            if (c_CharacterController.MovementCurrentState == ThirdPersonController.MovementState.Crouching)
            {
                c_CinemachineCrouchedFreeLook.Priority = 2;
                c_CinemachineFreeLook.Priority = 1;
            }
            else
            {
                c_CinemachineFreeLook.Priority = 2;
                c_CinemachineCrouchedFreeLook.Priority = 1;
            }
        }
    }
    void CheckCameraRaycastCollisions()
    {
        Ray cameraRay = c_CharacterCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Physics.Raycast(cameraRay, out cameraRayHit, Mathf.Infinity, lm, QueryTriggerInteraction.Ignore);
    }



    void LateUpdate()
    {
        MouseInput();
        PassCameraAxisToController();
    }
    void MouseInput()
    {
        cameraInput = c_PlayerControls.PlayerMap.CameraMovement.ReadValue<Vector2>();
    }
    void PassCameraAxisToController()
    {
        cameraForwardVector = Quaternion.Euler(0, transform.localEulerAngles.y, 0) * Vector3.forward;
        cameraRightVector = Quaternion.Euler(0, transform.localEulerAngles.y, 0) * Vector3.right;

        c_CharacterController.cameraForward = cameraForwardVector;
        c_CharacterController.cameraRight = cameraRightVector;

        c_CharacterAnimator.cameraForward = cameraForwardVector;
    }
    void ControlSchemeChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            ChangeSpeed(user.controlScheme.Value.name);
        }
    }
    void SensibilityChange()
    {
        float cameraSensibilityToUse = 0.5f;
        if (SaveInstance._Instance != null)
        {
            cameraSensibilityToUse = (c_CharacterController.isAiming) ? SaveInstance._Instance.currentLoadedConfig.userAimingSensibility : SaveInstance._Instance.currentLoadedConfig.userSensibility;
        }
        currentCameraSensibility = (SaveInstance._Instance != null) ? cameraSensibility * cameraSensibilityToUse : cameraSensibility;
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    #endregion


    #region Camera Glitch Methods
    public void SetAnalogGlitchValues(float scanLineJitterToSet, float colorDriftToSet, float verticalJumpToSet, float timeToReach, bool enteringGlitch)
    {
        if (startAnalogGlitchCoroutine != null)
        {
            StopCoroutine(startAnalogGlitchCoroutine);
        }
        startAnalogGlitchCoroutine = StartCoroutine(SettingAnalogGlitch(scanLineJitterToSet, colorDriftToSet, verticalJumpToSet, timeToReach, enteringGlitch));
    }

    private IEnumerator SettingAnalogGlitch(float scanLineJitterToSet, float colorDriftToSet, float verticalJumpToSet, float timeToReach, bool enteringGlitch)
    {
        float volumeToSet = (enteringGlitch) ? 1f : 0f;

        if (timeToReach > 0f)
        {
            float initialScanLineJitter = glitchEffect.scanLineJitter;
            float initialColorDrift = glitchEffect.colorDrift;
            float initialVerticalJump = glitchEffect.verticalJump;

            float initialVolume = c_GlitchAMLink.initialVolume;

            float lerp = 0f;
            float activationTime = Time.time;

            while (lerp < 1f)
            {
                lerp = Mathf.InverseLerp(activationTime, activationTime + timeToReach, Time.time);
                glitchEffect.scanLineJitter = Mathf.Lerp(initialScanLineJitter, scanLineJitterToSet, lerp);
                glitchEffect.colorDrift = Mathf.Lerp(initialColorDrift, colorDriftToSet, lerp);
                glitchEffect.verticalJump = Mathf.Lerp(initialVerticalJump, verticalJumpToSet, lerp);

                c_GlitchAMLink.initialVolume = Mathf.Lerp(initialVolume, volumeToSet, lerp);
                c_GlitchAudioSource.volume = (AudioManager._Instance != null) ? c_GlitchAMLink.initialVolume * AudioManager._Instance.soundVolume : c_GlitchAMLink.initialVolume;

                yield return null;
            }
        }

        glitchEffect.scanLineJitter = scanLineJitterToSet;
        glitchEffect.colorDrift = colorDriftToSet;
        glitchEffect.verticalJump = verticalJumpToSet;

        c_GlitchAMLink.initialVolume = volumeToSet;
        c_GlitchAudioSource.volume = (AudioManager._Instance != null) ? c_GlitchAMLink.initialVolume * AudioManager._Instance.soundVolume : c_GlitchAMLink.initialVolume;

        startAnalogGlitchCoroutine = null;

        yield break;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Ray cameraRay = c_CharacterCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Gizmos.DrawRay(cameraRay);
    }
}