using System.Collections;
using UnityEngine;

public class RangedEnemyBrain : AIBrain, ITageable
{
    #region Ranged Enemy Components
    [Header("Ranged Enemy Components")]
    public CapsuleCollider  EnemyCollider;
    public RifleWeapon      EnemyEquipedWeapon;
    [SerializeField] private Transform enemyChest;

    [HideInInspector] public SoundSpeaker   SoundSpeakerComponent;
    [HideInInspector] public Animator       AnimatorComponent;

    [HideInInspector] public LivingEntity   LivingEntityComponent;

    [HideInInspector] public AIMoveAgent    MoveAgentComponent;
    [HideInInspector] public AIPatrol       PatrolComponent;
    [HideInInspector] public AIHearing      HearingComponent;
    [HideInInspector] public AIScanHES      ScanHESComponent;
    [HideInInspector] public AISuspect      SuspectComponent;
    [HideInInspector] public AIShock        ShockComponent;
    [HideInInspector] public AIAssassinable AssassinableComponent;
    #endregion

    #region Animator Component Values
    [Header("Ranged Enemy Animator Values")]
    // Living Entity
    [SerializeField] protected string Animator_TriggerEntityHit;

    [SerializeField] protected string Animator_BoolEntityIsDead;
    [SerializeField] protected string Animator_TriggerEntityDeath;


    // Move Agent
    [SerializeField] protected string Animator_FloatAgentSpeed;

    // Hearing
    [SerializeField] protected string Animator_TriggerNoiseHeard;

    // ScanHES
    [SerializeField] protected string Animator_BoolScanHESDetectedEntity;

    // Suspect
    [SerializeField] protected string Animator_BoolAlertStatus;
    [SerializeField] protected string Animator_TriggerAlertExceeded;

    // Shock
    [SerializeField] protected string Animator_TriggerOnStun;
    [SerializeField] protected string Animator_TriggerOnStunStop;

    // Assassinable
    [SerializeField] protected string Animator_BoolBeingAssassinated;
    [SerializeField] protected string Animator_TriggerOnAssassination;


    // Collider
    [SerializeField] protected string Animator_TriggerOnTouched;
    #endregion

    #region Particle Component Values
    [Header("Particle Component Values")]
    [SerializeField] private GameObject shockParticlePrefab;
    private GameObject currentShockParticle;  
    #endregion

    #region Living Entity Component Values
    [Header("Living Entity Component Extra Values")]
    [SerializeField] private SoundInfo deathSound;
    [SerializeField] private SoundInfo hitGroundSound;
    [SerializeField] private SoundInfo daggerHitSound;
    [SerializeField] private SoundInfo lastBreathSound;
    #endregion

    #region Move Agent Component Values
    [Header("Move Agent Component Values")]
    public float MoveAgent_WalkSpeed;
    public float MoveAgent_RunSpeed;
    public float RotateAgent_TurnSpeed;
    #endregion

    #region Hearing Component Values
    [Header("Hearing Component Values")]
    [Range(0f, 1f)] [SerializeField] protected float Hearing_IntensityPriority;
    [SerializeField] protected float Hearing_SoundNegationTime;

    protected float Hearing_LastSoundIntensityHeard;

    protected Coroutine Hearing_NegateSoundsCoroutine;
    #endregion

    #region ScanHES Component Values
    [Header("ScanHES Component Values")]
    [SerializeField] protected float ScanHES_TrackTime;
    [SerializeField] protected SoundInfo ScanHES_SoundInfo;
    protected bool ScanHES_BeingTracked;

    protected Coroutine ScanHES_TrackCoroutine;
    #endregion

    #region Shock Component Values
    [Header("Shock Component Values")]
    public float shock_StunTime;

    private Coroutine shock_StunCoroutine;
    #endregion


    #region Sound Step Values
    [Header("Sound Step Values")]
    [SerializeField] private SoundInfo stepSound;
    [SerializeField] private int stepsToTake;
    private int stepsTaken;
    #endregion



