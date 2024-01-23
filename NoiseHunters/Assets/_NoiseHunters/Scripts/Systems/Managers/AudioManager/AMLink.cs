using UnityEngine;

public class AMLink : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] public AudioSource audioSource;
    public float initialVolume;


    private void Awake()
    {
        if (audioSource == null)
        {
            if (TryGetComponent(out AudioSource source))
            {
                audioSource = source;
            }
            else
            {
                Debug.LogWarning($"No AudioSource was found! Deleting.");
                Destroy(this);
            }
        }

        initialVolume = audioSource.volume;
        if (AudioManager._Instance != null)
        {
            SetVolume(AudioManager._Instance.soundVolume);
        }
    }

    private void OnEnable()
    {
        AudioManager._Instance.soundVolumeChanged += SetVolume;
        AudioManager._Instance.soundVolumeReset += StopSound;
    }
    private void OnDisable()
    {
        AudioManager._Instance.soundVolumeChanged -= SetVolume;
        AudioManager._Instance.soundVolumeReset -= StopSound;
    }


    public void SetVolume(float newVolume)
    {
        audioSource.volume = initialVolume * newVolume;
    }
    private void StopSound()
    {
        if (audioSource.loop)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        else
        {
            Destroy(this);
            Destroy(audioSource);
        }
    }
}