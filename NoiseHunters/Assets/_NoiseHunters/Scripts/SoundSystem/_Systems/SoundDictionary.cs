using System.Collections.Generic;
using UnityEngine;

public class SoundDictionary : MonoBehaviour
{
    [SerializeField] public static Dictionary<string, SoundInfo> Sound_Dictionary = new Dictionary<string, SoundInfo>();
    private static GlobalSoundList globalSoundList;
    private const string soundListData = "Global Sound List";


    private static void SetGlobalSoundList()
    {
        globalSoundList = Resources.Load(soundListData) as GlobalSoundList;
    }



    public static void CreateOnlyAudioSource(MonoBehaviour scriptCalling, AudioClip clipToPlay, Vector3 soundPosition)
    {
        if (globalSoundList == null)
        {
            SetGlobalSoundList();
        }

        SetAudioClip(scriptCalling, clipToPlay, soundPosition);
    }
    public static void CreateOnlyAudioSource(MonoBehaviour scriptCalling, SoundInfo soundToPlay, Vector3 soundPosition)
    {
        if (globalSoundList == null)
        {
            SetGlobalSoundList();
        }

        SetAudioClip(scriptCalling, soundToPlay, soundPosition);
    }

    public static void CreateSoundBubble(MonoBehaviour scriptCalling, SoundInfo soundToPlay, Vector3 soundPosition, SoundSpeaker soundSpeaker, GameObject instigator, bool onlyAudio)
    {
        if (globalSoundList == null)
        {
            SetGlobalSoundList();
        }

        if (soundToPlay != null)
        {
            if (!onlyAudio)
            {
                SetSoundBubble(soundToPlay, soundPosition, soundSpeaker, instigator);
            }

            SetAudioClip(scriptCalling, soundToPlay, soundPosition);
        }
    }



    private static void SetSoundBubble(SoundInfo SoundToPlay, Vector3 soundPosition, SoundSpeaker soundSpeaker, GameObject instigator)
    {
        ScannerEffect scannerEffect = soundSpeaker.ScannerCheckAvailability();
        float soundLengthMultiplier = (soundSpeaker.UnderSilencerInfluence && SoundToPlay.GetSoundType() != SoundInfo.SoundTypes.Silencer) ? globalSoundList.silencerSoundRadiusReductionPercentage : 1f;

        Material scanMatToSet = ScanMaterialToSet(SoundToPlay, soundSpeaker, soundLengthMultiplier);

        scannerEffect.SetScannerSettings(soundPosition, scanMatToSet);
        scannerEffect.StartScan();

        GameObject go = Instantiate(globalSoundList.SoundBubblePrefab, soundPosition, Quaternion.identity, null);
        go.name = $"{SoundToPlay.name} Sound Bubble ({SoundToPlay.GetSoundType()})";
        go.GetComponent<SoundBubbleLogic>().SetBubbleLogic(globalSoundList, soundSpeaker, SoundToPlay, scannerEffect, instigator, soundLengthMultiplier);
    }

    private static void SetAudioClip(MonoBehaviour scriptCalling, AudioClip clipToPlay, Vector3 soundPosition)
    {
        if (clipToPlay != null)
        {
            GameObject soundGO = new GameObject();
            soundGO.transform.position = soundPosition;
            soundGO.name = $"AudioSource: {clipToPlay.name}";

            AudioSource soundAudioSource = soundGO.AddComponent<AudioSource>();
            soundAudioSource.clip = clipToPlay;

            AMLink link = soundGO.AddComponent<AMLink>();
            link.audioSource = soundAudioSource;

            soundAudioSource.Play();

            DelayAction.DoDelayedActionUnscaled(scriptCalling, () => { Destroy(soundGO); }, soundAudioSource.clip.length);
        }
    }
    private static void SetAudioClip(MonoBehaviour scriptCalling, SoundInfo soundInfo, Vector3 soundPosition)
    {
        AudioClip clipToPlay = soundInfo.SelectAudioClip();

        if (clipToPlay != null)
        {
            GameObject soundGO = new GameObject();
            soundGO.transform.position = soundPosition;
            soundGO.name = $"AudioSource: {clipToPlay.name}";

            AudioSource soundAudioSource = soundGO.AddComponent<AudioSource>();
            soundAudioSource.clip = clipToPlay;
            soundAudioSource.loop = soundInfo.Sound_IsLooped;
            soundAudioSource.spatialBlend = (soundInfo.Sound_IsAudioSource3D) ? 1f : 0f;
            soundAudioSource.pitch = Random.Range(soundInfo.Sound_AudioSourcePitchRange.x, soundInfo.Sound_AudioSourcePitchRange.y);
            soundAudioSource.volume = Random.Range(soundInfo.Sound_AudioSourceVolumeRange.x, soundInfo.Sound_AudioSourceVolumeRange.y);
            soundAudioSource.minDistance = soundInfo.Sound_AudioSourceRange.x;
            soundAudioSource.maxDistance = soundInfo.Sound_AudioSourceRange.y;

            soundInfo.SetCustomAudioCurves(soundAudioSource);

            AMLink link = soundGO.AddComponent<AMLink>();
            link.audioSource = soundAudioSource;

            soundAudioSource.Play();

            Destroy(soundGO, soundAudioSource.clip.length);
        }
    }



