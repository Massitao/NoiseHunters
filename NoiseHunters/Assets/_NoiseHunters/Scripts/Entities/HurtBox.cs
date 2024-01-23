using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private enum HurtBoxType { Player, Other };
    private HurtBoxType hurtBoxType;

    private Collider hurtCollider;

    bool collidedBefore;

    private void Start()
    {
        hurtCollider = GetComponent<Collider>();

        if (GetComponentInParent<PlayerHealth>() != null)
        {
            hurtBoxType = HurtBoxType.Player;
        }
        else
        {
            hurtBoxType = HurtBoxType.Other;
        }

        List<Collider> thisEntityColliders = new List<Collider>(GetComponentsInParent<Collider>());

        for (int i = 0; i < thisEntityColliders.Count; i++)
        {
            Physics.IgnoreCollision(hurtCollider, thisEntityColliders[i]);
        }
    }

    public void ActivateHurtBox()
    {
        hurtCollider.enabled = true;
    }

    public void DeactivateHurtBox()
    {
        hurtCollider.enabled = false;
        collidedBefore = false;
    }

    void DealDamage(LivingEntity hitEntity)
    {
        collidedBefore = true;
        hitEntity.TakeDamage(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LivingEntity>() != null && !collidedBefore)
        {
            LivingEntity hitEntity = other.GetComponent<LivingEntity>();

            if (hurtBoxType == HurtBoxType.Player && other.GetComponentInParent<PlayerHealth>() != null)
            {
                DealDamage(hitEntity);
            }
            else
            {
                DealDamage(hitEntity);
            }
        }
    }
}
