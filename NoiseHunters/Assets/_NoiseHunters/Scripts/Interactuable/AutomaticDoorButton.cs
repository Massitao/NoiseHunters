using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutomaticDoorButton : Interactuable
{
    [Header("Button Properties")]
    private SoundSpeaker soundSpeaker;
    private AutomaticDoor automaticDoor;

    [Header("Button State")]
    public bool IsOn;

    [Header("Sounds")]
    [SerializeField] private Transform soundSpawn;

    [SerializeField] private SoundInfo buttonAccessSound;
    [SerializeField] private SoundInfo buttonDeniedSound;

    [Header("Events")]
    [SerializeField] private UnityEvent OnTrue;
    [SerializeField] private UnityEvent OnFalse;



    protected override void Awake()
    {
        base.Awake();

        InteractuableTrigger.enabled = true;

        automaticDoor = GetComponentInParent<AutomaticDoor>();

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
        IsOn = (IsOn) ? false : true;
        ButtonAction();
    }

    void ButtonAction()
    {
        if (IsOn) OnTrue.Invoke();
        else OnFalse.Invoke();
    }

    public override bool CanInteract()
    {
        if (IsActive)
        {
            if (!automaticDoor.IsLocked)
            {
                if (!OneTimeUse)
                {
                    Interact();
                    interactHUD_Animator.SetTrigger(interactHUD_Animator_TriggerUserInteracted);

                    if (buttonAccessSound != null)
                    {
                        soundSpeaker.CreateSoundBubble(buttonAccessSound, soundSpawn.position, gameObject, true);
                    }

                    return false;
                }
                else
                {
                    Interact();
                    interactHUD_Animator.SetTrigger(interactHUD_Animator_TriggerUserInteracted);

                    if (buttonAccessSound != null)
                    {
                        soundSpeaker.CreateSoundBubble(buttonAccessSound, soundSpawn.position, gameObject, true);
                    }

                    IsActive = false;
                    return true;
                }
            }
            else
            {
                if (buttonDeniedSound != null)
                {
                    soundSpeaker.CreateSoundBubble(buttonDeniedSound, soundSpawn.position, gameObject, true);
                }

                return false;
            }
        }
        else
        {
            return false;
        }
    }
}