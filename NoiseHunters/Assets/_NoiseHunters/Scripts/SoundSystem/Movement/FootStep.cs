using UnityEngine;

public class FootStep : MonoBehaviour
{
    private PlayerMovementSounds playerMovementList;
    private SoundSpeaker footSoundSpeaker;

    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask detectionMask;

    [SerializeField] private GameObject decalPrefab;


    // Start is called before the first frame update
    void Start()
    {
        if (footSoundSpeaker == null)
        {
            footSoundSpeaker = GetComponent<SoundSpeaker>();

            if (footSoundSpeaker == null)
            {
                footSoundSpeaker = gameObject.AddComponent<SoundSpeaker>();
            }
        }

        if (playerMovementList == null)
        {
            playerMovementList = GetComponentInParent<PlayerMovementSounds>();
        }
    }


    public void OnStep()
    {
        Physics.Raycast(transform.position, -Vector3.up, out RaycastHit rayHit, maxDistance, detectionMask, QueryTriggerInteraction.Ignore);

        if (rayHit.transform != null)
        {
            SoundInfo movementStepSound = playerMovementList.SelectingMovementSound();
            if (movementStepSound != null)
            {
                footSoundSpeaker.CreateSoundBubble(movementStepSound, rayHit.point, gameObject, false);
            }

            SpawnDecal(rayHit);
        }
    }

    public void OnLand()
    {
        Physics.Raycast(transform.position, -Vector3.up, out RaycastHit rayHit, maxDistance, detectionMask, QueryTriggerInteraction.Ignore);

        footSoundSpeaker.CreateSoundBubble(playerMovementList.LandSound(), transform.position, gameObject, false);

        SpawnDecal(rayHit);
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -Vector3.up * maxDistance);
    }

    private void SpawnDecal(RaycastHit hitInfo)
    {
        if (decalPrefab != null)
        {
            GameObject decal = Instantiate(decalPrefab);
            decal.transform.position = hitInfo.point + new Vector3(0, 0.001f, 0);
            decal.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        }
    }
}
