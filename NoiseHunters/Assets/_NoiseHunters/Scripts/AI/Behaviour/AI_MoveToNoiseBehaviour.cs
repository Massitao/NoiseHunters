using UnityEngine;

public class AI_MoveToNoiseBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIPatrol entityPatrol;
    private AIHearing entityHearing;
    private AIScanHES entityScanHES;
    private AISuspect entitySuspect;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerNoiseHeard;
    [SerializeField] private string TriggerNoTargetDetected;
    [SerializeField] private string BoolMeleeKill;
    [SerializeField] private string BoolTrackingEnemy;
    [SerializeField] private string BoolIsOnAlert;


    [Header("Check Last Position Values")]
    [SerializeField] private Vector2 randomStoppingDistance;
    [SerializeField] private Vector2 randomAlertedWaitTime;
    private float checkTime = 0f;
    private float checkTimer = 0f;
    private bool doOnceHES = false;


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
                if (entityHearing == null)
                {
                    entityBrain.TryGetComponent(out AIHearing thisEntityHearing);
                    entityHearing = thisEntityHearing;
                }
                if (entityScanHES == null)
                {
                    animator.TryGetComponent(out AIScanHES thisEntityScanHES);
                    entityScanHES = thisEntityScanHES;
                }
                if (entitySuspect == null)
                {
                    entityBrain.TryGetComponent(out AISuspect thisEntitySuspect);
                    entitySuspect = thisEntitySuspect;
                }
            }
        }
        #endregion

        if (entityMoveAgent != null && entityHearing != null)
        {
            entityMoveAgent.Agent_SetStoppingDistance(entityBrain.AI_ChooseRandomNumber(randomStoppingDistance));
            if (entityHearing.listenedSoundPosition == Vector3.zero)
            {
                entityMoveAgent.Agent_SetDestination(entityBrain.transform.position);
                entityHearing.listenedSoundPosition = Vector3.zero;
            }
            else
            {
                entityMoveAgent.Agent_SetDestination(entityBrain.detectedTarget);
            }

            checkTime = (randomAlertedWaitTime != Vector2.zero) ? entityBrain.AI_ChooseRandomNumber(randomAlertedWaitTime) : 0f;

            if (entityBrain is RangedEnemyBrain)
            {
                RangedEnemyBrain rangedEnemyBrain = entityBrain as RangedEnemyBrain;

                entityMoveAgent.Agent_SetMovementSpeed(rangedEnemyBrain.MoveAgent_RunSpeed);

                if (entityMoveAgent.Agent_GetMovementSpeed() == 0f)
                {
                    Debug.LogWarning("AI has no Movement Speed! Agent will not move!");
                }
            }

            if (entityBrain is PredatorEnemyBrain)
            {
                PredatorEnemyBrain predatorEnemyBrain = entityBrain as PredatorEnemyBrain;

                float speedToSet = (predatorEnemyBrain.SuspectComponent.AlertLevel > 0f) ? predatorEnemyBrain.MoveAgent_RunSpeed : predatorEnemyBrain.MoveAgent_SuspectSpeed;
                entityMoveAgent.Agent_SetMovementSpeed(speedToSet);

                if (entityMoveAgent.Agent_GetMovementSpeed() == 0f)
                {
                    Debug.LogWarning("AI has no Movement Speed! Agent will not move!");
                }
            }


            checkTimer = Time.time;
            doOnceHES = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((entityMoveAgent != null && entityMoveAgent.AgentActive) && entityHearing != null)
        {
            if (!entityMoveAgent.Agent_HasPathPending() && entityMoveAgent.Agent_GetRemainingDistance() <= entityMoveAgent.Agent_GetStoppingDistance())
            {
                if (Time.time >= checkTimer + checkTime)
                {
                    entitySuspect.Suspect_SetAlertLevel(false, 0f);
                    animator.SetTrigger(TriggerNoTargetDetected);
                }

                if (!doOnceHES)
                {
                    if (entityScanHES != null)
                    {
                        if (entityBrain is RangedEnemyBrain)
                        {
                            RangedEnemyBrain rangedEnemyBrain = entityBrain as RangedEnemyBrain;
                            rangedEnemyBrain.RangedEnemy_ScanHES_EmitScan();

                            doOnceHES = true;
                        }
                        if (entityBrain is PredatorEnemyBrain)
                        {
                            PredatorEnemyBrain predatorEnemyBrain = entityBrain as PredatorEnemyBrain;
                            predatorEnemyBrain.PredatorEnemy_ScanHES_EmitScan();

                            doOnceHES = true;
                        }
                    }
                }
            }
            else
            {
                checkTimer = Time.time;
                entityMoveAgent.Agent_SetDestination(entityBrain.detectedTarget);
                animator.ResetTrigger(TriggerNoiseHeard);
            }

            if (animator.GetBool(TriggerNoiseHeard) || animator.GetBool(BoolTrackingEnemy))
            {
                checkTimer = Time.time;
                entityMoveAgent.Agent_SetDestination(entityBrain.detectedTarget);
                animator.ResetTrigger(TriggerNoiseHeard);
            }

            if (entityBrain is PredatorEnemyBrain)
            {
                PredatorEnemyBrain predatorEnemyBrain = entityBrain as PredatorEnemyBrain;


                Debug.DrawLine(predatorEnemyBrain.EnemyCollider.bounds.center, predatorEnemyBrain.PlayerPosition.c_CharacterCapsuleCollider.bounds.center);
                if (Physics.Linecast(predatorEnemyBrain.EnemyCollider.bounds.center, predatorEnemyBrain.PlayerPosition.c_CharacterCapsuleCollider.bounds.center, out RaycastHit hit) && hit.transform == predatorEnemyBrain.PlayerPosition.transform)
                {
                    if (Vector3.Distance(animator.transform.position, predatorEnemyBrain.PlayerPosition.transform.position) <= predatorEnemyBrain.melee_KillTargetDistance && animator.GetBool(BoolIsOnAlert))
                    {
                        animator.SetBool(BoolMeleeKill, true);
                    }
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        entityMoveAgent.Agent_ClearDestination();
    }
}