using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Info", menuName = "NoiseHunters/Sound Info")]
public class SoundInfo : ScriptableObject
{
    [Header("Sound AudioClip")]
    [SerializeField] private List<AudioClip> m_soundClips;
    public List<AudioClip> Sound_AudioSourceClips
    {
        get { return m_soundClips; }
    }

    private AudioClip m_previouslyPlayedSound;

    public AudioClip SelectAudioClip()
    {
        if (Sound_AudioSourceClips.Count == 0)
        {
            return null;
        }

        if (Sound_AudioSourceClips.Count == 1)
        {
            return Sound_AudioSourceClips[0];
        }
        else
        {
            List<AudioClip> audioClipsToSelect = new List<AudioClip>(Sound_AudioSourceClips);

            if (m_previouslyPlayedSound != null && !Sound_CanAudioSourceRepeatClips)
            {
                audioClipsToSelect.Remove(m_previouslyPlayedSound);
            }

            m_previouslyPlayedSound = audioClipsToSelect[Random.Range(0, audioClipsToSelect.Count)];
            return m_previouslyPlayedSound;
        }
    }



    [Header("Sound Values")]
    [SerializeField] private bool m_canAudioSourceRepeatClips;
    public bool Sound_CanAudioSourceRepeatClips
    {
        get { return m_canAudioSourceRepeatClips; }
    }

    [SerializeField] private bool m_isAudioSourceLooped;
    public bool Sound_IsLooped
    {
        get { return m_isAudioSourceLooped; }
    }

    [SerializeField] private bool m_isAudio3D;
    public bool Sound_IsAudioSource3D
    {
        get { return m_isAudio3D; }
    }

    [SerializeField] private Vector2 m_soundPitchRange;
    public Vector2 Sound_AudioSourcePitchRange
    {
        get
        {
            m_soundPitchRange.x = Mathf.Clamp(m_soundPitchRange.x, -3, 3);
            m_soundPitchRange.y = Mathf.Clamp(m_soundPitchRange.y, -3, 3);
            return m_soundPitchRange;
        }
    }

    [SerializeField] private Vector2 m_soundVolumeRange;
    public Vector2 Sound_AudioSourceVolumeRange
    {
        get
        {
            m_soundVolumeRange.x = Mathf.Clamp(m_soundVolumeRange.x, 0, 1);
            m_soundVolumeRange.y = Mathf.Clamp(m_soundVolumeRange.y, 0, 1);
            return m_soundVolumeRange;
        }
    }

    [SerializeField] private Vector2 m_soundRange = new Vector2(1f, 500f);
    public Vector2 Sound_AudioSourceRange
    {
        get
        {
            return m_soundRange;
        }
    }



    [Header("Sound AudioClip Rolloff Curve")]
    [SerializeField] private AnimationCurve m_soundRolloffCurve;
    public AnimationCurve Sound_RolloffCurve
    {
        get
        {
            return m_soundRolloffCurve;
        }
    }
    [SerializeField] private AnimationCurve m_soundSpatialBlendCurve;
    public AnimationCurve Sound_SpatialBlendCurve
    {
        get
        {
            return m_soundSpatialBlendCurve;
        }
    }
    [SerializeField] private AnimationCurve m_soundSpreadCurve;
    public AnimationCurve Sound_SpreadCurve
    {
        get
        {
            return m_soundSpreadCurve;
        }
    }
    [SerializeField] private AnimationCurve m_soundReverbMixCurve;
    public AnimationCurve Sound_ReverbMixCurve
    {
        get
        {
            return m_soundReverbMixCurve;
        }
    }


    public void SetCustomAudioCurves(AudioSource sourceToChangeCurves)
    {
        if (Sound_RolloffCurve != null && Sound_RolloffCurve.keys.Length > 1)
        {
            sourceToChangeCurves.rolloffMode = AudioRolloffMode.Custom;
            sourceToChangeCurves.SetCustomCurve(AudioSourceCurveType.CustomRolloff, Sound_RolloffCurve);
        }
        if (Sound_SpatialBlendCurve != null && Sound_SpatialBlendCurve.keys.Length > 0)
        {
            sourceToChangeCurves.SetCustomCurve(AudioSourceCurveType.SpatialBlend, Sound_SpatialBlendCurve);
        }
        if (Sound_SpreadCurve != null && Sound_SpreadCurve.keys.Length > 0)
        {
            sourceToChangeCurves.SetCustomCurve(AudioSourceCurveType.Spread, Sound_SpreadCurve);
        }
        if (Sound_ReverbMixCurve != null && Sound_ReverbMixCurve.keys.Length > 0)
        {
            sourceToChangeCurves.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, Sound_ReverbMixCurve);
        }
    }
    [ContextMenu("Reset Curves")] private void ResetAudioSourceCurves()
    {
        m_soundRolloffCurve.keys = new Keyframe[0];
        m_soundSpatialBlendCurve.keys = new Keyframe[0];
        m_soundSpreadCurve.keys = new Keyframe[0];
        m_soundReverbMixCurve.keys = new Keyframe[0];

        Debug.Log("Audio Curves have been reseted. Actual curves are empty and you are free to start over again");
    }



    [Header("Sound Tier")]
    [SerializeField] private SoundTypes m_soundTypes;
    public enum SoundTypes { EntitySound, Silencer, HES };   

    public SoundTypes SetSoundType(SoundTypes soundTypeToSet) => m_soundTypes = soundTypeToSet;
    public SoundTypes GetSoundType()
    {
        return m_soundTypes;
    }



    [Header("Sound Values")]
    [Range(0f, 1f)] [SerializeField] private float m_soundStartingIntensity;
    public float Sound_StartIntensity
    {
        get { return m_soundStartingIntensity; }
    }

    [Range(0f, 1f)] [SerializeField] private float m_soundEndingIntensity;
    public float Sound_EndIntensity
    {
        get { return m_soundEndingIntensity; }
    }


    [SerializeField] private float m_soundStartDelay;
    public float Sound_StartDelay
    {
        get { return m_soundStartDelay; }
    }

    [SerializeField] private float m_soundEndDelay;
    public float Sound_EndDelay
    {
        get { return m_soundEndDelay; }
    }



    [Header("Scan Effect")]
    [SerializeField] private ScanMaterialInstance m_soundScanMaterial;
    public ScanMaterialInstance Sound_ScanMaterial
    {
        get { return m_soundScanMaterial; }
    }


    [Header("Scan Values")]
    [SerializeField] private float m_scanSpeed;
    public float Scanner_Speed
    {
        get { return m_scanSpeed; }
    }

    [SerializeField] private float m_scanMaxDistance;
    public float Scanner_MaxDistance
    {
        get { return m_scanMaxDistance; }
    }

    [SerializeField] private float m_scanDistanceStopOffset;
    public float Scanner_DistanceStopOffset
    {
        get { return m_scanDistanceStopOffset; }
    }

    [SerializeField] private float m_scanWaveWidth;
    public float Scanner_WaveWidth
    {
        get { return m_scanWaveWidth; }
    }

    [SerializeField] private float m_scanLeadingEdgeWidth;
    public float Scanner_LeadingEdgeWaveWidth
    {
        get { return m_scanLeadingEdgeWidth; }
    }

    [SerializeField] private float m_scanWaveRepetitions;
    public float Scanner_WaveRepetitions
    {
        get { return m_scanWaveRepetitions; }
    }

}