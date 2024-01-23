using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class OnTriggerEnterOverlap : OnTriggerBaseOverlap
{
    [Space(5)]

    [Header("Do Trigger Enter Once?")]
    [SerializeField] protected bool doOnce;
    private bool active = true;

    [Header("Trigger Events")]
    [SerializeField] protected UnityEvent onTriggerEnter;


    private void OnTriggerEnter(Collider enteredCollider)
    {
        if (active)
        {
            switch (triggerModeToUse)
            {
                case TriggerModes.Any:
                    onTriggerEnter.Invoke();

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
                            if (enteredCollider.gameObject == goNeededToTrigger[i])
                            {
                                onTriggerEnter.Invoke();

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

                    if (triggerLayerMask == (triggerLayerMask | 1 << enteredCollider.gameObject.layer))
                    {
                        onTriggerEnter.Invoke();

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