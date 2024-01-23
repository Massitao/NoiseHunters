using UnityEngine;

public class RifleWeapon : MonoBehaviour
{
    public int clipAmmoSize;
    public int clipAmmoCurrent;
    public bool clipEmpty;

    public float weaponFireRate;
    public int weaponBurstCount;

    public Transform gunMuzzle;
    public GameObject bulletProjectile;

    public Vector3 previousPosition;
    public Vector3 shotPosition;

    public string Animator_TriggerFiringRifle;

    Animator myAnimator;

    SoundSpeaker rifleSpeaker;
    [SerializeField] private SoundInfo shootSound;

    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private ParticleSystem chargeParticle;


    private void Start()
    {
        clipAmmoCurrent = clipAmmoSize;

        clipEmpty = (clipAmmoCurrent <= 0) ? true : false;

        myAnimator = GetComponentInParent<Animator>();

        rifleSpeaker = GetComponent<SoundSpeaker>();
    }

    public void Shoot(Vector3 targetToShoot)
    {
        if (clipAmmoCurrent > 0)
        {
            myAnimator.SetTrigger(Animator_TriggerFiringRifle);
            Vector3 dirToShoot = (targetToShoot - transform.position).normalized;

            GameObject bullet = Instantiate(bulletProjectile, gunMuzzle.position, Quaternion.LookRotation(dirToShoot, Vector3.up));

            rifleSpeaker.CreateSoundBubble(shootSound, null, gameObject, true);

            PlayShootParticle();

            Collider[] thisEntityColliders = GetComponentsInParent<Collider>();

            for (int i = 0; i < thisEntityColliders.Length; i++)
            {
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), thisEntityColliders[i], true);
            }

            clipAmmoCurrent--;

            if (clipAmmoCurrent <= 0)
            {
                clipAmmoCurrent = 0;
                clipEmpty = true;
            }
        }
    }

    public void Reload()
    {
        clipAmmoCurrent = clipAmmoSize;
        clipEmpty = false;
    }

    public void PlayShootParticle()
    {
        shootParticle.Play();
    }

    public void PlayChargeParticle()
    {
        chargeParticle.Play();
    }

}