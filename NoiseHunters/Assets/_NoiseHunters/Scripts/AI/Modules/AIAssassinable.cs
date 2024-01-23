using System;
using UnityEngine;

public class AIAssassinable : MonoBehaviour
{
    [Header("Assassination State")]
    [SerializeField] protected bool _canBeAssassinated = true;
    public bool CanBeAssassinated
    {
        get { return _canBeAssassinated; }
        set
        {
            if (_canBeAssassinated != value)
            {
                _canBeAssassinated = value;
            }
        }
    }


    [Header("Assassination Events")]
    public Action OnBeingAssassinated;
    public Action OnAssassinated;



    public void Assassination_BeingAttacked()
    {
        OnBeingAssassinated?.Invoke();
    }
    public void Assassination_Killed()
    {
        CanBeAssassinated = false;
        OnAssassinated?.Invoke();
    }
}
