using System;
using System.Collections;
using UnityEngine;

public class SoundGenerator : MonoBehaviour
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
                OnActiveStateChange?.Invoke();
            }
        }
    }

    public Action OnActiveStateChange;


    [Header("Generator Sound Info")]
    [SerializeField] private SoundInfo generatorSound;
    private SoundSpeaker generatorSpeaker;


    [Header("Sound Generation")]
    [SerializeField] private Transform soundGenerationTransform;


    [Header("Generation by Code")]
    [SerializeField] private float generatorSpawnWaveInterval;
    private Coroutine generatorBehaviour;


    private void OnEnable()
    {
        OnActiveStateChange += CheckState;
    }

    private void OnDisable()
    {
        OnActiveStateChange -= CheckState;
    }


    private void Start()
    {
        if (generatorSpeaker == null)
        {
            generatorSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }
        else
        {
            generatorSpeaker = GetComponentInChildren<SoundSpeaker>();
        }

        OnActiveStateChange?.Invoke();
    }


    void CheckState()
    {
        if (IsActive)
        {
            StartGenerator();
        }
        else
        {
            EndGenerator();
        }
    }

    void StartGenerator()
    {
        if (generatorBehaviour != null)
        {
            StopCoroutine(generatorBehaviour);    
        }

        generatorBehaviour = StartCoroutine(GeneratorBehaviour());
    }

    IEnumerator GeneratorBehaviour()
    {
        while (true)
        {
            Generator_PlaySound();

            yield return new WaitForSeconds(generatorSpawnWaveInterval);
        }
    }

    void EndGenerator()
    {
        if (generatorBehaviour != null)
        {
            StopCoroutine(generatorBehaviour);
            generatorBehaviour = null;
        }
    }


    void Generator_PlaySound()
    {
        if (soundGenerationTransform == null)
        {
            generatorSpeaker.CreateSoundBubble(generatorSound, null, gameObject, false);
        }
        else
        {
            generatorSpeaker.CreateSoundBubble(generatorSound, soundGenerationTransform.position, gameObject, false);
        }
    }


    [ContextMenu("Activate")]
    public void ActivateGenerator()
    {
        IsActive = true;
    }

    [ContextMenu("Deactivate")]
    public void DeactivateGenerator()
    {
        IsActive = false;
    }
}
