using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);

        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out LivingEntity entityToHit) && collision.transform.TryGetComponent(out PlayerHealth playerHit))
        {
            playerHit.DealDamage(damage, entityToHit);
        }

        Destroy(gameObject);
    }
}
