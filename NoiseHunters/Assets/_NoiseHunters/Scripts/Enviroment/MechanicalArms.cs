using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalArms : MonoBehaviour
{
    [SerializeField]private Transform particleSpawnPoint;
    private SoundSpeaker soundSpeaker;
    private AudioSource aSource;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent(out SoundSpeaker ss))
        {
            soundSpeaker = ss;
        }
        else
        {
            soundSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }
        aSource = GetComponent<AudioSource>();
    }

    public void PlayerOneShotSound(SoundInfo soundInfo)
    {
        PlaySound(soundInfo, particleSpawnPoint.position, false);
    }

    public void SpawnParticle(GameObject particle)
    {
        Instantiate(particle, particleSpawnPoint.position, particleSpawnPoint.rotation);
    }

    public void PlaySoundNoWave(SoundInfo sound)
    {
        PlaySound(sound, particleSpawnPoint.position, true);
    }
    private void PlaySound(SoundInfo soundToPlay, Vector3? posToPlaySound, bool onlySound)
    {
        if (soundToPlay != null)
        {
            soundSpeaker.CreateSoundBubble(soundToPlay, posToPlaySound, gameObject, onlySound);
        }
    }
}
