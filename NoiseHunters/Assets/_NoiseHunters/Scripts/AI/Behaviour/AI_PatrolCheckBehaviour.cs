using UnityEngine;

public class AI_PatrolCheckBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIPatrol entityPatrolAgent;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerDoPatrolPoint;


    [Header("Patrol Check Values")]
    float waitTimer = 0f;
    float waitTime = 0f;
    bool doLookAtPointForward = false;


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
            entityMoveAgent.Agent_SetMovementSpeed(0f);
            entityMoveAgent.Agent_ClearDestination();


            doLookAtPointForward = entityPatrolAgent.Patrol_LookAtPatrolPointRotation();
            waitTime = entityPatrolAgent.Patrol_GetPointWaitTime();

            waitTimer = Time.time;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((entityMoveAgent != null && entityMoveAgent.AgentActive) && entityPatrolAgent != null)
        {
            if (doLookAtPointForward)
            {
                float rotationSpeed = 0f;

                if (entityBrain is RangedEnemyBrain)
                {
                    RangedEnemyBrain rangedEnemyBrain = entityBrain as RangedEnemyBrain;
                    rotationSpeed = rangedEnemyBrain.RotateAgent_TurnSpeed;
                }

                entityBrain.transform.rotation =
                Quaternion.Slerp(entityBrain.transform.rotation, Quaternion.LookRotation(entityPatrolAgent.Patrol_GetCurrentPointPosition().forward, Vector3.up), Time.deltaTime * rotationSpeed);
            }

            if (Time.time >= waitTimer + waitTime)
            {
                entityPatrolAgent.Patrol_PointChange();
                animator.SetTrigger(TriggerDoPatrolPoint);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(TriggerDoPatrolPoint);
    }
}