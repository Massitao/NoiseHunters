using System;
using UnityEngine;

public class PickableProp : Interactuable
{
    [Header("Pickable Components")]
    public GameObject pickableGameObject;
    public bool picked;

    [Header("Sound Components")]
    [SerializeField] private SoundInfo pickUpSound;
    private SoundSpeaker speaker;

    // EVENT
    public event Action<PickableProp> OnPickUpProp;


    protected override void Awake()
    {
        base.Awake();
        speaker = gameObject.AddComponent<SoundSpeaker>();
    }


    protected override void Interact()
    {
        PickedObject();
    }

    void PickedObject()
    {
        picked = true;

        if (pickUpSound != null)
        {
            speaker.CreateSoundBubble(pickUpSound, null, gameObject, true);
        }

        OnPickUpProp?.Invoke(this);
        _interactuableTrigger.enabled = false;
        pickableGameObject.SetActive(false);
    }
}