    #region UI Values
    [Header("UI Values")]
    [SerializeField] private GameObject UI_TagPrefab;

    private TagUI UI_TagCurrentUI;
    public TagUI tag_UIScript
    {
        get { return UI_TagCurrentUI; }
    }

    [SerializeField] private Transform UI_TagPosition;
    public Transform tag_Position
    {
        get { return UI_TagPosition; }
    }

    [Space(10)]

    [SerializeField] private Sprite UI_TagEntitySprite;
    [SerializeField] private Sprite UI_TagEntityBackgroundSprite;
    [SerializeField] private Sprite UI_TagEntityBackgroundFillSprite;
    [SerializeField] private Sprite UI_TagEntityFocusedSprite;
    #endregion


    protected virtual void Awake() => AI_GetComponents();

    protected virtual void AI_GetComponents()
    {
        #region Get AI Needed Components
        TryGetComponent(out SoundSpeaker thisEntitySoundSpeaker);
        if (thisEntitySoundSpeaker != null)
        {
            SoundSpeakerComponent = thisEntitySoundSpeaker;
        }
        else
        {
            SoundSpeakerComponent = gameObject.AddComponent<SoundSpeaker>();
        }

        TryGetComponent(out Animator thisEntityAnimator);
        AnimatorComponent = thisEntityAnimator;

        TryGetComponent(out LivingEntity thisLivingEntity);
        LivingEntityComponent = thisLivingEntity;

        TryGetComponent(out AIHearing thisEntityHearing);
        HearingComponent = thisEntityHearing;

        TryGetComponent(out AIMoveAgent thisEntityMovingAgent);
        MoveAgentComponent = thisEntityMovingAgent;

        TryGetComponent(out AIPatrol thisEntityPatrol);
        PatrolComponent = thisEntityPatrol;

        TryGetComponent(out AISuspect thisEntitySuspect);
        SuspectComponent = thisEntitySuspect;

        TryGetComponent(out AIAssassinable thisEntityAssassinable);
        AssassinableComponent = thisEntityAssassinable;

        TryGetComponent(out AIShock thisEntityShock);
        ShockComponent = thisEntityShock;

        TryGetComponent(out AIScanHES thisEntityScanHES);
        ScanHESComponent = thisEntityScanHES;
        #endregion

        #region Get and Set UI components
        if (UI_TagCurrentUI == null)
        {
            UI_TagCurrentUI = Instantiate(UI_TagPrefab, UI_TagPosition.transform.position, Quaternion.identity, UI_TagPosition.transform).GetComponent<TagUI>();
            UI_TagCurrentUI.TagEntityImage_SetSprite(UI_TagEntitySprite, UI_TagEntityBackgroundSprite, UI_TagEntityBackgroundFillSprite, UI_TagEntityFocusedSprite);
        }
        #endregion
    }


    // Suscribe Ranged Enemy Brain methods to the following events //
    protected virtual void OnEnable()
    {
        #region Animator Events
        #endregion


        #region Living Entity Events
        LivingEntityComponent.OnEntityHit += RangedEnemy_LivingEntity_Hit;
        #endregion


        #region Move Agent Events
        MoveAgentComponent.OnAgentVelocityChanged += RangedEnemy_Animator_UpdateAgentVelocity;
        #endregion

        #region Patrol Events
        #endregion

        #region Hearing Events
        HearingComponent.OnSoundListened += RangedEnemy_Hearing_NoiseHeard;
        #endregion

        #region Scan HES Events
        ScanHESComponent.OnDetectedEntity += RangedEnemy_ScanHES_DetectedTarget;
        #endregion

        #region Suspect Events
        SuspectComponent.OnAlertTriggered += RangedEnemy_Suspect_Alerted;
        SuspectComponent.OnAlertEnd += RangedEnemy_Suspect_NoLongerAlerted;

        SuspectComponent.OnSuspicionLevelChange += RangedEnemy_UI_ShowSuspect;
        #endregion

        #region Shock Events
        ShockComponent.OnShocked += RangedEnemy_Shock_OnShock;
        ShockComponent.OnShockedEnd += RangedEnemy_Shock_OnShockEnd;
        #endregion

        #region Assassinable Events
        AssassinableComponent.OnBeingAssassinated += RangedEnemy_Assassination_OnBeingAssassinated;
        AssassinableComponent.OnAssassinated += RangedEnemy_LivingEntity_Death;
        #endregion


        #region UI
        tag_UIScript.OnTagStateChange += RangedEnemy_UI_TagStateChange;
        #endregion
    }

