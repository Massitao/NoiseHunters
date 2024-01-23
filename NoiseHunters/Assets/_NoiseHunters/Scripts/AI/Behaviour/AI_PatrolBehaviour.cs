using UnityEngine;

public class AI_PatrolBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIPatrol entityPatrolAgent;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerArrivedToPatrolPoint;


    [Header("Patrol Values")]
    [SerializeField] private Vector2 randomStoppingDistance;


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


        if (entityMoveAgent != null && entityPatrolAgent != null)
        {
            entityMoveAgent.Agent_SetStoppingDistance(entityBrain.AI_ChooseRandomNumber(randomStoppingDistance));

            if (entityBrain is RangedEnemyBrain)
            {
                RangedEnemyBrain rangedEnemyBrain = entityBrain as RangedEnemyBrain;
                entityMoveAgent.Agent_SetMovementSpeed(rangedEnemyBrain.MoveAgent_WalkSpeed);

                if (entityMoveAgent.Agent_GetMovementSpeed() == 0f)
                {
                    Debug.LogWarning("AI has no Movement Speed! Agent will not move!");
                }
            }

            if (entityBrain is PredatorEnemyBrain)
            {
                PredatorEnemyBrain predatorEnemyBrain = entityBrain as PredatorEnemyBrain;
                entityMoveAgent.Agent_SetMovementSpeed(predatorEnemyBrain.MoveAgent_WalkSpeed);

                if (entityMoveAgent.Agent_GetMovementSpeed() == 0f)
                {
                    Debug.LogWarning("AI has no Movement Speed! Agent will not move!");
                }
            }


            entityMoveAgent.Agent_SetDestination(entityPatrolAgent.Patrol_GetCurrentPointPosition().position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((entityMoveAgent != null && entityMoveAgent.AgentActive) && entityPatrolAgent != null)
        {
            if (!entityMoveAgent.Agent_HasPathPending() && entityMoveAgent.Agent_GetRemainingDistance() <= entityMoveAgent.Agent_GetStoppingDistance())
            {
                if (entityPatrolAgent.Patrol_CheckPatrolPoint())
                {
                    if (entityPatrolAgent.Patrol_GetPointWaitTime() != 0f)
                    {
                        animator.SetTrigger(TriggerArrivedToPatrolPoint);
                    }
                    else
                    {
                        entityPatrolAgent.Patrol_PointChange();
                        entityMoveAgent.Agent_SetDestination(entityPatrolAgent.Patrol_GetCurrentPointPosition().position);
                    }
                }
                else
                {
                    entityPatrolAgent.Patrol_PointChange();
                    entityMoveAgent.Agent_SetDestination(entityPatrolAgent.Patrol_GetCurrentPointPosition().position);
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(TriggerArrivedToPatrolPoint);
    }
}