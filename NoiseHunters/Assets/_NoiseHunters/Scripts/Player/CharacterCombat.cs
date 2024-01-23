using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    #region Character Components
    [Header("Character Components")]
    [SerializeField] private Transform CharacterRightHand;
    [SerializeField] private Transform CharacterHead;

    [SerializeField] private Renderer CharacterBackArmor;
    [SerializeField] private Renderer CharacterBodyArmor;
    [SerializeField] private Renderer CharacterBracelet;

    private ThirdPersonController c_CharacterController;
    private ThirdPersonCamera c_CharacterCamera;
    private CharacterMeshAnimator c_CharacterMeshAnimator;
    private PlayerHealth c_PlayerHealth;
    private PlayerControls c_PlayerControl;

    private SoundSpeaker c_CombatSpeaker;   
    #endregion

    #region Weapons GameObjects
    [Header("Weapons GameObjects")]
    [SerializeField] private GameObject rightWeaponSword;

    [SerializeField] private GameObject weaponProjectile;
    #endregion

    #region Constrains
    [Header("Constrains")]
    [HideInInspector] public bool constrain_Attack;
    #endregion


    #region Combat State
    [Header("Combat State")]
    private bool hasRightWeaponSword;
    private bool HasRightWeaponSword
    {
        get { return hasRightWeaponSword; }
        set
        {
            if (hasRightWeaponSword != value)
            {
                hasRightWeaponSword = value;

                Combat_CheckSwordAvailability(false);
            }
        }
    }

    [HideInInspector] public CombatState currentCombatState;
    public enum CombatState { None, CombatModeActive };

    private bool _IsThrowing;
    public bool IsThrowing
    {
        get { return _IsThrowing; }
        set
        {
            if (_IsThrowing != value)
            {
                _IsThrowing = value;
                OnStateChange?.Invoke();
            }
        }
    }

    private bool _IsAssassinating;
    public bool IsAssassinating
    {
        get { return _IsAssassinating; }
        set
        {
            if (_IsAssassinating != value)
            {
                _IsAssassinating = value;
                OnStateChange?.Invoke();
            }
        }
    }

    [SerializeField] private SoundInfo playerDeathSound;

    public Action OnStateChange;
    #endregion

    #region Checkers - Area and AutoAim Check
    [Header("Area and AutoAim Checkers")]
    // Area Check
    [SerializeField] private float enemyAreaCheckUpdateTime;
    [SerializeField] private float enemyAreaCheckListenersRadius;
    [SerializeField] private LayerMask enemyAreaCheckLayermask;
    [SerializeField] private LayerMask enemyLineCheckLayermask;

    public Action OnCheckerStart;
    public Action OnCheckerEnd;

    private Coroutine enemyAreaCheckCoroutine;

    [Space(10)]

    // AutoAim Check
    [Range(0f, 30f)] [SerializeField] private float throwAutoAimAngle;
    [SerializeField] private float throwAutoAimMaxDistance;


    private List<GameObject> throwPossibleTargets = new List<GameObject>();
    private GameObject _throwAutoAimTarget;
    private GameObject ThrowAutoAimTarget
    {
        get { return _throwAutoAimTarget; }
        set
        {
            if (_throwAutoAimTarget != value)
            {
                if (_throwAutoAimTarget != null)
                {
                    Combat_AutoAimUnmarkEntity();
                }

                _throwAutoAimTarget = value;

                if (_throwAutoAimTarget != null)
                {
                    Combat_AutoAimMarkEntity();
                }
            }
        }
    }

    [Space(10)]

    // Enemy Nearby Check
    [SerializeField] private float armorDetectionGlowLerpTime;
    [SerializeField] private float armorDetectionStopGlowLerpTime;
    [SerializeField] private AnimationCurve armorDetectionGlowAnimationCurve;

    private Material armorDetectionInitialMat;
    private Color armorDetectionInitialColor;
    private float armorDetectionInitialIntensity;
    private float armorDetectionCurrentIntensity;

    private Coroutine ArmorGlowCoroutine;
    private Coroutine ArmorStaticCoroutine;
    #endregion

    #region Assassinate Values
    [Header("Assassinate Values")]
    [SerializeField] private float assassinationMinimumNearDistance;
    [SerializeField] private float assassinationLookMinimumNearAngle;
    [SerializeField] private float assassinationFeetHeightOffset;

    [Space(10)]

    [SerializeField] private float assassinationMoveTime;
    [SerializeField] private float assassinationRotationTime;
    [SerializeField] [Range(0f, 1f)] private float assassinationMoveToBackPercentage;

    [Space(10)]

    [SerializeField] private SoundInfo assassinationExecutedSound;


    // Private Vars
    private bool _canAssassinate;
    public bool CanAssassinate
    {
        get { return _canAssassinate; }
        set
        {
            if (_canAssassinate != value)
            {
                _canAssassinate = value;

                if (_canAssassinate)
                {
                    OnAssassinationAvailable?.Invoke();
                }
                else
                {
                    OnAssassinationNotAvailable?.Invoke();
                }
            }
        }
    }


    private Transform assassinationFocusedTarget;
    private AIAssassinable assassinatingTarget;


    private Coroutine assassinationLoopCoroutine;


    public Action OnAssassinationAvailable;
    public Action OnAssassinationNotAvailable;
    #endregion

    #region Throw Values
    [Header("Throw Values")]
    [SerializeField] private float throwAutoAimPower;
    [SerializeField] private float throwForcedAimPower;
    [SerializeField] private float throwRaycastDistanceThreshold;

    [Space(10)]

    [SerializeField] private float throwWeaponReturnSpeed;
    [SerializeField] private float throwWeaponReturnRotationSpeed;
    [SerializeField] private float throwWeaponReturnCooldownTime;
    [SerializeField] private float throwWeaponReturnColorLerpTime;

    [Space(10)]

    [SerializeField] private SoundInfo throwWeaponSound;
    [SerializeField] private SoundInfo throwWeaponReturnSound;


    private bool throwCanCallReturnWeapon;
    private bool throwWeaponReturning;


    private GameObject thrownSword;


    private Coroutine throwReturnSwordCoroutine;
    private Coroutine throwReturnSwordCooldownCoroutine;
    private Coroutine throwSwordStateChangeCoroutine;


    public Action<GameObject> OnSwordThrown;
    public Action<float> OnThrowCooldownChange;
    public Action OnSwordRetreived;
    #endregion

    #region Particle Components
    [Header("Particle Components")]
    [SerializeField] private ParticleSystem magnetParticle;
    #endregion

    #region UI
    [Header("Player HUD")]
    [SerializeField] private GameObject compassHUDPrefab;
    private CompassHUD compassHUD;


    [Header("Mission HUD")]
    [SerializeField] private GameObject missionManagerUIPrefab;
    [HideInInspector] public MissionManagerUI currentMissionManagerUI;


    [Header("Assassinate UI")]
    [SerializeField] private GameObject assassinateUIPrefab;
    private FollowingIconHUD assassinateCurrentUI;
    #endregion



    #region Awake, Enable/Disable & Start Methods
    // First call before Start method
    private void Awake()
    {
        Combat_GetCombatComponents();
    }
    // Get and Set Components
    void Combat_GetCombatComponents()
    {
        c_CharacterController = GetComponent<ThirdPersonController>();
        c_PlayerHealth = GetComponent<PlayerHealth>();
        c_CombatSpeaker = gameObject.AddComponent<SoundSpeaker>();

        compassHUD = Instantiate(compassHUDPrefab).GetComponent<CompassHUD>();
        compassHUD.SetPlayer(c_CharacterController);

        assassinateCurrentUI = Instantiate(assassinateUIPrefab, Vector3.zero, Quaternion.identity, null).GetComponentInChildren<FollowingIconHUD>();
        assassinateCurrentUI.gameObject.SetActive(false);

        currentMissionManagerUI = Instantiate(missionManagerUIPrefab).GetComponent<MissionManagerUI>();
        currentMissionManagerUI.compassHUD = compassHUD;
    }



    // Suscribe to Events
    private void OnEnable()
    {
        // State Check
        OnStateChange += Combat_CheckState;

        // Enemy Check
        OnCheckerStart += Combat_StartCheckersCoroutine;
        OnCheckerEnd += Combat_EndCheckersCoroutine;

        // UI
        OnAssassinationAvailable += Combat_UI_AssassinationAvailable;
        OnAssassinationNotAvailable += Combat_UI_AssassinationNotAvailable;

        // Living Entity
        c_PlayerHealth.OnEntityDeath += Combat_PlayerDeath;
    }

    // Unsuscribe to Events
    private void OnDisable()
    {
        // State Check
        OnStateChange -= Combat_CheckState;

        // Enemy Check
        OnCheckerStart -= Combat_StartCheckersCoroutine;

        if (enemyAreaCheckCoroutine != null)
        {
            StopCoroutine(enemyAreaCheckCoroutine);
            enemyAreaCheckCoroutine = null;
        }
        OnCheckerEnd -= Combat_EndCheckersCoroutine;

        // UI
        OnAssassinationAvailable -= Combat_UI_AssassinationAvailable;
        OnAssassinationNotAvailable -= Combat_UI_AssassinationNotAvailable;

        // Living Entity
        c_PlayerHealth.OnEntityDeath -= Combat_PlayerDeath;
    }



    // Start is called before the first frame update
    void Start()
    {
        Combat_GetExtraCombatComponentsFromController();
        Combat_SetCombatValues();
    }

    // Get and Set Components
    void Combat_GetExtraCombatComponentsFromController()
    {
        // Safety Set: Accessing components from another components in Awake will probably return null.
        c_CharacterMeshAnimator = c_CharacterController.c_CharacterAnimator;
        c_CharacterCamera = c_CharacterController.c_CharacterCamera;
        c_PlayerControl = c_CharacterController.playerControl;

        if (CharacterBackArmor != null)
        {
            armorDetectionInitialColor = CharacterBackArmor.material.GetColor("_EmissionColor");
            armorDetectionInitialIntensity = 1f;
            armorDetectionCurrentIntensity = armorDetectionInitialIntensity;

            armorDetectionInitialMat = CharacterBackArmor.material;
            CharacterBackArmor.material = new Material(armorDetectionInitialMat);
        }
    }
    void Combat_SetCombatValues()
    {
        // Check if player has rightWeaponSword
        hasRightWeaponSword = (rightWeaponSword != null);
        Combat_CheckSwordAvailability(true);

        // If player has weapon, allow player to Call Return
        throwCanCallReturnWeapon = HasRightWeaponSword;

        // Start Enemy Checker
        OnCheckerStart?.Invoke();
    }
    #endregion


    // All Actions that require Input
    public void Combat_InputActions()
    {
        if (currentCombatState != CombatState.CombatModeActive && !constrain_Attack && c_PlayerHealth.IsAlive)
        {
            if (CanAssassinate)
            {
                Combat_Assassinate();
            }
            else
            {
                Combat_ThrowProjectile();
            }

            Combat_ReturnSwordRanged();
        }
    }


    // Detect if player is in Combat Mode
    void Combat_CheckState()
    {
        currentCombatState = (IsAssassinating || IsThrowing) ? CombatState.CombatModeActive : CombatState.None;
    }

    void Combat_PlayerDeath(LivingEntity entity)
    {
        c_CombatSpeaker.CreateSoundBubble(playerDeathSound, transform.position, gameObject, true);
    }

    #region Checkers - Assassination Area and AutoAim Checkers
    // Enemy Checker Starter
    void Combat_StartCheckersCoroutine()
    {
        // Check if Enemy Checker Coroutine is active. If so, stop the coroutine
        if (enemyAreaCheckCoroutine != null)
        {
            StopCoroutine(enemyAreaCheckCoroutine);
        }

        // Set and start Enemy Checker Coroutine
        enemyAreaCheckCoroutine = StartCoroutine(Combat_CheckersCoroutine());
    }

    // Checker Loop
    IEnumerator Combat_CheckersCoroutine()
    {
        // Keep loop active forever
        while (true)
        {
            // All checkers
            Combat_AssassinationAndListenerCheckers();
            Combat_AutoAimTargetChecker();

            // Loop Fixed Time
            yield return new WaitForSeconds(enemyAreaCheckUpdateTime);
        }
    }

    // Enemy Checker Finisher
    void Combat_EndCheckersCoroutine()
    {
        // Safety Set: If player dies, Assassination UI will be disabled
        CanAssassinate = false;

        // Check if Enemy Checker Coroutine is active. If so, stop the coroutine and reset "enemyAreaCheckCoroutine" to null
        if (enemyAreaCheckCoroutine != null)
        {
            StopCoroutine(enemyAreaCheckCoroutine);
            enemyAreaCheckCoroutine = null;
        }
    }

    #region Assassination Checker Methods
    // Area Checker - Assassinable Entities and Listeners
    void Combat_AssassinationAndListenerCheckers()
    {
        // Enemies with all the listeners found
        bool listenerFound = false;

        // Assassination Distance and Target Checker Values
        Transform assassinationTargetChosen = null;
        float assassinationNearestDistance = assassinationMinimumNearDistance;


        // Get all possible enemies
        Collider[] enemiesDetected = new Collider[10];
        int numberOfEnemies = Physics.OverlapSphereNonAlloc(c_CharacterController.c_CharacterCapsuleCollider.bounds.center, enemyAreaCheckListenersRadius, enemiesDetected, enemyAreaCheckLayermask, QueryTriggerInteraction.Ignore);


        // Enemy Checker
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Get Possible Components
            enemiesDetected[i].TryGetComponent(out LivingEntity enemyLivingEntity);
            enemiesDetected[i].TryGetComponent(out AIHearing enemyHearingEntity);
            enemiesDetected[i].TryGetComponent(out CharacterCombat playerCombat);

            // Checks for nearby target to assassinate
            if (enemyLivingEntity != null && enemyLivingEntity.IsAlive && playerCombat == null)
            {
                // WARN PLAYER OF ENEMIES
                if (enemyHearingEntity != null)
                {
                    listenerFound = true;
                }

                // CHECK NEAREST ENEMY
                // Local Position Values
                Vector3 playerPos = c_CharacterController.c_CharacterCapsuleCollider.bounds.center;
                Vector3 enemyPos = enemiesDetected[i].bounds.center;

                // Get Distance Between Entities
                float enemyToPlayerDistance = Vector3.Distance(playerPos, enemyPos);


                // Is Distance smaller than the initial distance or the last distance gotten
                if (enemyToPlayerDistance < assassinationNearestDistance)
                {
                    // Get the Player's Direction towards the enemy and get angle from Camera's forward direction
                    Vector3 playerToEnemyDir = (enemyPos - playerPos);
                    float angleBetweenGuardAndPlayer = Vector3.Angle(c_CharacterCamera.transform.forward, playerToEnemyDir);

                    // Is Angle smaller than the initial angle
                    if (angleBetweenGuardAndPlayer < assassinationLookMinimumNearAngle)
                    {
                        // Set new distance as a filtering method
                        assassinationNearestDistance = enemyToPlayerDistance;

                        // Set new target
                        assassinationTargetChosen = enemiesDetected[i].transform;
                    }
                }
            }
        }


        // Listeners Found Event
        Combat_CheckForNearbyEnemies(listenerFound);

        // Set Target Enemy
        assassinationFocusedTarget = assassinationTargetChosen;

        // Check if player is able to assassinate entity
        CanAssassinate = Combat_CheckAssassinableTarget();
    }
    #endregion

    #region AutoAim Checker Methods
    // AutoAim Checker
    void Combat_AutoAimTargetChecker()
    {
        GameObject autoAimTarget = null;

        float nearestTargetDistance = throwAutoAimMaxDistance;

        for (int i = 0; i < throwPossibleTargets.Count; i++)
        {
            if (c_PlayerHealth.IsAlive && HasRightWeaponSword)
            {
                throwPossibleTargets[i].TryGetComponent(out ITageable taggedEntity);

                if (taggedEntity.tag_UIScript.TagUI_Active)
                {
                    Vector3 playerPos = transform.position;
                    Vector3 cameraForward = c_CharacterCamera.transform.forward;
                    Vector3 taggedPos = throwPossibleTargets[i].transform.position;
                    Vector3 taggedCameraYLevelPos = new Vector3(taggedPos.x, c_CharacterCamera.transform.position.y, taggedPos.z);


                    // Get player to enemy direction and get angle from player's forward to the obtained direction
                    Vector3 cameraToTaggedEntityDir = (taggedCameraYLevelPos - c_CharacterCamera.transform.position);
                    float playerForwardToTaggedEntityAngle = Vector3.Angle(cameraForward, cameraToTaggedEntityDir);


                    float distanceFromTaggedEntity = Vector3.Distance(taggedPos, playerPos);

                    // Are both obtained angles smaller than the maximum angle allowed
                    if (playerForwardToTaggedEntityAngle < throwAutoAimAngle && distanceFromTaggedEntity < nearestTargetDistance)
                    {
                        nearestTargetDistance = distanceFromTaggedEntity;
                        autoAimTarget = throwPossibleTargets[i];
                    }
                }
            }
        }

        ThrowAutoAimTarget = autoAimTarget;
    }


    // Add Target to List
    public void Combat_AutoAimAddTag(GameObject taggedEntity)
    {
        if (!throwPossibleTargets.Contains(taggedEntity))
        {
            throwPossibleTargets.Add(taggedEntity);
        }
    }

    // Remove Target from List
    public void Combat_AutoAimRemoveTag(GameObject taggedEntity)
    {
        if (throwPossibleTargets.Contains(taggedEntity))
        {
            throwPossibleTargets.Remove(taggedEntity);
        }
    }


    // Activate Aim Icon from Tagged Entity
    void Combat_AutoAimMarkEntity()
    {
        ThrowAutoAimTarget.TryGetComponent(out ITageable taggedEntity);
        taggedEntity.tag_UIScript.TagFocusedImage_SetState(true);
    }

    // Deactivate Aim Icon from Tagged Entity
    void Combat_AutoAimUnmarkEntity()
    {
        ThrowAutoAimTarget.TryGetComponent(out ITageable taggedEntity);
        taggedEntity.tag_UIScript.TagFocusedImage_SetState(false);
    }
    #endregion

    #region Enemies Nearby Checker Methods
    void Combat_CheckForNearbyEnemies(bool foundEnemies)
    {
        if (foundEnemies)
        {
            if (ArmorGlowCoroutine == null)
            {
                if (ArmorStaticCoroutine != null)
                {
                    StopCoroutine(ArmorStaticCoroutine);
                    ArmorStaticCoroutine = null;
                }

                ArmorGlowCoroutine = StartCoroutine(Combat_EnemyNearbyArmorGlow());
            }
        }
        else
        {
            if (ArmorStaticCoroutine == null)
            {
                if (ArmorGlowCoroutine != null)
                {
                    StopCoroutine(ArmorGlowCoroutine);
                    ArmorGlowCoroutine = null;
                }

                ArmorStaticCoroutine = StartCoroutine(Combat_NoEnemiesNearbyArmorGlow());
            }
        }
    }

    IEnumerator Combat_EnemyNearbyArmorGlow()
    {
        float lerpPercentage = 0;

        while (true)
        {
            for (float timeToReachLerpTime = armorDetectionGlowLerpTime; timeToReachLerpTime > 0f; timeToReachLerpTime -= Time.deltaTime)
            {
                lerpPercentage = Mathf.InverseLerp(armorDetectionGlowLerpTime, 0f, timeToReachLerpTime);
                armorDetectionCurrentIntensity = armorDetectionGlowAnimationCurve.Evaluate(lerpPercentage);

                CharacterBackArmor.material.SetColor("_EmissionColor", armorDetectionInitialColor * armorDetectionCurrentIntensity);
                CharacterBodyArmor.material.SetColor("_EmissionColor", armorDetectionInitialColor * armorDetectionCurrentIntensity);

                yield return null;
            }
        }
    }

    IEnumerator Combat_NoEnemiesNearbyArmorGlow()
    {
        float lerpPercentage = 0f;

        while(lerpPercentage < 1f && armorDetectionCurrentIntensity != armorDetectionInitialIntensity)
        {
            for (float timeToReachLerpTime = armorDetectionStopGlowLerpTime; timeToReachLerpTime > 0f; timeToReachLerpTime -= Time.deltaTime)
            {
                lerpPercentage = Mathf.InverseLerp(armorDetectionStopGlowLerpTime, 0f, timeToReachLerpTime);
                armorDetectionCurrentIntensity = Mathf.Lerp(armorDetectionCurrentIntensity, armorDetectionInitialIntensity, lerpPercentage);

                CharacterBackArmor.material.SetColor("_EmissionColor", armorDetectionInitialColor * armorDetectionCurrentIntensity);
                CharacterBodyArmor.material.SetColor("_EmissionColor", armorDetectionInitialColor * armorDetectionCurrentIntensity);

                yield return null;
            }

            lerpPercentage = 1f;
            armorDetectionCurrentIntensity = armorDetectionInitialIntensity;

            CharacterBackArmor.material.SetColor("_EmissionColor", armorDetectionInitialColor * armorDetectionInitialIntensity);
            CharacterBodyArmor.material.SetColor("_EmissionColor", armorDetectionInitialColor * armorDetectionInitialIntensity);
        }

        yield break;
    }

    #endregion
    #endregion

    #region Assassination Methods
    // Check for assassinable targets
    bool Combat_CheckAssassinableTarget()
    {
        // Is there any enemy near player and is Player not assassinating.
        if (assassinationFocusedTarget != null && !IsAssassinating)
        {
            // Try get Assassinable and Shockable Components in AI
            assassinationFocusedTarget.TryGetComponent(out AIAssassinable assassinableTarget);
            assassinationFocusedTarget.TryGetComponent(out AIShock shockableTarget);

            // Does the enemy has the Assassinable and Shockable Components, can be Assassinated and is being shocked
            if (assassinableTarget != null && shockableTarget != null && assassinableTarget.CanBeAssassinated && shockableTarget.IsShocked)
            {
                if (!HasRightWeaponSword)
                {
                    if (thrownSword.transform.parent != assassinableTarget.transform)
                    {
                        return false;
                    }
                }

                // Local Position Values
                Vector3 playerPos = c_CharacterController.c_CharacterCapsuleCollider.bounds.center;
                Vector3 playerFeetPos = new Vector3(c_CharacterController.c_CharacterCapsuleCollider.bounds.center.x, c_CharacterController.c_CharacterCapsuleCollider.bounds.min.y + c_CharacterController.c_CharacterCapsuleCollider.radius + assassinationFeetHeightOffset, c_CharacterController.c_CharacterCapsuleCollider.bounds.center.z);
                Vector3 enemyPos = assassinationFocusedTarget.GetComponent<CapsuleCollider>().bounds.center;

                /*
                // Get enemy to player direction and get angle from enemy's back to the obtained direction
                Vector3 enemyToPlayerDir = (playerPos - enemyPos);
                float enemyBackToPlayerAngle = Vector3.Angle(-assassinationFocusedTarget.forward, enemyToPlayerDir);


                // Get player to enemy direction and get angle from player's forward to the obtained direction
                Vector3 playerToEnemyDir = (enemyPos - playerPos);
                float playerForwardToEnemyAngle = Vector3.Angle(transform.forward, playerToEnemyDir);


                // Are both obtained angles smaller than the maximum angle allowed
                if (enemyBackToPlayerAngle < assassinationMinimumNearAngle && playerForwardToEnemyAngle < assassinationMinimumNearAngle)
                {
                */
                if (Physics.Linecast(playerFeetPos, enemyPos, out RaycastHit hitFeetInfo, enemyLineCheckLayermask, QueryTriggerInteraction.Ignore))
                {
                    if (hitFeetInfo.transform == assassinationFocusedTarget)
                    {
                        // Check for obstacles from player's position
                        if (Physics.Linecast(playerPos, enemyPos, out RaycastHit hitInfo, enemyLineCheckLayermask, QueryTriggerInteraction.Ignore))
                        {
                            // Did Raycast hit player
                            if (hitInfo.transform == assassinationFocusedTarget)
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
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                /*
                }
                else
                {
                    return false;
                }
                */
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


    // Assassination Input
    void Combat_Assassinate()
    {
        // Is player not assassinating and did CheckAssassinableTarget return true
        if (CanAssassinate && !IsAssassinating)
        {
            // Safety Check: Is Assassination Coroutine playing
            if (assassinationLoopCoroutine != null)
            {
                // Stop Coroutine and display warning
                StopCoroutine(assassinationLoopCoroutine);
                Debug.LogWarning("Coroutine tried to play twice!");
            }

            // Start Assassination Coroutine
            assassinationLoopCoroutine = StartCoroutine(Combat_Assassination());
        }
    }

    // Assassination Start and Loop
    IEnumerator Combat_Assassination()
    {
        // Get Assassinable Component
        assassinationFocusedTarget.TryGetComponent(out AIAssassinable assassinatingEntity);


        // Safety Check: If Assassinable component has not been found, break Coroutine
        if (assassinatingEntity == null)
        {
            Debug.LogError("Player tried to Assassinate a non assassinable target!");
            assassinationLoopCoroutine = null;
            yield break;
        }

        // Get Enemy's back position and Ignore Collisions
        Vector3 enemyBackPosition = assassinatingEntity.transform.position - assassinatingEntity.transform.forward * assassinationMoveToBackPercentage;

        TryGetComponent(out CapsuleCollider playerCapsuleCollider);
        TryGetComponent(out CharacterController playerCharacterController);
        assassinatingEntity.TryGetComponent(out Collider enemyCollider);

        Physics.IgnoreCollision(playerCapsuleCollider, enemyCollider, true);
        Physics.IgnoreCollision(playerCharacterController, enemyCollider, true);


        // Player Assassination Setup: Set "IsAssassinating" to true, set the target the Player is assassinating, constrain Player Controller and call Animator for Assassination Event
        IsAssassinating = true;
        assassinatingTarget = assassinatingEntity;
        Combat_ActivateMovementConstrains(true);
        c_CharacterController.isCrouching = false;
        c_CharacterMeshAnimator.Assasinate();


        // Enemy Assassination Setup: Trigger Assassination Event
        assassinatingEntity.Assassination_BeingAttacked();


        // Coroutine Setup
        // Set Timer
        float transformUpdateTimer = Time.time;

        // Set Move and Rotate bools
        bool keepUpdatingMove = true;
        bool keepUpdatingRotate = true;


        // Assassination Loop
        while (IsAssassinating)
        {
            c_CharacterController.ResetVelocity();

            // Update Positions and Rotations if necessary
            if (keepUpdatingMove)
            {
                keepUpdatingMove = (Combat_MoveTowardsTarget(enemyBackPosition, transformUpdateTimer, assassinationMoveTime)) ? false : true;

                if (!keepUpdatingMove)
                {
                    Combat_SwordPickUp();
                }
            }
            if (keepUpdatingRotate)
            {
                keepUpdatingRotate = (Combat_LookAtTarget(assassinatingEntity.transform.position, transformUpdateTimer, assassinationRotationTime)) ? false : true;
            }

            yield return null;
        }

        // Reset Coroutine
        assassinationLoopCoroutine = null;

        // Break Coroutine
        yield break;
    }

    // Enemy Assassinated
    public void Combat_AssassinatedEnemy()
    {
        // Play Execution Sound
        Combat_AssassinatedEnemySound();
    }

    // Assassination Animation End
    public void Combat_EndOfAssassination()
    {
        // Assassination Completed
        IsAssassinating = false;

        c_CharacterController.isCrouching = false;

        // Deactivate Player Controller constrains, reset "assassinationFocusedTarget" if it's the assassinated entity, and reset Assassinate Coroutine to null
        Combat_ActivateMovementConstrains(false);
    }


    // Play Assassinated Enemy Sound
    public void Combat_AssassinatedEnemySound()
    {
        Combat_PlaySound(assassinationExecutedSound, transform.position, false);
    }
    #endregion

    #region Throw Methods
    // Check if player has Sword
    void Combat_CheckSwordAvailability(bool directSet)
    {
        Color colorToSet;
        colorToSet = (HasRightWeaponSword) ? Color.green : Color.red;

        if (directSet)
        {
            CharacterBracelet.material.SetColor("_EmissionColor", colorToSet);
        }
        else
        {
            if (throwSwordStateChangeCoroutine != null)
            {
                StopCoroutine(throwSwordStateChangeCoroutine);
            }

            throwSwordStateChangeCoroutine = StartCoroutine(Combat_ChangeSwordStateColor(colorToSet));
        }
    }

    // Lerp Sword State Color
    IEnumerator Combat_ChangeSwordStateColor(Color colorToChange)
    {
        Color initialColor = CharacterBracelet.material.GetColor("_EmissionColor");
        float lerpPercentage = 0f;

        for (float timeToReachLerpTime = throwWeaponReturnColorLerpTime; timeToReachLerpTime > 0; timeToReachLerpTime -= Time.deltaTime)
        {
            lerpPercentage = Mathf.InverseLerp(throwWeaponReturnColorLerpTime, 0f, timeToReachLerpTime);
            CharacterBracelet.material.SetColor("_EmissionColor", Color.Lerp(initialColor, colorToChange, lerpPercentage));
            yield return null;
        }

        CharacterBracelet.material.SetColor("_EmissionColor", colorToChange);
        throwSwordStateChangeCoroutine = null;

        yield break;
    }


    // Throw Sword
    void Combat_ThrowProjectile()
    {
        // Check if Player has the sword equiped and isn't already throwing it
        if (HasRightWeaponSword && !IsThrowing)
        {
            IsThrowing = true;
            c_CharacterMeshAnimator.Throw();
        }
    }

    // Throwing Sword
    public void Combat_ThrowingProjectile()
    {
        // Player no longer has the sword and hide the sword mesh
        HasRightWeaponSword = false;
        rightWeaponSword.SetActive(hasRightWeaponSword);

        // Check if Player is aiming
        bool isPlayerAiming = (c_CharacterController.AimingCurrentState == ThirdPersonController.AimingState.Aiming);

        // Calculate Throw Direction
        Vector3 throwDirection = Vector3.zero;


        if (isPlayerAiming)
        {
            throwDirection = (c_CharacterCamera.cameraRayHit.transform != null)
                ? c_CharacterCamera.cameraRayHit.point - CharacterRightHand.position
                : (c_CharacterCamera.transform.position + c_CharacterCamera.transform.forward * 30) - CharacterRightHand.position;
        }
        else
        {
            if (ThrowAutoAimTarget != null)
            {
                ThrowAutoAimTarget.TryGetComponent(out CapsuleCollider hitCapsule);

                throwDirection = hitCapsule.bounds.center - CharacterRightHand.position;
            }
            else
            {
                throwDirection = (c_CharacterCamera.cameraRayHit.transform != null && c_CharacterCamera.cameraRayHit.distance >= throwRaycastDistanceThreshold)
                    ? c_CharacterCamera.cameraRayHit.point - CharacterRightHand.position
                    : (c_CharacterCamera.transform.position + c_CharacterCamera.transform.forward * 30) - CharacterRightHand.position;
            }
        }

        throwDirection = throwDirection.normalized;

        // Instantiate Projectile
        thrownSword = Instantiate(weaponProjectile, CharacterRightHand.position, Quaternion.LookRotation(throwDirection, Vector3.up));
        thrownSword.TryGetComponent(out SwordProjectile swordProjectile);
        thrownSword.TryGetComponent(out CapsuleCollider swordCollider);


        // Ignore Collisions with Player
        Physics.IgnoreCollision(swordCollider, c_CharacterController.c_CharacterController, true);
        Physics.IgnoreCollision(swordCollider, c_CharacterController.c_CharacterCapsuleCollider, true);


        // Activate projectile and launch
        thrownSword.SetActive(true);

        swordProjectile.projectileSpeed = (isPlayerAiming) ? throwForcedAimPower : throwAutoAimPower;
        swordProjectile.ProjectileLaunch();

        // Play SoundBubble
        Combat_PlayThrowSound();

        // Invoke Throw Event
        OnSwordThrown?.Invoke(thrownSword);


        // Suscribe to Impact events
        swordProjectile.OnProjectileCollision += Combat_OnProjectileCollision;

        // No longer throwing
        IsThrowing = false;
        throwCanCallReturnWeapon = false;


        // Start Return Weapon Cooldown
        // Safety Check: If Return Cooldown Coroutine is not null, stop the coroutine
        if (throwReturnSwordCooldownCoroutine != null)
        {
            StopCoroutine(throwReturnSwordCooldownCoroutine);
        }
        throwReturnSwordCooldownCoroutine = StartCoroutine(Combat_ReturnWeaponCooldownTime());
    }

    // Pickup Sword
    public void Combat_SwordPickUp()
    {
        // Check if player didn't pick up a sword yet
        if (!HasRightWeaponSword)
        {
            // Weapon no longer returning
            throwWeaponReturning = false;

            // Player now has the sword
            HasRightWeaponSword = true;

            // Activate Sword Mesh
            rightWeaponSword.SetActive(HasRightWeaponSword);

            // If thrown sword is still null
            if (thrownSword != null)
            {
                // Trigger On Sword Pickup
                OnSwordRetreived?.Invoke();

                // Unsuscribe from such sword
                thrownSword.GetComponent<SwordProjectile>().OnProjectileCollision -= Combat_OnProjectileCollision;

                // Destroy projectile
                Destroy(thrownSword);
                thrownSword = null;
            }
        }
    }

    // On Electric Collision
    public void Combat_OnProjectileCollision(Collision collidedWith)
    {
        collidedWith.collider.TryGetComponent(out ShockableEntity shockableEntity);

        if (shockableEntity != null)
        {
            if (throwReturnSwordCooldownCoroutine != null)
            {
                StopCoroutine(throwReturnSwordCooldownCoroutine);

                // Clear Throw Return Cooldown Coroutine
                throwReturnSwordCooldownCoroutine = null;
            }

            OnThrowCooldownChange?.Invoke(1f);

            // Player is now able to Call Return again
            throwCanCallReturnWeapon = true;
        }
    }


    #region Sword Magnet Methods
    // Return Sword from any position with stamina reduction
    void Combat_ReturnSwordRanged()
    {
        // If player has thrown the sword, can call sword to return, and the sword isn't already returning
        if (thrownSword != null && throwCanCallReturnWeapon && !throwWeaponReturning)
        {
            // Safety Check: If Return Coroutine is not null, stop the coroutine
            if (throwReturnSwordCoroutine != null)
            {
                StopCoroutine(throwReturnSwordCoroutine);
            }

            // Start Return Coroutine
            throwReturnSwordCoroutine = StartCoroutine(Combat_ReturningSword(thrownSword, CharacterRightHand));
        }
    }

    // Return Sword Loop
    IEnumerator Combat_ReturningSword(GameObject weaponToReturn, Transform socketToReturn)
    {
        // Sword Original Position before calling return
        Vector3 swordOriginPosition = thrownSword.transform.position;

        // Remove Sword from Collided Surface
        thrownSword.transform.parent = null;

        thrownSword.TryGetComponent(out SwordProjectile swordProjectile);
        swordProjectile.ProjectileReturning();


        // Weapon is now returning and cannot call for return
        throwWeaponReturning = true;


        // If there's Sound for throwing the dagger, play it
        if (throwWeaponReturnSound != null)
        {
            Combat_PlaySound(throwWeaponReturnSound, CharacterRightHand.position, false);
        }

        c_CharacterMeshAnimator.MagnetStart();

        // Loop: Keep looping until player picks up the sword
        while (!HasRightWeaponSword)
        {
            // Lerp Thrown Position to player's hand
            thrownSword.transform.position = Vector3.Lerp(thrownSword.transform.position, CharacterRightHand.position, throwWeaponReturnSpeed * Time.deltaTime);
            thrownSword.transform.rotation = Quaternion.Slerp(thrownSword.transform.rotation, rightWeaponSword.transform.rotation, throwWeaponReturnRotationSpeed * Time.deltaTime);


            // Coroutine Loop Fixed Time
            yield return null;
        }

        c_CharacterMeshAnimator.MagnetEnd();

        // Reset Returning Sword Coroutine reference
        throwReturnSwordCoroutine = null;

        // Stop Coroutine
        yield break;
    }

    // Return Sword Cooldown
    IEnumerator Combat_ReturnWeaponCooldownTime()
    {
        // Cooldown Timer
        for (float throwCooldownTimer = throwWeaponReturnCooldownTime; throwCooldownTimer > 0f; throwCooldownTimer -= Time.deltaTime)
        {
            OnThrowCooldownChange?.Invoke(Mathf.InverseLerp(throwWeaponReturnCooldownTime, 0f, throwCooldownTimer));

            // Skip frame
            yield return null;
        }

        OnThrowCooldownChange?.Invoke(Mathf.InverseLerp(throwWeaponReturnCooldownTime, 0f, 0f));

        // Player is now able to Call Return again
        throwCanCallReturnWeapon = true;

        // Clear Throw Return Cooldown Coroutine
        throwReturnSwordCooldownCoroutine = null;

        // Stop Coroutine
        yield break;
    }
    #endregion

    #region Throw Play Sounds
    // Play Throw Sound
    void Combat_PlayThrowSound()
    {
        c_CombatSpeaker.CreateSoundBubble(throwWeaponSound, CharacterRightHand.position, gameObject, true);
    }
    #endregion
    #endregion

    #region Particle Methods
    public void Combat_PlayMagnetParticle()
    {
        magnetParticle.Play();
    }
    #endregion

    #region UI
    void Combat_UI_AssassinationAvailable()
    {
        assassinateCurrentUI.TargetToFollow = assassinationFocusedTarget;
        assassinateCurrentUI.TargetOffset = Vector3.up * 2;
        assassinateCurrentUI.gameObject.SetActive(true);
    }

    void Combat_UI_AssassinationNotAvailable()
    {
        assassinateCurrentUI.gameObject.SetActive(false);
    }
    #endregion

    #region Combat Tools
    // Play Combat Sounds
    void Combat_PlaySound(SoundInfo soundToPlay, Vector3 soundOrigin, bool onlyAudio)
    {
        c_CombatSpeaker.CreateSoundBubble(soundToPlay, soundOrigin, gameObject, onlyAudio);
    }

    // Look At Target Method Helpers
    bool Combat_LookAtTarget(Vector3 targetPos, float speed)
    {
        // Get Player to Target Direction
        Vector3 playerToTargetDir = targetPos - transform.position;

        // Get Rotation to Target
        Quaternion targetLookRotation = Quaternion.LookRotation(new Vector3(playerToTargetDir.x, 0f, playerToTargetDir.z), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLookRotation, speed * Time.deltaTime);

        // Get Angle from Player's forward to Target Position
        float angle = Vector3.Angle(transform.forward, targetPos);

        // Safety Threshold: As we get closer to the target rotation, it really slows down at the end. This check will stop rotation as soon as the threshold has been reached. 
        if (angle <= 2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool Combat_LookAtTarget(Vector3 targetPos, float timer, float timeToEnd)
    {
        // Get Lerp percentage
        float lerpTime = Mathf.InverseLerp(timer, timer + timeToEnd, Time.time);

        // Get Player to Target Direction
        Vector3 playerToTargetDir = targetPos - transform.position;

        // Get Rotation to Target
        Quaternion targetLookRotation = Quaternion.LookRotation(new Vector3(playerToTargetDir.x, 0f, playerToTargetDir.z), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLookRotation, lerpTime);

        // If Lerp has ended, stop rotating
        if (lerpTime >= 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Move Towards Target Method Helpers
    bool Combat_MoveTowardsTarget(Vector3 targetPos, float speed)
    {
        // Get Lerp position
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

        // Get Lerp distance
        float distanceBetweenPlayerAndTarget = Vector3.Distance(transform.position, targetPos);

        // Safety Threshold: As we get closer to the target position, it really slows down at the end. This check will stop movement as soon as the threshold has been reached. 
        if (distanceBetweenPlayerAndTarget <= 0.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool Combat_MoveTowardsTarget(Vector3 targetPos, float timer, float timeToEnd)
    {
        // Get Lerp percentage
        float lerpTime = Mathf.InverseLerp(timer, timer + timeToEnd, Time.time);

        // Lerp Player Position to Target
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpTime);

        // If Lerp has ended, stop moving
        if (lerpTime >= 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Constrain Player Movement
    public void Combat_ActivateMovementConstrains(bool activate)
    {
        c_CharacterController.constrain_Move = activate;
        c_CharacterController.constrain_Crouch = activate;
        c_CharacterController.constrain_Rotate = activate;
        c_CharacterController.constrain_Aim = activate;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (enemyAreaCheckCoroutine != null)
        {
            Gizmos.DrawWireSphere(transform.position, enemyAreaCheckListenersRadius);
        }

        if (assassinationFocusedTarget != null)
        {
            // Local Position Values
            Vector3 playerPos = c_CharacterController.c_CharacterCapsuleCollider.bounds.center;
            Vector3 playerFeetPos = new Vector3(c_CharacterController.c_CharacterCapsuleCollider.bounds.center.x, c_CharacterController.c_CharacterCapsuleCollider.bounds.min.y + c_CharacterController.c_CharacterCapsuleCollider.radius + assassinationFeetHeightOffset, c_CharacterController.c_CharacterCapsuleCollider.bounds.center.z);
            Vector3 enemyPos = assassinationFocusedTarget.GetComponent<CapsuleCollider>().bounds.center;

            // Player to Enemy Line
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerPos, enemyPos);
            Gizmos.DrawLine(playerFeetPos, enemyPos);

            // Player Model Forward Ray
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(playerPos, transform.forward * assassinationMinimumNearDistance);

            // Enemy Model Forward Ray
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(enemyPos, -assassinationFocusedTarget.forward * assassinationMinimumNearDistance);

            if (CanAssassinate)
            {
                // Enemy Back Assassination Position
                Vector3 enemyBackPosition = enemyPos - assassinationFocusedTarget.transform.forward * assassinationMoveToBackPercentage;

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(enemyBackPosition, 0.2f);
            }
        }
    }
    #endregion
}