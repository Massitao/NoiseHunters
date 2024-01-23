using System;
using UnityEngine;

public class AIScanHES : MonoBehaviour
{
    [Header("ScanHES States")]
    [SerializeField] protected bool _detectScanActive;
    public bool ScanDetectionActive
    {
        get { return _detectScanActive; }
        set
        {
            if (_detectScanActive != value)
            {
                _detectScanActive = value;
            }
        }
    }


    [Header("ScanHES Events")]
    public Action OnScanEmit;
    public Action OnDetectedEntity;



    public void ScanHES_DetectionSetActive(bool stateToSet)
    {
        _detectScanActive = stateToSet;
    }


    public void ScanHES_EmitScan(SoundSpeaker soundSpeaker, SoundInfo scanInfoHES)
    {
        if (!ScanDetectionActive || soundSpeaker == null || scanInfoHES == null || scanInfoHES.GetSoundType() != SoundInfo.SoundTypes.HES)
        {
            return;
        }

        soundSpeaker.CreateSoundBubble(scanInfoHES, transform.position, gameObject, false);
        OnScanEmit?.Invoke();
    }

    public void ScanHES_DetectedTarget()
    {
        if (ScanDetectionActive)
        {
            OnDetectedEntity?.Invoke();
        }
    }
}