    // Unsuscribe Ranged Enemy Brain methods to the following events //
    protected virtual void OnDisable()
    {
        #region Animator Events
        #endregion


        #region Living Entity Events
        LivingEntityComponent.OnEntityHit -= RangedEnemy_LivingEntity_Hit;
        #endregion


        #region Move Agent Events
        MoveAgentComponent.OnAgentVelocityChanged -= RangedEnemy_Animator_UpdateAgentVelocity;
        #endregion

        #region Patrol Events
        #endregion

        #region Hearing Events
        HearingComponent.OnSoundListened -= RangedEnemy_Hearing_NoiseHeard;
        #endregion

        #region Scan HES Events
        ScanHESComponent.OnDetectedEntity -= RangedEnemy_ScanHES_DetectedTarget;
        #endregion

        #region Suspect Events
        SuspectComponent.OnAlertTriggered -= RangedEnemy_Suspect_Alerted;
        SuspectComponent.OnAlertEnd -= RangedEnemy_Suspect_NoLongerAlerted;

        SuspectComponent.OnSuspicionLevelChange -= RangedEnemy_UI_ShowSuspect;
        #endregion

        #region Shock Events
        ShockComponent.OnShocked -= RangedEnemy_Shock_OnShock;
        ShockComponent.OnShockedEnd -= RangedEnemy_Shock_OnShockEnd;
        #endregion

        #region Assassinable Events
        AssassinableComponent.OnBeingAssassinated -= RangedEnemy_Assassination_OnBeingAssassinated;
        AssassinableComponent.OnAssassinated -= RangedEnemy_LivingEntity_Death;
        #endregion


        #region UI
        tag_UIScript.OnTagStateChange -= RangedEnemy_UI_TagStateChange;
        #endregion
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        PlayerPosition = FindObjectOfType<ThirdPersonController>();
    }


    #region Ranged Enemy Methods
    #region ANIMATOR COMPONENT METHODS
    #region Living Entity
    void RangedEnemy_Animator_OnHit()
    {
        AnimatorComponent.SetTrigger(Animator_TriggerEntityHit);
    }

    void RangedEnemy_Animator_Death()
    {
        AnimatorComponent.SetTrigger(Animator_TriggerEntityDeath);
        AnimatorComponent.SetBool(Animator_BoolEntityIsDead, LivingEntityComponent.IsAlive);
    }


    #region Sound Methods
    void Animator_PlayDeathSound()
    {
        SoundSpeakerComponent.CreateSoundBubble(deathSound, null, gameObject, true);
    }
    void Animator_PlayHitGroundSound()
    {
        SoundSpeakerComponent.CreateSoundBubble(hitGroundSound, null, gameObject, true);
    }
    void Animator_PlayDaggerHitSound()
    {
        SoundSpeakerComponent.CreateSoundBubble(daggerHitSound, null, gameObject, true);
    }
    void Animator_PlayLastBreathSound()
    {
        SoundSpeakerComponent.CreateSoundBubble(lastBreathSound, null, gameObject, true);
    }
    #endregion
    #endregion


