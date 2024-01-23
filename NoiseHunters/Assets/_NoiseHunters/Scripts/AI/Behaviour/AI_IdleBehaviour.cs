using UnityEngine;

public class AI_IdleBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIPatrol entityPatrolAgent;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerDoPatrolPoint;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        #region Get Initial Components
        if (entityBrain == null)
        {
            animator.TryGetComponent(out AIBrain thisEntityBrain);
            entityBrain = thisEntityBrain;

            if (entityBrain != null)
            {
                if (entityMoveAgent == null)
                {
                    entityBrain.TryGetComponent(out AIMoveAgent thisEntityMoveAgent);
                    entityMoveAgent = thisEntityMoveAgent;
                }
                if (entityPatrolAgent == null)
                {
                    entityBrain.TryGetComponent(out AIPatrol thisEntityPatrol);
                    entityPatrolAgent = thisEntityPatrol;
                }
            }
        }
        #endregion

        entityMoveAgent?.Agent_SetMovementSpeed(0f);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (entityPatrolAgent != null)
        {
            if (entityPatrolAgent.pathDetected && entityMoveAgent.AgentActive)
            {
                animator.SetTrigger(TriggerDoPatrolPoint);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(TriggerDoPatrolPoint);
    }
}