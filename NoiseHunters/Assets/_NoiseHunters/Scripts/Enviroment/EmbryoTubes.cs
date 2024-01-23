using System.Collections;
using UnityEngine;

public class EmbryoTubes : MonoBehaviour
{
    [Header("Embryo Components")]
    [SerializeField] private MeshRenderer embryoLiquidMeshRenderer;
    private ThirdPersonController thirdPersonController;
    private SoundSpeaker embryoSpeaker;
    private Animator embryoAnimator;


    [Header("Embryo Proximity Values")]
    [SerializeField] private Transform embryoTransform;
    [SerializeField] private float embryoDistanceFromPlayer;

    private bool playerIsNear;


    [Header("Embryo Animation Values")]
    [SerializeField] private string embryoStartAlertTrigger;
    [SerializeField] private string embryoEndAlertTrigger;


    [Header("Embryo Material Values")]
    [SerializeField] private Vector2 embryoRandomIdleMatChangeTime;
    private float embryoIdleMatChangeTime;
    [SerializeField] private float embryoAlertedMatChangeTime;

    [SerializeField] private AnimationCurve embryoIdleMatChangeAnimationCurve;
    [SerializeField] private AnimationCurve embryoAlertedMatChangeAnimationCurve;

    private Color embryoInitialHeartColor;
    private float embryoCurrentEmissiveIntensity;


    [Header("Embryo Sound Values")]
    [SerializeField] private bool fixedFrequency;

    [SerializeField] private float embryoFixedInterval;
    [SerializeField] private Vector2 embryoRandomInterval;

    [SerializeField] private SoundInfo embryoSound;
    [SerializeField] private SoundInfo embryoSound_Medium;
    [SerializeField] private SoundInfo embryoSound_Subtle;

    private Coroutine embryoCoroutine;


    private void Awake()
    {
        thirdPersonController = FindObjectOfType<ThirdPersonController>();

        if (TryGetComponent(out SoundSpeaker speaker))
        {
            embryoSpeaker = speaker;
        }
        else
        {
            embryoSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }

        embryoAnimator = GetComponent<Animator>();

        if (embryoLiquidMeshRenderer != null)
        {
            embryoInitialHeartColor = embryoLiquidMeshRenderer.material.GetColor("_EmissionColor");
            embryoCurrentEmissiveIntensity = 1f;

            embryoIdleMatChangeTime = Random.Range(embryoRandomIdleMatChangeTime.x, embryoRandomIdleMatChangeTime.y);

            StartCoroutine(EmbryoMaterialChange());
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckProximity();
    }
    void CheckProximity()
    {
        Vector3 posToGenerate = (embryoTransform != null) ? embryoTransform.position : transform.position;

        if (Vector3.Distance(posToGenerate, thirdPersonController.transform.position) <= embryoDistanceFromPlayer)
        {
            StartHeartbeat();
        }
        else
        {
            EndHeartBeat();
        }
    }

    // Embryo Heart Behaviour
    void StartHeartbeat()
    {
        if (embryoCoroutine == null)
        {
            playerIsNear = true;
            embryoAnimator?.SetTrigger(embryoStartAlertTrigger);
            embryoCoroutine = StartCoroutine(HeartbeatBehaviour());
        }
    }
    IEnumerator HeartbeatBehaviour()
    {
        float interval = (fixedFrequency) ? embryoFixedInterval: Random.Range(embryoRandomInterval.x, embryoRandomInterval.y);
        SoundInfo embryoSoundToPlay = embryoSound;

        while (true)
        {
            yield return new WaitForSeconds(interval);

            Vector3 posToGenerate = (embryoTransform != null) ? embryoTransform.position : transform.position;
            if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Standard)
            {
                embryoSoundToPlay = embryoSound;
            }
            else if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Medium)
            {
                embryoSoundToPlay = embryoSound_Medium;
            }
            else if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Subtle)
            {
                embryoSoundToPlay = embryoSound_Subtle;
            }

            embryoSpeaker.CreateSoundBubble(embryoSoundToPlay, posToGenerate, gameObject, false);

            interval = (fixedFrequency) ? embryoFixedInterval : Random.Range(embryoRandomInterval.x, embryoRandomInterval.y);
        }
    }
    void EndHeartBeat()
    {
        if (embryoCoroutine != null)
        {
            playerIsNear = false;
            embryoAnimator?.SetTrigger(embryoEndAlertTrigger);
            StopCoroutine(embryoCoroutine);
            embryoCoroutine = null;
        }
    }

    // Material Behaviour
    IEnumerator EmbryoMaterialChange()
    {
        float lerpPercentage = 0f;
        Color currentEmbryoColor = embryoInitialHeartColor;

        while (true)
        {
            if (playerIsNear)
            {
                currentEmbryoColor = embryoLiquidMeshRenderer.material.GetColor("_EmissionColor");

                for (float timeToReachLerpTime = embryoAlertedMatChangeTime; timeToReachLerpTime > 0f; timeToReachLerpTime -= Time.deltaTime)
                {
                    lerpPercentage = Mathf.InverseLerp(embryoAlertedMatChangeTime, 0f, timeToReachLerpTime);
                    embryoCurrentEmissiveIntensity = embryoAlertedMatChangeAnimationCurve.Evaluate(lerpPercentage);

                    embryoLiquidMeshRenderer.material.SetColor("_EmissionColor", Color.Lerp(currentEmbryoColor, embryoInitialHeartColor * embryoCurrentEmissiveIntensity, lerpPercentage));

                    if (!playerIsNear) break;

                    yield return null;
                }
            }
            else
            {
                currentEmbryoColor = embryoLiquidMeshRenderer.material.GetColor("_EmissionColor");

                for (float timeToReachLerpTime = embryoIdleMatChangeTime; timeToReachLerpTime > 0f; timeToReachLerpTime -= Time.deltaTime)
                {
                    lerpPercentage = Mathf.InverseLerp(embryoIdleMatChangeTime, 0f, timeToReachLerpTime);
                    embryoCurrentEmissiveIntensity = embryoIdleMatChangeAnimationCurve.Evaluate(lerpPercentage);

                    embryoLiquidMeshRenderer.material.SetColor("_EmissionColor", Color.Lerp(currentEmbryoColor, embryoInitialHeartColor * embryoCurrentEmissiveIntensity, lerpPercentage));

                    if (playerIsNear) break;

                    yield return null;
                }
            }
        }
    }
}