    #region Move Agent
    void RangedEnemy_Animator_UpdateAgentVelocity(float agentVelocityMagnitude)
    {
        AnimatorComponent.SetFloat(Animator_FloatAgentSpeed, agentVelocityMagnitude);
    }
    #endregion

    #region Hearing
    void RangedEnemy_Animator_NoiseDetected()
    {
        AnimatorComponent.SetTrigger(Animator_TriggerNoiseHeard);
    }
    #endregion

    #region ScanHES
    void RangedEnemy_Animator_ScanHESDetectedEntity()
    {
        AnimatorComponent.SetBool(Animator_BoolScanHESDetectedEntity, ScanHES_BeingTracked);
    }
    #endregion

    #region Suspect
    void RangedEnemy_Animator_IsAlerted()
    {
        AnimatorComponent.SetBool(Animator_BoolAlertStatus, true);
        RangedEnemy_Animator_AlertExceeded();
    }

    void RangedEnemy_Animator_AlertExceeded()
    {
        AnimatorComponent.SetTrigger(Animator_TriggerAlertExceeded);
    }

    void RangedEnemy_Animator_NoLongerAlerted()
    {
        AnimatorComponent.SetBool(Animator_BoolAlertStatus, false);
        AnimatorComponent.ResetTrigger(Animator_TriggerAlertExceeded);
    }
    #endregion

    #region Shock
    void RangedEnemy_Animator_OnStun()
    {
        AnimatorComponent.SetTrigger(Animator_TriggerOnStun);
    }
    void RangedEnemy_Animator_OnStunStop()
    {
        AnimatorComponent.SetTrigger(Animator_TriggerOnStunStop);
    }
    #endregion

    #region Assassinable
    void RangedEnemy_Animator_OnAssassination()
    {
        AnimatorComponent.SetBool(Animator_BoolBeingAssassinated, true);
        AnimatorComponent.SetTrigger(Animator_TriggerOnAssassination);
    }
    #endregion


    #region Collider
    void RangedEnemy_Animator_Touched()
    {
        AnimatorComponent.SetTrigger(Animator_TriggerOnTouched);
    }
    #endregion

