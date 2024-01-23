using UnityEngine;

[RequireComponent(typeof(SaveID))]
public class Collectible : Interactuable
{
    [Header("Collectible Components")]
    public GameObject collectibleGameObject;
    private SaveID collectibleID;

    [Header("Sound Components")]
    [SerializeField] private SoundInfo pickUpSound;
    private SoundSpeaker speaker;


    protected override void Awake()
    {
        if (gameObject.TryGetComponent(out SaveID collectible))
        {
            collectibleID = collectible;
        }
        else
        {
            Debug.LogError($"SaveID was not found!", gameObject);
        }

        if (SaveInstance._Instance != null)
        {
            if (CollectibleSet.CheckIfCollectibleWasPicked(collectibleID.thisID))
            {
                DeactivateCollectible();
            }
        }
        else
        {
            Debug.LogError($"SaveInstance was not found!");
        }

        base.Awake();
        speaker = gameObject.AddComponent<SoundSpeaker>();
    }

    protected override void Interact()
    {
        AddCollectible();
    }


    private void AddCollectible()
    {
        CollectibleSet.AddCollectibleToSet(collectibleID.thisID);
        speaker.CreateSoundBubble(pickUpSound, transform.position, gameObject, true);
        DeactivateCollectible();
    }
    private void DeactivateCollectible()
    {
        collectibleGameObject.SetActive(false);
    }
}