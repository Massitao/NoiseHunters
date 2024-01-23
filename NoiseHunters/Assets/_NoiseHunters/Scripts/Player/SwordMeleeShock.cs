using System.Collections.Generic;
using UnityEngine;

public class SwordMeleeShock : MonoBehaviour
{
    private Collider swordCollider;

    private List<Collider> swordHitColliders = new List<Collider>();


    private void Start()
    {
        swordCollider = GetComponent<Collider>();

        List<Collider> thisEntityColliders = new List<Collider>(GetComponentsInParent<Collider>());

        for (int i = 0; i < thisEntityColliders.Count; i++)
        {
            Physics.IgnoreCollision(swordCollider, thisEntityColliders[i]);
        }
    }

    public void ActivateShock()
    {
        swordCollider.enabled = true;
    }

    public void DeactivateShock()
    {
        swordCollider.enabled = false;
        swordHitColliders.Clear();
    }


    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out AIShock livingShockableEntity);
        other.TryGetComponent(out ShockableEntity shockableEntity);

        if (livingShockableEntity != null)
        {
            for (int i = 0; i < swordHitColliders.Count; i++)
            {
                if (other == swordHitColliders[i])
                {
                    return;
                }
            }

            swordHitColliders.Add(other);

            livingShockableEntity.Shock_OnBeingElectrified();
        }
        else if (shockableEntity != null)
        {
            for (int i = 0; i < swordHitColliders.Count; i++)
            {
                if (other == swordHitColliders[i])
                {
                    return;
                }
            }

            swordHitColliders.Add(other);

            shockableEntity.OnShock?.Invoke();
        }
    }
}