using System.Collections;
using UnityEngine;

public class Heartbeat : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private ThirdPersonController thirdPersonController;

    private bool isAlive;

    [SerializeField] private float heartbeatInterval;
    [SerializeField] private float heartbeatAfterIntervalTime;

    private SoundSpeaker heartbeatSpeaker;

    [SerializeField] private SoundInfo firstHeartbeatSound;
    [SerializeField] private SoundInfo secondHeartbeatSound;

    Coroutine heartbeatBehaviour;

    private void Start()
    {
        thirdPersonController = GetComponentInParent<ThirdPersonController>();
        playerHealth = GetComponentInParent<PlayerHealth>();

        if (heartbeatSpeaker == null)
        {
            heartbeatSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }
        else
        {
            heartbeatSpeaker = GetComponentInChildren<SoundSpeaker>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckLife();
    }

    void CheckLife()
    {
        if (playerHealth.IsAlive && thirdPersonController.controllerVelocity == Vector3.zero)
        {
            StartHeartbeat();
        }
        else
        {
            EndHeartBeat();
        }
    }

    void StartHeartbeat()
    {
        if (heartbeatBehaviour == null)
            heartbeatBehaviour = StartCoroutine(HeartbeatBehaviour());
    }

    IEnumerator HeartbeatBehaviour()
    {
        while (true)
        {
            yield return new WaitForSeconds(heartbeatAfterIntervalTime);

            heartbeatSpeaker.CreateSoundBubble(firstHeartbeatSound, transform.position, gameObject, false);
            yield return new WaitForSeconds(heartbeatInterval);
            heartbeatSpeaker.CreateSoundBubble(secondHeartbeatSound, transform.position, gameObject, false);
        }
    }

    void EndHeartBeat()
    {
        if (heartbeatBehaviour != null)
        {
            StopCoroutine(heartbeatBehaviour);
            heartbeatBehaviour = null;
        }
    }
}
