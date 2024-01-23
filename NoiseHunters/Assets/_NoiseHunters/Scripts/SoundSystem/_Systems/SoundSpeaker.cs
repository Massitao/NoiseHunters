using System.Collections.Generic;
using UnityEngine;

public class SoundSpeaker : MonoBehaviour
{
    private Camera mainCamera;

    public List<ScannerEffect> scanEffects = new List<ScannerEffect>();
    [SerializeField] Transform soundPosition;

    [HideInInspector] public bool UnderSilencerInfluence;
    [SerializeField] public List<GameObject> silencersAffectingGO = new List<GameObject>();
    

    private void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        if (scanEffects.Count == 0)
        {
            AddNewScanner();
        }
    }

    private void Update()
    {
        CheckSilencerInfluence();
    }

    private ScannerEffect AddNewScanner()
    {
        ScannerEffect newScanner = mainCamera.gameObject.AddComponent<ScannerEffect>();
        scanEffects.Add(newScanner);

        return newScanner;
    }

    public ScannerEffect ScannerCheckAvailability()
    {
        for (int i = 0; i < scanEffects.Count; i++)
        {
            if (!scanEffects[i]._scanning)
            {
                return scanEffects[i];
            }
        }

        return AddNewScanner();
    }

    private void CheckSilencerInfluence()
    {
        UnderSilencerInfluence = (silencersAffectingGO.Count != 0) ? true : false;
    }

    public void CreateSoundBubble(SoundInfo soundToPlay, Vector3? customSoundPosition, GameObject instigator, bool onlyAudio)
    {
        if (soundToPlay != null)
        {
            if (customSoundPosition == null)
            {
                if (soundPosition != null)
                {
                    SoundDictionary.CreateSoundBubble(this, soundToPlay, soundPosition.position, this, instigator, onlyAudio);
                }
                else
                {
                    SoundDictionary.CreateSoundBubble(this, soundToPlay, transform.position, this, instigator, onlyAudio);
                }
            }
            else
            {
                SoundDictionary.CreateSoundBubble(this, soundToPlay, (Vector3)customSoundPosition, this, instigator, onlyAudio);
            }
        }
        else
        {
            Debug.LogWarning($"Sound to Play has not been set! Check {instigator.name} GameObject.", instigator);
        }
    }
}