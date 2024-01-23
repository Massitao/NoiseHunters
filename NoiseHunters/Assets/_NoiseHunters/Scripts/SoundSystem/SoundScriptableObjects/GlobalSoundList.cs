using UnityEngine;

[CreateAssetMenu(fileName = "Sound List", menuName = "NoiseHunters/Sound List")]
public class GlobalSoundList : ScriptableObject
{
    [Header("Sound Bubble Prefab")]
    public GameObject SoundBubblePrefab;

    [Header("Sound Scanner Materials")]
    public Material scannerBase;

    [Space(10)]

    public Material scannerTierEntitySound;
    public Material scannerTierMediumEntitySound;
    public Material scannerTierSubtleEntitySound;

    [Space(5)]

    public Material scannerTierSilencer;
    public Material scannerTierMediumSilencer;
    public Material scannerTierSubtleSilencer;

    [Space(5)]

    public Material scannerTierHES;
    public Material scannerMediumTierHES;
    public Material scannerSubtleTierHES;


    [Header("Sound Color Intensity Values")]
    public Color entitySoundLowestIntensityColor;
    public Color entitySoundMediumLowestIntensityColor;
    public Color entitySoundSubtleLowestIntensityColor;

    [Space(5)]

    public Color HESSoundLowestIntensityColor;
    public Color HESSoundMediumLowestIntensityColor;
    public Color HESSoundSubtleLowestIntensityColor;


    [Header("Silencer Sound Values")]
    public float silencerSoundRadiusReductionPercentage = 0.1f;
}