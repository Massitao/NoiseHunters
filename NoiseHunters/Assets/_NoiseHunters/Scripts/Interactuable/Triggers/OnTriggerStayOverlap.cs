using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class OnTriggerStayOverlap : OnTriggerBaseOverlap
{
    [Space(5)]

    [Header("On Stay Trigger Values")]
    [Tooltip("Mark this if you want every GameObject or Script in the list to be present inside the trigger. Not using this option will mean that any object in the list selected will trigger the events.")]
    [SerializeField] protected bool allGameObjectsOrScriptsRequired;

    [Space(5)]

    [Header("Trigger Events")]
    [Tooltip("OnTriggerStay can be dangerous if used inproperly! Think about why will you use this script for before trying!")]
    [SerializeField] protected UnityEvent onTriggerStay;


    private void OnTriggerStay(Collider stayingCollider)
    {
        switch (triggerModeToUse)
        {
            case TriggerModes.Any:
                onTriggerStay.Invoke();

                break;

            case TriggerModes.GameObjects_Only:

                if (goNeededToTrigger.Count > 0)
                {
                    for (int i = 0; i < goNeededToTrigger.Count; i++)
                    {
                        if (stayingCollider.gameObject == goNeededToTrigger[i])
                        {
                            onTriggerStay.Invoke();
                            break;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"{triggerModeToUse} is being used, but, no GameObjects where placed in {goNeededToTrigger.ToString()}! It will never trigger!", gameObject);
                }

                break;

            case TriggerModes.LayerMasked_GameObjects_Only:

                if (triggerLayerMask == (triggerLayerMask | 1 << stayingCollider.gameObject.layer))
                {
                    onTriggerStay.Invoke();
                }

                break;
        }
    }
}