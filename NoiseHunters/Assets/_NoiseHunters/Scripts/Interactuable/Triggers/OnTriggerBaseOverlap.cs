using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OnTriggerBaseOverlap : MonoBehaviour
{
    [Header("Collider")]
    [HideInInspector] public Collider thisTrigger;

    [Header("Trigger Mode")]
    [Tooltip("Select the trigger filter mode you want to use.")]
    [SerializeField] protected TriggerModes triggerModeToUse;
    protected enum TriggerModes { Any, GameObjects_Only, LayerMasked_GameObjects_Only }

    [Space(5)]

    [Header("Trigger Lists")]
    [Tooltip("Do not use this if you are going to use any mode that isn't GameObjects_Only.")]
    [SerializeField] protected List<GameObject> goNeededToTrigger;

    [Tooltip("Select the layer you wish to detect GameObjects with. Only works in LayerMasked_GameObjects_Only.")]
    [SerializeField] protected LayerMask triggerLayerMask;


    protected void Start()
    {
        TryGetComponent(out Collider thisTriggerCollider);

        if (thisTriggerCollider != null)
        {
            thisTrigger = thisTriggerCollider;

            if (!thisTrigger.isTrigger)
            {
                Debug.LogWarning($"{gameObject} Trigger Collider is not set as Trigger! Setting it as Trigger", gameObject);
                thisTrigger.isTrigger = true;
            }            
        }
        else
        {
            Debug.LogError("No Collider was found here!", gameObject);
        }
    }
}