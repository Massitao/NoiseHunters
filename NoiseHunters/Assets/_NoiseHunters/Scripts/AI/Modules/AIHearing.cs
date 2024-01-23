using System;
using UnityEngine;

public class AIHearing : SoundListener
{
    [Header("Hearing States")]
    [SerializeField] protected bool _hearingActive;
    protected bool HearingActive
    {
        get { return _hearingActive; }
        set
        {
            if (_hearingActive != value)
            {
                _hearingActive = value;

                if (_hearingActive)
                {
                    OnHearingActivation?.Invoke();
                }
                else
                {
                    OnHearingDeactivation?.Invoke();
                }
            }
        }
    }

    [Header("Hearing Events")]
    public Action OnHearingActivation;
    public Action OnHearingDeactivation;


    public override void CheckSoundListened(SoundInfo.SoundTypes bubbleSoundType, Vector3 soundPosition, float entitySoundIntensity)
    {
        if (HearingActive)
        {
            base.CheckSoundListened(bubbleSoundType, soundPosition, entitySoundIntensity);
        }
    }

    public void Hearing_SetActive(bool stateToSet)
    {
        HearingActive = stateToSet;
    }
}