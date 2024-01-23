using System;
using UnityEngine;

public class AIShock : MonoBehaviour
{
    [Header("Shock State")]
    [SerializeField] protected bool _canBeShocked = true;
    public bool CanBeShocked
    {
        get { return _canBeShocked; }
        set
        {
            if (_canBeShocked != value)
            {
                _canBeShocked = value;
            }
        }
    }

    [HideInInspector] public bool IsShocked;

    [Header("Shock Events")]
    public Action OnShocked;
    public Action OnShockedEnd;


    public void Shock_OnBeingElectrified()
    {
        IsShocked = true;
        OnShocked?.Invoke();
    }
    public void Shock_OnShockEnd()
    {
        IsShocked = false;
        OnShockedEnd?.Invoke();
    }
}
