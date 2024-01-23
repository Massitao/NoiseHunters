using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class OnTriggerExitOverlap : OnTriggerBaseOverlap
{
    [Space(5)]

    [Header("Do Trigger Exit Once?")]
    [SerializeField] protected bool doOnce;
    private bool active = true;

    [Header("Trigger Events")]
    [SerializeField] protected UnityEvent onTriggerExit;


    private void OnTriggerExit(Collider exitedCollider)
    {
        if (active)
        {
            switch (triggerModeToUse)
            {
                case TriggerModes.Any:
                    onTriggerExit.Invoke();

                    if (doOnce)
                    {
                        active = false;
                    }

                    break;

                case TriggerModes.GameObjects_Only:

                    if (goNeededToTrigger.Count > 0)
                    {
                        for (int i = 0; i < goNeededToTrigger.Count; i++)
                        {
                            if (exitedCollider.gameObject == goNeededToTrigger[i])
                            {
                                onTriggerExit.Invoke();

                                if (doOnce)
                                {
                                    active = false;
                                }

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

                    if (triggerLayerMask == (triggerLayerMask | 1 << exitedCollider.gameObject.layer))
                    {
                        onTriggerExit.Invoke();

                        if (doOnce)
                        {
                            active = false;
                        }
                    }

                    break;
            }
        }
    }
}