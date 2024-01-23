using UnityEngine;

public class AI_ReloadBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIHearing entityHearing;
    private RifleWeapon entityWeapon;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerNoiseDetected;
    [SerializeField] private string BoolWeaponClipEmpty;


    [Header("Reload Values")]
    public float waitToReloadTime;
    float reloadTimer;

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
                    entityWeapon = rangedEnemyBrain.EnemyEquipedWeapon;
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

        reloadTimer = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time >= reloadTimer + waitToReloadTime && entityWeapon.clipEmpty)
        {
            entityWeapon.Reload();
            animator.SetBool(BoolWeaponClipEmpty, entityWeapon.clipEmpty);
        }
    }
}