    #region Animation Step
    void RangedEnemy_Animator_Step(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            if (stepsTaken >= stepsToTake)
            {
                SoundSpeakerComponent.CreateSoundBubble(stepSound, transform.position, gameObject, false);
                stepsTaken = 0;
            }
            else
            {
                stepsTaken++;
            }
        }
    }
    #endregion
    #endregion


    #region LIVING ENTITY COMPONENT METHODS
    void RangedEnemy_LivingEntity_Hit()
    {
        RangedEnemy_Animator_OnHit();
    }
    void RangedEnemy_LivingEntity_Death()
    {       
        RangedEnemy_Collider_SetActive(false);

        RangedEnemy_MoveAgent_Disable();
        RangedEnemy_ScanHES_DetectionSetActive(false);
        SuspectComponent.Suspect_SetAlertLevel(false, 0f);

        RangedEnemy_Shock_OnShockEnd();

        RangedEnemy_UI_StopTag();

        SteamManager._Instance?.EnemyKilled();

        LivingEntityComponent.Death();
    }
    #endregion


    #region MOVE AGENT COMPONENT METHODS
    void RangedEnemy_MoveAgent_Activate()
    {
        MoveAgentComponent.Agent_SetActive(false);
    }

    void RangedEnemy_MoveAgent_Disable()
    {
        MoveAgentComponent.Agent_SetActive(false);
    }
    #endregion

    #region PATROL COMPONENT METHODS
    #endregion

    #region HEARING COMPONENT METHODS
    void RangedEnemy_Hearing_Enable()
    {
        HearingComponent.Hearing_SetActive(true);
    }
    void RangedEnemy_Hearing_Disable()
    {
        HearingComponent.Hearing_SetActive(false);
    }

    void RangedEnemy_Hearing_NoiseHeard()
    {
        if (RangedEnemy_Hearing_CheckForHighSoundIntensity())
        {
            detectedTarget = HearingComponent.listenedSoundPosition;

            SuspectComponent.Suspect_SetAlertLevel(false, 1f);
            RangedEnemy_Animator_NoiseDetected();
        }
    }

    bool RangedEnemy_Hearing_CheckForHighSoundIntensity()
    {
        if (HearingComponent.listenedSoundIntensity >= Hearing_LastSoundIntensityHeard || Hearing_NegateSoundsCoroutine == null)
        {
            Hearing_LastSoundIntensityHeard = HearingComponent.listenedSoundIntensity;

            if (ScanHES_BeingTracked && Hearing_LastSoundIntensityHeard >= Hearing_IntensityPriority)
            {
                RangedEnemy_ScanHES_EndTracking();
            }

            if (Hearing_NegateSoundsCoroutine != null)
            {
                StopCoroutine(Hearing_NegateSoundsCoroutine);
            }
            Hearing_NegateSoundsCoroutine = StartCoroutine(RangedEnemy_Hearing_SoundNegation());

            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator RangedEnemy_Hearing_SoundNegation()
    {
        yield return new WaitForSeconds(Hearing_SoundNegationTime);

        Hearing_NegateSoundsCoroutine = null;

        yield break;
    }

    #endregion

    #region SCANHES COMPONENT METHODS
    void RangedEnemy_ScanHES_DetectionSetActive(bool stateToSet)
    {
        ScanHESComponent.ScanHES_DetectionSetActive(stateToSet);
    }

    public void RangedEnemy_ScanHES_EmitScan()
    {
        ScanHESComponent.ScanHES_EmitScan(SoundSpeakerComponent, ScanHES_SoundInfo);
    }
    void RangedEnemy_ScanHES_DetectedTarget()
    {
        if (LivingEntityComponent.IsAlive && ScanHESComponent.ScanDetectionActive && ShockComponent.CanBeShocked)
        {
            SuspectComponent.Suspect_SetAlertLevel(false, 1f);

            RangedEnemy_ScanHES_StartTracking();
        }
    }

    void RangedEnemy_ScanHES_StartTracking()
    {
        if (ScanHES_TrackCoroutine != null)
        {
            StopCoroutine(ScanHES_TrackCoroutine);
        }

        ScanHES_BeingTracked = true;
        RangedEnemy_Animator_ScanHESDetectedEntity();
        ScanHES_TrackCoroutine = StartCoroutine(RangedEnemy_ScanHES_TrackLoop());
    }
    IEnumerator RangedEnemy_ScanHES_TrackLoop()
    {
        // Set cooldown time. If cooldown time is still bigger than 0, subtract frame time
        for (float cooldownTime = ScanHES_TrackTime; cooldownTime > 0f; cooldownTime -= Time.deltaTime)
        {
            // Keep Track of Entity
            detectedTarget = PlayerPosition.transform.position;

            // Wait for next frame
            yield return null;
        }

        ScanHES_TrackCoroutine = null;
        RangedEnemy_ScanHES_EndTracking();

        yield break;
    }
    public void RangedEnemy_ScanHES_EndTracking()
    {
        if (ScanHES_TrackCoroutine != null)
        {
            StopCoroutine(ScanHES_TrackCoroutine);
        }

        ScanHES_BeingTracked = false;
        RangedEnemy_Animator_ScanHESDetectedEntity();
    }
    #endregion

    #region SUSPECT COMPONENT METHODS
    void RangedEnemy_Suspect_Alerted()
    {
        RangedEnemy_Animator_IsAlerted();
        SuspectComponent.Suspect_SetAlertLevel(false, 1f);
    }
    void RangedEnemy_Suspect_NoLongerAlerted()
    {
        RangedEnemy_Animator_NoLongerAlerted();
    }
    #endregion

    #region ASSASSINABLE COMPONENT METHODS
    void RangedEnemy_Assassination_OnBeingAssassinated()
    {
        if (shock_StunCoroutine != null)
        {
            StopCoroutine(shock_StunCoroutine);
        }

        RangedEnemy_Collider_SetActive(false);

        RangedEnemy_Animator_OnAssassination();

        RangedEnemy_MoveAgent_Disable();
        RangedEnemy_Hearing_Disable();
        RangedEnemy_ScanHES_DetectionSetActive(false);
        SuspectComponent.Suspect_SetAlertLevel(false, 0f);
    }
    #endregion

    #region SHOCK COMPONENT METHODS
    void RangedEnemy_Shock_OnShock()
    {
        if (shock_StunCoroutine != null)
        {
            StopCoroutine(shock_StunCoroutine);
        }

        shock_StunCoroutine = StartCoroutine(RangedEnemy_Shock_BeingShockedLoop());
    }
    IEnumerator RangedEnemy_Shock_BeingShockedLoop()
    {
        RangedEnemy_Animator_OnStun();

        MoveAgentComponent.Agent_ClearDestination();
        RangedEnemy_Hearing_Disable();

        if (currentShockParticle == null) 
        {
            currentShockParticle = Instantiate(shockParticlePrefab, enemyChest.position, Quaternion.identity, null);
            TransformFollower shockFollower = currentShockParticle.AddComponent<TransformFollower>();
            currentShockParticle.TryGetComponent(out ParticleSystem shockParticleSystem);
            currentShockParticle.TryGetComponent(out AudioSource shockParticleAudio);

            shockFollower.followMode = TransformFollower.FollowMode.Tranform;
            shockFollower.transformTarget = enemyChest;

            shockParticleSystem.Play();
            shockParticleAudio.Play();
        }
        yield return new WaitForSeconds(shock_StunTime);

        shock_StunCoroutine = null;
        ShockComponent.Shock_OnShockEnd();

        yield break;
    }
    void RangedEnemy_Shock_OnShockEnd()
    {
        if (shock_StunCoroutine != null)
        {
            StopCoroutine(shock_StunCoroutine);
            shock_StunCoroutine = null;
        }

        Destroy(currentShockParticle);
        currentShockParticle = null;
        

        if (LivingEntityComponent.IsAlive && AssassinableComponent.CanBeAssassinated)
        {
            RangedEnemy_Animator_OnStunStop();

            RangedEnemy_Hearing_Enable();
            RangedEnemy_Suspect_Alerted();
        }
    }
    #endregion


    #region COLLIDER COMPONENT METHODS
    void RangedEnemy_Collider_SetActive(bool active)
    {
        EnemyCollider.enabled = active;
    }

    public void RangedEnemy_Collider_Touched()
    {
        RangedEnemy_Animator_Touched();

        detectedTarget = PlayerPosition.transform.position;

        SuspectComponent.Suspect_SetAlertLevel(false, 1f);
    }
    #endregion


    #region UI COMPONENT METHODS
    #region Tag UI
    public void OnTag(float soundIntensity)
    {
        if (LivingEntityComponent.IsAlive)
        {
            UI_TagCurrentUI.Tagged(soundIntensity);
        }
    }
    public void RangedEnemy_UI_StopTag()
    {
        UI_TagCurrentUI.StopTag();
    }

    void RangedEnemy_UI_TagStateChange()
    {
        if (tag_UIScript.TagUI_Active)
        {
            tag_UIScript.tagUI_PlayerCombat.Combat_AutoAimAddTag(gameObject);
        }
        else
        {
            tag_UIScript.tagUI_PlayerCombat.Combat_AutoAimRemoveTag(gameObject);
        }
    }
    void RangedEnemy_UI_ShowSuspect(float alertLevel)
    {
        tag_UIScript.TagEntityImage_AlertState(alertLevel);
    }
    #endregion
    #endregion
    #endregion
}