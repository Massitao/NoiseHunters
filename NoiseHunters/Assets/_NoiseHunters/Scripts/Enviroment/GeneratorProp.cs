using System;
using UnityEngine;

public class GeneratorProp : MonoBehaviour
{
    [Header("Generator State")]
    [SerializeField] bool isActive;
    public bool IsActive
    {
        get { return isActive; }
        set
        {
            if (value != isActive)
            {
                isActive = value;
                OnActiveStateChange?.Invoke(this);
            }
        }
    }

    [SerializeField] private bool generatorNeedsShock;

    public Action<GeneratorProp> OnActiveStateChange;


    [Header("Generator Sound Info")]
    [SerializeField] private SoundInfo generatorSound;
    [SerializeField] private SoundInfo generatorSound_NeedShock;
    private SoundSpeaker generatorSpeaker;


    [Header("Generator Animator")]
    [SerializeField] private string animator_IsActiveBool;
    [SerializeField] private string animator_NeedsShockBool;
    [SerializeField] private string animator_ShockedTrigger;


    private Animator generatorAnimator;

    private void Awake()
    {
        generatorAnimator = GetComponent<Animator>();

        if (generatorSpeaker == null)
        {
            generatorSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }
        else
        {
            generatorSpeaker = GetComponentInChildren<SoundSpeaker>();
        }

        if (generatorNeedsShock)
        {
            isActive = false;
        }
    }

    private void OnEnable()
    {
        OnActiveStateChange += Generator_CheckState;
    }

    private void OnDisable()
    {
        OnActiveStateChange -= Generator_CheckState;
    }


    private void Start()
    {
        if (generatorNeedsShock)
        {
            isActive = false;
            generatorAnimator.SetBool(animator_NeedsShockBool, true);
        }
        else
        {
            if (IsActive)
            {
                Generator_CheckState(this);
            }
        }
    }


    void Generator_CheckState(GeneratorProp thisProp)
    {
        generatorAnimator?.SetBool(animator_IsActiveBool, IsActive);
    }


    public void Generator_AnimationEventPlaySound()
    {
        Generator_PlaySound();
    }

    void Generator_PlaySound()
    {
        generatorSpeaker.CreateSoundBubble(generatorSound, transform.position, gameObject, false);
    }


    public void ShockedToActive()
    {
        if (!IsActive)
        {
            generatorAnimator.SetBool(animator_NeedsShockBool, false);
            generatorAnimator.SetTrigger(animator_ShockedTrigger);

            GeneratorSetActive(true);
        }
    }


    public void GeneratorSetActive(bool active)
    {
        IsActive = active;
    }

    public void Generator_NeedShock_Sound()
    {
        generatorSpeaker.CreateSoundBubble(generatorSound_NeedShock, transform.position, gameObject, false);
    }
}