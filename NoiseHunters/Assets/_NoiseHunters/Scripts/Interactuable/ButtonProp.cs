using UnityEngine;
using UnityEngine.Events;

public class ButtonProp : Interactuable
{
    [Header("Button Properties")]
    public bool IsOn;

    public Transform soundSpawn;

    public SoundInfo buttonPressSound;

    [SerializeField] UnityEvent OnTrue;
    [SerializeField] UnityEvent OnFalse;

    SoundSpeaker soundSpeaker;


    protected override void Awake()
    {
        base.Awake();

        InteractuableTrigger.enabled = true;

        if (TryGetComponent(out SoundSpeaker thisSpeaker))
        {
            soundSpeaker = thisSpeaker;
        }
        else
        {
            soundSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }
    }


    protected override void Interact()
    {
        IsOn = (IsOn) ? false : true ;
        ButtonAction();
    }

    public override bool CanInteract()
    {
        if (IsActive)
        {
            if (!OneTimeUse)
            {
                Interact();
                interactHUD_Animator.SetTrigger(interactHUD_Animator_TriggerUserInteracted);
                soundSpeaker.CreateSoundBubble(buttonPressSound, soundSpawn.position, gameObject, true);
                return false;
            }
            else
            {
                Interact();
                interactHUD_Animator.SetTrigger(interactHUD_Animator_TriggerUserInteracted);
                soundSpeaker.CreateSoundBubble(buttonPressSound, soundSpawn.position, gameObject, true);
                IsActive = false;
                return true;
            }
        }
        else
        {
            return false;
        }
    }


    void ButtonAction()
    {
        if (IsOn) OnTrue.Invoke();
        else OnFalse.Invoke();
    }
}