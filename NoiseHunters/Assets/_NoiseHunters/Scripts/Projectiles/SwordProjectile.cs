using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwordProjectile : MonoBehaviour
{
    [Header("Projectile Component")]
    private Rigidbody swordRB;
    private SoundSpeaker swordSpeaker;

    private CapsuleCollider swordMeshCollider;
    private SphereCollider swordPickupCollider;


    [Header("Projectile Values")]
    [SerializeField] private int projectileDamage;
    [HideInInspector] public float projectileSpeed;


    [Header("Projectile Sounds")]
    [SerializeField] private SoundInfo daggerImpactSound;
    [SerializeField] private SoundInfo daggerImpactMediumSound;
    [SerializeField] private SoundInfo daggerImpactSubtleSound;

    [SerializeField] private SoundInfo daggerImpactInteractuableSound;

    [SerializeField] private SoundInfo daggerPickupManualSound;
    [SerializeField] private SoundInfo daggerPickupMagnetSound;

    private SoundInfo daggerPickupSoundToPlay;

    [Header("Projectile Events")]
    public Action<Collision> OnProjectileCollision;

    [Header("Projectile Particles")]
    [SerializeField] private GameObject sparkParticle;
    [SerializeField] private GameObject shockwave;
    [SerializeField] private Transform shockwaveSpawnPoint;
    private GameObject currentShockwave;


    private void OnEnable()
    {
        OnProjectileCollision += ProjectileCollision;
        OnProjectileCollision += ShockCollision;
    }

    private void OnDisable()
    {
        OnProjectileCollision -= ProjectileCollision;
        OnProjectileCollision -= ShockCollision;
    }

    void Awake()
    {
        TryGetComponent(out SoundSpeaker thisSpeaker);
        // Set components
        if (thisSpeaker == null)
        {
            swordSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }
        else
        {
            swordSpeaker = thisSpeaker;
        }

        TryGetComponent(out Rigidbody thisRB);
        swordRB = thisRB;

        TryGetComponent(out CapsuleCollider thisMeshCollider);
        swordMeshCollider = thisMeshCollider;

        TryGetComponent(out SphereCollider thisPickupCollider);
        swordPickupCollider = thisPickupCollider;
    }

    private void Start()
    {
        // Set Force
        swordRB.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        daggerPickupSoundToPlay = daggerPickupManualSound;
    }

    private void Update()
    {
        if (!swordRB.isKinematic)
        {
            transform.rotation = Quaternion.LookRotation(swordRB.velocity.normalized, Vector3.up);
        }
    }

    public void ProjectileLaunch()
    {
        swordRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        swordRB.isKinematic = false;

        swordMeshCollider.enabled = true;
        swordPickupCollider.enabled = false;
    }



    void ProjectileCollision(Collision collidedWith)
    {
        transform.parent = collidedWith.transform;

        ProjectilePickupMode();
    }
    void ShockCollision(Collision collidedWith)
    {
        // Get all possible shockable entities
        collidedWith.collider.TryGetComponent(out AIShock shockableAI);
        collidedWith.collider.TryGetComponent(out ShockableEntity shockableEntity);

        SoundInfo soundToPlay = daggerImpactSound;
        if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Medium)
        {
            soundToPlay = daggerImpactMediumSound;
        }
        else if (SaveInstance._Instance.currentLoadedConfig.userWaveBrightness == BrightnessEnum.Subtle)
        {
            soundToPlay = daggerImpactSubtleSound;
        }

        GameObject decal1 = Instantiate(sparkParticle);

        currentShockwave = Instantiate(shockwave, shockwaveSpawnPoint.position, Quaternion.identity, null);
        currentShockwave.TryGetComponent(out ParticleSystem shockwaveParticleSystem);
        shockwaveParticleSystem.Play();

        shockwave.GetComponent<ParticleSystem>().Play();

        decal1.transform.position = collidedWith.GetContact(0).point; 
        decal1.transform.forward = collidedWith.GetContact(0).normal;


        // If impacted collider has the component AIShock, call shock event
        if (shockableAI != null)
        {
            shockableAI.Shock_OnBeingElectrified();
        }
        // If impacted collider has the component ShockableEntity, call shock event
        else if (shockableEntity != null)
        {
            shockableEntity.OnShock?.Invoke();
            soundToPlay = daggerImpactInteractuableSound;
        }

        swordSpeaker.CreateSoundBubble(soundToPlay, transform.position, gameObject, false);
    }

    public void ProjectilePickupMode()
    {
        swordRB.collisionDetectionMode = CollisionDetectionMode.Discrete;
        swordRB.isKinematic = true;

        swordMeshCollider.enabled = false;
        swordPickupCollider.enabled = true;
    }
    public void ProjectileReturning()
    {
        swordRB.isKinematic = true;
        swordRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        swordMeshCollider.enabled = false;
        swordPickupCollider.enabled = true;

        daggerPickupSoundToPlay = daggerPickupMagnetSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnProjectileCollision?.Invoke(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out CharacterCombat playerCombat))
        {
            swordSpeaker.CreateSoundBubble(daggerPickupSoundToPlay, null, gameObject, true);

            playerCombat.Combat_SwordPickUp();
            Destroy(gameObject);
        }
    }

}
