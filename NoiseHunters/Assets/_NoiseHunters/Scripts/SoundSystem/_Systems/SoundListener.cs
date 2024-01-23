using System;
using UnityEngine;

public class SoundListener : MonoBehaviour
{
    [Header("Hearing Type")]
    public SoundInfo.SoundTypes SoundListenerType;

    [Header("Last Heard Sound Position")]
    [HideInInspector] public Vector3 listenedSoundPosition;
    [HideInInspector] public float listenedSoundIntensity;

    [Header("Hearing Event")]
    public Action OnSoundListened;


    // Checks Sound Bubble Trigger and checks if this entity is capable to hear it.
    public virtual void CheckSoundListened (SoundInfo.SoundTypes bubbleSoundType, Vector3 soundPosition, float entitySoundIntensity)
    {
        bool canListen = false;

        switch (SoundListenerType)
        {
            case SoundInfo.SoundTypes.EntitySound:
                if (bubbleSoundType == SoundInfo.SoundTypes.EntitySound)
                {
                    listenedSoundIntensity = entitySoundIntensity;
                    canListen = true;
                }

                break;
        }

        if (canListen)
        {
            SoundTypeListened(soundPosition);
        }
    }

    // Entity has heard a sound. Update the Last Heard Position and trigger a "SoundListened" event.
    protected virtual void SoundTypeListened(Vector3 soundPosition)
    {
        listenedSoundPosition = soundPosition;
        OnSoundListened?.Invoke();
    }
}
