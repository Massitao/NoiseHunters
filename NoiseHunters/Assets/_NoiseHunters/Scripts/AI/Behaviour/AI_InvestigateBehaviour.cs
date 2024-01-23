using UnityEngine;

public class AI_InvestigateBehaviour : StateMachineBehaviour
{
    /*
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIPatrol entityPatrol;
    private AISuspect entitySuspect;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerWaitDelayPassed;


    [Header("Check Last Position Values")]
    [Range(0f, 1f)] [SerializeField] private float suspectLoss;
    [Range(0f, 1f)] [SerializeField] private float alertLoss;

    [SerializeField] private Vector2 randomStoppingDistance;
    [SerializeField] private Vector2 randomInvestigateWaitTime;
    [SerializeField] private float distanceToInvestigate;

    private float investigateTime = 0f;
    private float investigateTimer = 0f;


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
                if (entityMoveAgent == null)
                {
                    entityBrain.TryGetComponent(out AIMoveAgent thisEntityMoveAgent);
                    entityMoveAgent = thisEntityMoveAgent;
                }
                if (entityPatrol == null)
                {
                    entityBrain.TryGetComponent(out AIPatrol thisEntityPatrol);
                    entityPatrol = thisEntityPatrol;
                }
                if (entitySuspect == null)
                {
                    entityBrain.TryGetComponent(out AISuspect thisEntitySuspect);
                    entitySuspect = thisEntitySuspect;
                }
            }
        }
        #endregion

        if (entityMoveAgent != null)
        {
            entityMoveAgent.Agent_SetDestination(entityMoveAgent.Agent_SetRandomPointToInvestigate(distanceToInvestigate));
            entityMoveAgent.Agent_SetStoppingDistance(entityBrain.AI_ChooseRandomNumber(randomStoppingDistance));
            investigateTime = entityBrain.AI_ChooseRandomNumber(randomInvestigateWaitTime);
        }

        investigateTimer = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((entityMoveAgent != null && entityMoveAgent.AgentActive) && entitySuspect != null)
        {
            if (!entityMoveAgent.Agent_HasPathPending() && entityMoveAgent.Agent_GetRemainingDistance() <= entityMoveAgent.Agent_GetStoppingDistance())
            {
                if (Time.time >= investigateTimer + investigateTime)
                {
                    if (entitySuspect.currentSuspectStatus == AISuspect.SuspectStatus.Alerted)
                    {
                        if (entitySuspect.AlertLevel <= 0f)
                        {
                            if (entitySuspect.statesRemoved == AISuspect.RemoveSuspicionState.Suspect)
                            {
                                Investigate_End(animator);
                            }
                        }
                        else
                        {
                            entitySuspect.Suspect_SetAlertLevel(true, -(alertLoss));
                            Investigate_KeepSearching(animator);
                        }

                    }
                    else if (entitySuspect.currentSuspectStatus == AISuspect.SuspectStatus.Suspecting)
                    {
                        if (entitySuspect.SuspectLevel <= 0f)
                        {
                            Investigate_End(animator);
                        }
                        else
                        {
                            entitySuspect.Suspect_SetSuspicionLevel(true, -(suspectLoss));
                            Investigate_KeepSearching(animator);
                        }
                    }
                    else
                    {
                        Investigate_End(animator);
                    }
                }
            }
            else
            {
                investigateTimer = Time.time;
            }
        }      
    }

    void Investigate_KeepSearching(Animator animator)
    {
        entityMoveAgent.Agent_SetDestination(entityMoveAgent.Agent_SetRandomPointToInvestigate(distanceToInvestigate));
        investigateTime = entityBrain.AI_ChooseRandomNumber(randomInvestigateWaitTime);
        investigateTimer = Time.time;
    }

    void Investigate_End(Animator animator)
    {
        if (entityPatrol != null)
        {
            entityPatrol.Patrol_CheckNearestPatrolPoint();
        }
        animator.SetTrigger(TriggerWaitDelayPassed);
    }
    */
}