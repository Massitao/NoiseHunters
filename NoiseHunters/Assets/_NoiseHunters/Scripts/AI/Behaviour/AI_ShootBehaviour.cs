using System.Collections;
using UnityEngine;

public class AI_ShootBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIHearing entityHearing;
    private RifleWeapon entityWeapon;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerNoiseDetected;
    [SerializeField] private string BoolWeaponClipEmpty;


    [Header("Shoot Values")]
    public float meleeDistance;
    public float waitToShootTime;

    private float shootTimer;
    private float rotationSpeed;

    private Coroutine shootingCoroutine;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        #region Get Initial Components
        if (entityBrain == null)
        {
            animator.TryGetComponent(out AIBrain thisEntityBrain);
            entityBrain = thisEntityBrain;

            if (entityBrain != null)
            {
                if (entityBrain is RangedEnemyBrain)
                {
                    RangedEnemyBrain rangedEnemyBrain = entityBrain as RangedEnemyBrain;
                    rotationSpeed = rangedEnemyBrain.RotateAgent_TurnSpeed;

                    entityWeapon = rangedEnemyBrain.EnemyEquipedWeapon;
                    if (entityWeapon != null)
                    {
                        entityWeapon.shotPosition = Vector3.zero;

                        animator.SetBool(BoolWeaponClipEmpty, entityWeapon.clipEmpty);

                        shootTimer = Time.time;
                    }
                }

                if (entityMoveAgent == null)
                {
                    entityBrain.TryGetComponent(out AIMoveAgent thisEntityMoveAgent);
                    entityMoveAgent = thisEntityMoveAgent;
                }

                if (entityHearing == null)
                {
                    entityBrain.TryGetComponent(out AIHearing thisEntityHearing);
                    entityHearing = thisEntityHearing;
                }
            }
        }
        #endregion

        entityMoveAgent.Agent_ClearDestination();

        if (!entityWeapon.clipEmpty)
        {
            if (shootingCoroutine == null)
            {
                shootTimer = Time.time;
                shootingCoroutine = entityBrain.StartCoroutine(Shooting(animator));
            }
        }
        else
        {
            animator.SetBool(BoolWeaponClipEmpty, entityWeapon.clipEmpty);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, entityBrain.AI_LookAt_TargetXZ(entityBrain.detectedTarget, animator.transform.position), rotationSpeed * Time.deltaTime);

        if (entityWeapon.clipEmpty)
        {
            animator.SetBool(BoolWeaponClipEmpty, entityWeapon.clipEmpty);
        }

        if (animator.GetBool(TriggerNoiseDetected))
        {
            animator.ResetTrigger(TriggerNoiseDetected);

            /*
            if (Vector3.Distance(animator.transform.position, entityHearing.listenedSoundPosition) <= meleeDistance)
            {
                animator.SetTrigger(TriggerMeleeCombat);
            }
            else
            {
                animator.ResetTrigger(TriggerNoiseDetected);
            }
            */
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (shootingCoroutine != null)
        {
            entityBrain.StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    IEnumerator Shooting(Animator animator)
    {
        int bulletsShot = 0;
        entityWeapon.PlayChargeParticle();

        while (Time.time < shootTimer + waitToShootTime)
        {
            yield return null;
        }

        while (bulletsShot < entityWeapon.weaponBurstCount)
        {
            if (!entityWeapon.clipEmpty)
            {
                entityWeapon.Shoot(entityBrain.detectedTarget + Vector3.up);
               

                bulletsShot++;
                yield return new WaitForSeconds(entityWeapon.weaponFireRate);
            }
        }

        shootingCoroutine = null;

        yield break;
    }
}