    private static Material GetTierMaterial(SoundInfo soundInfo)
    {
        switch (soundInfo.GetSoundType())
        {
            case SoundInfo.SoundTypes.EntitySound:
                switch (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness)
                {
                    case BrightnessEnum.Standard:
                        return globalSoundList.scannerTierEntitySound;

                    case BrightnessEnum.Medium:
                        return globalSoundList.scannerTierMediumEntitySound;

                    case BrightnessEnum.Subtle:
                        return globalSoundList.scannerTierSubtleEntitySound;
                }
                break;

            case SoundInfo.SoundTypes.Silencer:
                switch (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness)
                {
                    case BrightnessEnum.Standard:
                        return globalSoundList.scannerTierSilencer;

                    case BrightnessEnum.Medium:
                        return globalSoundList.scannerTierMediumSilencer;

                    case BrightnessEnum.Subtle:
                        return globalSoundList.scannerTierSubtleSilencer;
                }
                break;

            case SoundInfo.SoundTypes.HES:
                switch (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness)
                {
                    case BrightnessEnum.Standard:
                        return globalSoundList.scannerTierHES;

                    case BrightnessEnum.Medium:
                        return globalSoundList.scannerMediumTierHES;

                    case BrightnessEnum.Subtle:
                        return globalSoundList.scannerSubtleTierHES;
                }
                break;
        }
        return globalSoundList.scannerBase;
    }

    private static Material ScanMaterialToSet(SoundInfo soundInfo, SoundSpeaker soundSpeaker, float soundLengthMultiplier)
    {
        Material scanMatToSet = new Material(GetTierMaterial(soundInfo));
        bool scanMaterialInstanceProvided = (soundInfo.Sound_ScanMaterial != null);


        scanMatToSet.SetFloat("_ScanDistance", 0);

        scanMatToSet.SetFloat("_ScanSpeed", soundInfo.Scanner_Speed);
        scanMatToSet.SetFloat("_ScanMaxDistance", soundInfo.Scanner_MaxDistance * soundLengthMultiplier);
        scanMatToSet.SetFloat("_ScanStopOffset", soundInfo.Scanner_DistanceStopOffset * soundLengthMultiplier);
        scanMatToSet.SetFloat("_ScanWidth", soundInfo.Scanner_WaveWidth);
        scanMatToSet.SetFloat("_LeadSharp", soundInfo.Scanner_LeadingEdgeWaveWidth);
        scanMatToSet.SetFloat("_Repetitions", soundInfo.Scanner_WaveRepetitions);


        if (scanMaterialInstanceProvided)
        {
            // Set Horizontal Bars Color
            if (soundInfo.Sound_ScanMaterial.Scanner_EnableHorizontalBars)
            {
                scanMatToSet.SetFloat("_HBarEnabled", 1f);
            }

            // Set Texture if is not equal to null and Horizontal Bars are not enabled
            else
            {
                if (soundInfo.Sound_ScanMaterial.Scanner_Texture != null)
                {
                    scanMatToSet.SetTexture("_DetailTex", soundInfo.Sound_ScanMaterial.Scanner_Texture);
                }

                scanMatToSet.SetFloat("_HBarEnabled", 0f);
            }

            // Set Custom Colors if enabled
            if (soundInfo.Sound_ScanMaterial.Scanner_CustomSoundColors)
            {
                scanMatToSet.SetColor("_LeadColor", soundInfo.Sound_ScanMaterial.Scanner_LeadingEdgeColor);
                scanMatToSet.SetColor("_MidColor", soundInfo.Sound_ScanMaterial.Scanner_MidColor);
                scanMatToSet.SetColor("_TrailColor", soundInfo.Sound_ScanMaterial.Scanner_TrailColor);
            }

            if (soundInfo.Sound_ScanMaterial.Scanner_CustomTextureColor)
            {
                scanMatToSet.SetColor("_HBarColor", soundInfo.Sound_ScanMaterial.Scanner_TextureColor);
            }
        }

        return scanMatToSet;
    }
}