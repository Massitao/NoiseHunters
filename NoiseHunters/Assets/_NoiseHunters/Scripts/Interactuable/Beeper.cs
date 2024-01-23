using UnityEngine;

public class Beeper : MonoBehaviour
{
    private SoundSpeaker soundSpeaker;

    [SerializeField] private bool canBeInteracted;

    [SerializeField] private Transform beeperTransform;
    [SerializeField] private SoundInfo beeperSound;

    private void Awake()
    {
        if (TryGetComponent(out SoundSpeaker speaker))
        {
            soundSpeaker = speaker;
        }
        else
        {
            soundSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }
    }

    public void BeeperSetActive(bool active)
    {
        canBeInteracted = active;
    }

    public void PlayBeeperSound()
    {
        if (canBeInteracted)
        {
            soundSpeaker.CreateSoundBubble(beeperSound, beeperTransform.position, gameObject, false);
        }
    }
}
