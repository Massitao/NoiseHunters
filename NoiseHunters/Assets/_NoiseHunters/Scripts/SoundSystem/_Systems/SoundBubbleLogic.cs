using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBubbleLogic : MonoBehaviour
{
    [Header("Sound Bubble Components")]
    [HideInInspector] public GlobalSoundList GlobalList;
    [HideInInspector] public SoundSpeaker SoundProducedBySpeaker;
    [HideInInspector] public SoundInfo SoundData;
    [HideInInspector] public GameObject SoundInstigator;
    [HideInInspector] private float _soundBubbleRadius;

    private SphereCollider _bubbleTrigger;
    private ScannerEffect _bubbleScanEffect;

    private Coroutine _soundPropagationCoroutine;


    [Header("Sound Instigator")]
    private SoundInstigatorType _soundTypeOfInstigator;
    private enum SoundInstigatorType { Player, Entity, Enemy }


    [Header("Sound Variables")]
    public SoundInfo.SoundTypes SoundBubbleType;
    private float _soundEntityIntensity;


    [Header("Sound Entity Filter")]
    private List<SoundListener> m_entitiesListenersRegistered = new List<SoundListener>();
    private List<SoundSpeaker> m_entitiesSpeakersRegistered = new List<SoundSpeaker>();

    private List<OutlineAdder> outlinesInEnemy = new List<OutlineAdder>();
    private List<SoundSpeaker> soundSpeakersDetected = new List<SoundSpeaker>();


    private void Awake()
    {
        _bubbleTrigger = GetComponent<SphereCollider>();
    }

    void OnEnable()
    {
        _bubbleTrigger.radius = 0f;
        _bubbleTrigger.enabled = false;
    }

    private void OnDisable()
    {
        #region Reset References
        SoundProducedBySpeaker = null;
        SoundInstigator = null;
        SoundData = null;
        _bubbleScanEffect = null;
        #endregion

        #region Reset Registered Listeners and Silenced Entity Speakers
        m_entitiesListenersRegistered.Clear();

        for (int i = 0; i < m_entitiesSpeakersRegistered.Count; i++)
        {
            if (m_entitiesSpeakersRegistered[i].silencersAffectingGO.Contains(gameObject))
            {
                m_entitiesSpeakersRegistered[i].silencersAffectingGO.Remove(gameObject);
            }
        }
        m_entitiesSpeakersRegistered.Clear();
        #endregion

        #region Stop Sound Propagation Coroutine
        if (_soundPropagationCoroutine != null)
        {
            StopCoroutine(_soundPropagationCoroutine);
            _soundPropagationCoroutine = null;
        }
        #endregion

        _bubbleTrigger.enabled = false;

        Destroy(gameObject);
    }



    public void SetBubbleLogic(GlobalSoundList soundList, SoundSpeaker soundSpeaker, SoundInfo soundInfo, ScannerEffect scanToTrack, GameObject instigator, float soundReductionMultiplier)
    {
        #region Get Instigator
        SoundInstigator = instigator;

        SoundInstigator.TryGetComponent(out AIBrain EnemyInstigator);
        SoundInstigator.TryGetComponent(out CharacterCombat PlayerInstigator);

        if (PlayerInstigator != null)
        {
            _soundTypeOfInstigator = SoundInstigatorType.Player;
        }
        else if (EnemyInstigator != null)
        {
            _soundTypeOfInstigator = SoundInstigatorType.Enemy;
        }
        else
        {
            _soundTypeOfInstigator = SoundInstigatorType.Entity;
        }
        #endregion

        #region Get Sound Values and ScanEffect
        GlobalList = soundList;
        SoundProducedBySpeaker = soundSpeaker;
        SoundData = soundInfo;
        SoundBubbleType = SoundData.GetSoundType();

        _bubbleScanEffect = scanToTrack;
        _soundBubbleRadius = SoundData.Scanner_MaxDistance * soundReductionMultiplier;
        _soundEntityIntensity = 0f;
        #endregion

        #region Filter Own Sound Produced
        if (SoundInstigator.TryGetComponent(out SoundListener soundListener))
        {
            m_entitiesListenersRegistered.Add(soundListener);
        }
        #endregion

        #region Start Sound Bubble Expansion
        _soundPropagationCoroutine = StartCoroutine(SoundPropagation(scanToTrack));
        #endregion
    }

    IEnumerator SoundPropagation(ScannerEffect scanToTrack)
    {
        bool doIntensityDegradation = (SoundData.GetSoundType() != SoundInfo.SoundTypes.Silencer);
        Color degradationColor = Color.white;

        if (SoundData.GetSoundType() == SoundInfo.SoundTypes.EntitySound)
        {
            if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Standard)
            {
                degradationColor = GlobalList.entitySoundLowestIntensityColor;
            }
            else if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Medium)
            {
                degradationColor = GlobalList.entitySoundMediumLowestIntensityColor;
            }
            else
            {
                degradationColor = GlobalList.entitySoundSubtleLowestIntensityColor;
            }
        }
        else if (SoundData.GetSoundType() == SoundInfo.SoundTypes.HES)
        {
            if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Standard)
            {
                degradationColor = GlobalList.HESSoundLowestIntensityColor;
            }
            else if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Medium)
            {
                degradationColor = GlobalList.HESSoundMediumLowestIntensityColor;
            }
            else
            {
                degradationColor = GlobalList.HESSoundSubtleLowestIntensityColor;
            }
        }

        // Start Wait Delay
        yield return new WaitForSeconds(SoundData.Sound_StartDelay);
        _bubbleTrigger.radius = _bubbleScanEffect.ScanDistance;


        // Update Propagation Loop
        if (_bubbleTrigger.radius < _soundBubbleRadius)
        {
            _bubbleTrigger.enabled = true;
            while (_bubbleTrigger.radius < _soundBubbleRadius)
            {
                _bubbleTrigger.radius = _bubbleScanEffect.ScanDistance;

                if (doIntensityDegradation)
                {
                    float radiusCompletionPercentage = Mathf.InverseLerp(0f, _soundBubbleRadius, _bubbleTrigger.radius);
                    _soundEntityIntensity = Mathf.Lerp(SoundData.Sound_StartIntensity, SoundData.Sound_EndIntensity, radiusCompletionPercentage);
                    
                    if (SoundData.GetSoundType() == SoundInfo.SoundTypes.EntitySound)
                    {
                        Color intensityColor = Color.Lerp(GlobalList.scannerTierEntitySound.GetColor("_TrailColor"), degradationColor, radiusCompletionPercentage);
                        scanToTrack.scanEffect.SetColor("_TrailColor", intensityColor);
                    }
                    else if (SoundData.GetSoundType() == SoundInfo.SoundTypes.HES)
                    {
                        Color intensityColor = Color.Lerp(GlobalList.scannerTierHES.GetColor("_TrailColor"), degradationColor, radiusCompletionPercentage);
                        scanToTrack.scanEffect.SetColor("_TrailColor", intensityColor);
                    }
                }

                yield return null;
            }
        }


        // End Wait Delay
        yield return new WaitForSeconds(SoundData.Sound_EndDelay);
        this.enabled = false;
        

        // Break Coroutine Loop
        yield break;
    }
    


    void SoundOutlineChecker(GameObject outlineGameObject)
    {
        outlinesInEnemy.AddRange(outlineGameObject.GetComponents<OutlineAdder>());
        
        for (int i = 0; i < outlinesInEnemy.Count; i++)
        {
            outlinesInEnemy[i].ObjectPinged();
        }

        outlinesInEnemy.Clear();
    }

    void SoundTagChecker(ITageable taggedEntity)
    {
        taggedEntity.OnTag(_soundEntityIntensity);
    }


    public void OnTriggerEnter(Collider soundUserCollider)
    {
        #region Entity Sound
        switch (SoundBubbleType)
        {
            #region Sound Type Entity Sound
            case SoundInfo.SoundTypes.EntitySound:

                SoundOutlineChecker(soundUserCollider.gameObject);
                soundUserCollider.TryGetComponent(out ITageable tageableEntity);

                if (tageableEntity != null)
                {
                    SoundTagChecker(tageableEntity);
                }

                if (_soundTypeOfInstigator != SoundInstigatorType.Enemy)
                {
                    soundUserCollider.TryGetComponent(out AIHearing enemyHearing);

                    if (enemyHearing != null)
                    {
                        #region Check if Sound Listener is this Sound Speaker
                        bool breakCheckListenersLoop = false;
                        for (int i = 0; i < m_entitiesListenersRegistered.Count; i++)
                        {
                            if (enemyHearing == m_entitiesListenersRegistered[i])
                            {
                                breakCheckListenersLoop = true;
                                break;
                            }
                        }

                        if (!breakCheckListenersLoop)
                        {
                            enemyHearing.CheckSoundListened(SoundBubbleType, transform.position, _soundEntityIntensity);
                            m_entitiesListenersRegistered.Add(enemyHearing);
                        }
                        #endregion
                    }
                }

                break;
            #endregion

            #region Sound Type Silencer
            case SoundInfo.SoundTypes.Silencer:

                soundSpeakersDetected.AddRange(soundUserCollider.GetComponentsInChildren<SoundSpeaker>());
                soundUserCollider.TryGetComponent(out AIBrain aiBrainDetected);

                if (aiBrainDetected != null)
                {
                    if (aiBrainDetected is RangedEnemyBrain)
                    {
                        RangedEnemyBrain reb = aiBrainDetected as RangedEnemyBrain;
                        reb.RangedEnemy_ScanHES_EndTracking();
                    }
                    if (aiBrainDetected is PredatorEnemyBrain)
                    {
                        PredatorEnemyBrain peb = aiBrainDetected as PredatorEnemyBrain;
                        peb.PredatorEnemy_ScanHES_EndTracking();
                    }
                }

                bool breakSilencerCheckerLoop = false;
                for (int i = 0; i < soundSpeakersDetected.Count; i++)
                {
                    for (int j = 0; j < m_entitiesSpeakersRegistered.Count; j++)
                    {
                        if (soundSpeakersDetected[i] == m_entitiesSpeakersRegistered[j])
                        {
                            breakSilencerCheckerLoop = true;
                            break;
                        }
                    }

                    if (!breakSilencerCheckerLoop)
                    {
                        soundSpeakersDetected[i].silencersAffectingGO.Add(gameObject);
                        m_entitiesSpeakersRegistered.Add(soundSpeakersDetected[i]);

                        breakSilencerCheckerLoop = false;
                    }
                }
                soundSpeakersDetected.Clear();

                break;
            #endregion

            #region Sound Type HES
            case SoundInfo.SoundTypes.HES:

                SoundOutlineChecker(soundUserCollider.gameObject);

                if (_soundTypeOfInstigator == SoundInstigatorType.Enemy)
                {
                    SoundInstigator.TryGetComponent(out AIScanHES enemyScanHES);

                    if (enemyScanHES != null)
                    {
                        soundUserCollider.TryGetComponent(out CharacterCombat playerEntity);

                        if (playerEntity != null)
                        {
                            enemyScanHES.ScanHES_DetectedTarget();
                        }
                    }
                }

                break;
            #endregion
        }
        #endregion
    }

    public void OnTriggerExit(Collider soundUserCollider)
    {
        #region Remove Silencer Influence from Speaker
        if (SoundBubbleType == SoundInfo.SoundTypes.Silencer)
        {
            soundSpeakersDetected.AddRange(soundUserCollider.GetComponentsInChildren<SoundSpeaker>());

            for (int i = 0; i < soundSpeakersDetected.Count; i++)
            {
                if (soundSpeakersDetected[i].silencersAffectingGO.Contains(gameObject))
                {
                    soundSpeakersDetected[i].silencersAffectingGO.Remove(gameObject);
                    m_entitiesSpeakersRegistered.Remove(soundSpeakersDetected[i]);
                }
            }

            soundSpeakersDetected.Clear();
        }
        #endregion
    }
}