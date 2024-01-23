using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbryoTubesDefective : MonoBehaviour
{
    [Header("Embryo Components")]
    [SerializeField] private MeshRenderer embryoLiquidMeshRenderer;
    private ThirdPersonController thirdPersonController;
    private SoundSpeaker embryoSpeaker;


    [Header("Embryo Proximity Values")]
    [SerializeField] private Transform embryoTransform;
    [SerializeField] private float embryoDistanceFromPlayer;

    private bool playerIsNear;


    [Header("Embryo Material Values")]
    [SerializeField] private Vector2 embryoRandomIdleMatChangeTime;
    private float embryoIdleMatChangeTime;
    [SerializeField] private float embryoAlertedMatChangeTime;

    [SerializeField] private AnimationCurve embryoIdleMatChangeAnimationCurve;
    [SerializeField] private AnimationCurve embryoAlertedMatChangeAnimationCurve;

    private Color embryoInitialHeartColor;
    private float embryoCurrentEmissiveIntensity;


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

        playerIsNear = (Vector3.Distance(posToGenerate, thirdPersonController.transform.position) <= embryoDistanceFromPlayer);
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
