using System;
using UnityEngine;

public class AISuspect : MonoBehaviour
{
    [Header("Suspicion States")]
    public SuspectStatus currentSuspectStatus;
    public enum SuspectStatus { None, Alerted };


    [Header("AI Alert")]
    [Range(0, 1)] [SerializeField] protected float _alertLevel;
    [HideInInspector] public float AlertLevel
    {
        get { return _alertLevel; }
        set
        {
            if (_alertLevel != value)
            {
                _alertLevel = value;

                OnSuspicionLevelChange?.Invoke(_alertLevel);
            }
        }
    }


    [Header("Suspicion Events")]
    public Action<float> OnSuspicionLevelChange;

    public Action OnAlertTriggered;
    public Action OnAlertEnd;



    protected virtual void OnEnable()
    {
        // Suscribe to Events
        OnSuspicionLevelChange += Suspect_CheckSuspectLevel;
        OnAlertTriggered += Suspect_AlertExceeded;
        OnAlertEnd += Suspect_AlertEnd;
    }
    protected virtual void OnDisable()
    {
        // Unsuscribe from Events
        OnAlertTriggered -= Suspect_AlertExceeded;
        OnAlertEnd -= Suspect_AlertEnd;
    }


    protected void Suspect_CheckSuspectLevel(float alert)
    {
        switch (currentSuspectStatus)
        {
            case SuspectStatus.None:

                if (alert >= 1)
                {
                    OnAlertTriggered?.Invoke();
                }

                break;


            case SuspectStatus.Alerted:

                if (alert <= 0)
                {
                    OnAlertEnd?.Invoke();
                }
                        
                break;
        }
    }


    #region SUSPICION TOOLS
    public void Suspect_SetAlertLevel(bool additive, float levelToSet)
    {
        float alertLevelToSet = Mathf.Abs(levelToSet);

        AlertLevel = (additive) ? Mathf.Clamp(AlertLevel + alertLevelToSet, 0, 1) : Mathf.Clamp(alertLevelToSet, 0, 1);  //clampeo entre valores 0 y 1 de alerta
    }

    public void Suspect_AlertExceeded()
    {
        currentSuspectStatus = SuspectStatus.Alerted;
    }
    public void Suspect_AlertEnd()
    {
            currentSuspectStatus = SuspectStatus.None;
    }

    public void Suspect_ResetSuspicion()
    {
        currentSuspectStatus = SuspectStatus.None;

        AlertLevel = 0f;
    }
    #endregion
}
