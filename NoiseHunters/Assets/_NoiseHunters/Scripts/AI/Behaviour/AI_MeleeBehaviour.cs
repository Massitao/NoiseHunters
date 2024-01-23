using UnityEngine;

public class AI_MeleeBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIHearing entityHearing;
    private AISuspect entitySuspect;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerNoiseHeard;
    [SerializeField] private string TriggerMeleeStart;
    [SerializeField] private string BoolMeleeEnd;


    float rotationSpeed;
    float timer;

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
                if (entityBrain is PredatorEnemyBrain)
                {
                    PredatorEnemyBrain predatorEnemyBrain = entityBrain as PredatorEnemyBrain;
                    //entityHurtbox = rangedEnemyBrain.EnemyArmHurtbox;
                    rotationSpeed = predatorEnemyBrain.RotateAgent_TurnSpeed;
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

                if (entitySuspect == null)
                {
                    entityBrain.TryGetComponent(out AISuspect thisEntitySuspect);
                    entitySuspect = thisEntitySuspect;
                }
            }
        }
        #endregion

        animator.SetTrigger(TriggerMeleeStart);

        entityMoveAgent.Agent_ClearDestination();

        timer = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, entityBrain.AI_LookAt_TargetXZ(entityBrain.PlayerPosition.transform.position, animator.transform.position), rotationSpeed * Time.deltaTime);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Melee") && Time.time >= timer + 0.5f)
        {
            if (!entityBrain.PlayerPosition.GetComponent<PlayerHealth>().IsAlive)
            {
                animator.ResetTrigger(TriggerNoiseHeard);
                entitySuspect.Suspect_SetAlertLevel(false, 0f);
            }
            animator.SetBool(BoolMeleeEnd, false);
        }
    }
}