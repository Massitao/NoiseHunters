using System.Collections;
using UnityEngine;

public class CollisionAlarm : MonoBehaviour
{
    private SoundGenerator soundGenerator;

    [SerializeField] private bool canBeInteracted;

    [SerializeField] private float alarmTime;
    private Coroutine alarmCoroutine;

    private void Awake()
    {
        if (TryGetComponent(out SoundGenerator generator))
        {
            soundGenerator = generator;
            soundGenerator.IsActive = false;
        }
        else
        {
            Debug.LogError("No SoundGenerator Found");
        }
    }

    public void AlarmSetActive(bool active)
    {
        canBeInteracted = active;

        if (!canBeInteracted && alarmCoroutine != null)
        {
            StopCoroutine(alarmCoroutine);
        }
    }

    public void StartGenerator()
    {
        if (canBeInteracted)
        {
            if (alarmCoroutine != null)
            {
                StopCoroutine(alarmCoroutine);
            }
            alarmCoroutine = StartCoroutine(GeneratorLoop());
        }
    }

    IEnumerator GeneratorLoop()
    {
        soundGenerator.IsActive = true;

        yield return new WaitForSeconds(alarmTime);

        soundGenerator.IsActive = false;
        alarmCoroutine = null;

        yield break;
    }
}
