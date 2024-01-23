using System;
using UnityEngine;

public abstract class Interactuable : MonoBehaviour
{
    [Header("Interactuable Entity Activation")]
    [Tooltip("Is the Interactuable Entity enabled?")]
    [SerializeField] protected bool _isActive;
    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            if (value != _isActive)
            {
                _isActive = value;
                ActivationStatus();
            }
        }
    }


    [Tooltip("Should the Interactuable Entity be disabled after use?")]
    [SerializeField] protected bool _oneTimeUse;
    public bool OneTimeUse
    {
        get { return _oneTimeUse; }
        set { _oneTimeUse = value; }
    }

    public Action<bool> OnActiveChange;


    [Tooltip("Trigger needed to make the Interactuable Entities work.")]
    [SerializeField] protected Collider _interactuableTrigger;
    public Collider InteractuableTrigger
    {
        get { return _interactuableTrigger; }
        set { _interactuableTrigger = value; }
    }

    [Tooltip("Which colliders do you want to check?")]
    [SerializeField] protected LayerMask _interactuableViewMask;
    public LayerMask InteractuableViewMask
    {
        get { return _interactuableViewMask; }
        set { _interactuableViewMask = value; }
    }


    [Header("Interact HUD")]
    public Transform interactHUD_PosRot;

    protected GameObject interactHUD;
    protected Animator interactHUD_Animator;
    protected const string interactHUD_Prefab = "InteractHUD";
    protected const string interactHUD_Animator_BoolUserIsInside = "InsideTrigger";
    protected const string interactHUD_Animator_TriggerUserInteracted = "Interacted";



    protected virtual void Awake()
    {
        if (InteractuableTrigger == null)
        {
            TryGetComponent(out Collider thisTrigger);
            InteractuableTrigger = thisTrigger;
        }

        if (InteractuableTrigger == null) Debug.LogError($"Entity Trigger has not been set! {gameObject.name} won't work!", gameObject);

        interactHUD = Instantiate(Resources.Load(interactHUD_Prefab), interactHUD_PosRot) as GameObject;
        interactHUD_Animator = interactHUD.GetComponent<Animator>();

        ActivationStatus();
    }



    protected virtual void ActivationStatus()
    {
        InteractuableTrigger.enabled = IsActive;

        if (!IsActive)
        {
            if (interactHUD_Animator == null) { Debug.Log(gameObject, gameObject); }
            interactHUD_Animator.SetBool(interactHUD_Animator_BoolUserIsInside, false);
        }
    }

    public virtual bool CanInteract()
    {
        if (IsActive)
        {
            if (!OneTimeUse)
            {
                Interact();
                interactHUD_Animator.SetTrigger(interactHUD_Animator_TriggerUserInteracted);
                return false;
            }
            else
            {
                Interact();
                interactHUD_Animator.SetTrigger(interactHUD_Animator_TriggerUserInteracted);
                IsActive = false;
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    protected abstract void Interact();


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsActive && other.TryGetComponent(out InteractUser interactUser))
        {
            interactUser.InteractuableNear = this;
            interactHUD_Animator.SetBool(interactHUD_Animator_BoolUserIsInside, true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out InteractUser interactUser))
        {
            if (interactUser.InteractuableNear == this)
            {
                interactUser.InteractuableNear = null;
            }

            interactHUD_Animator.SetBool(interactHUD_Animator_BoolUserIsInside, false);
        }
    }
}