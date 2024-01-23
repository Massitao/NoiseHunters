using UnityEngine;

public class AI_AlertBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AIMoveAgent entityMoveAgent;
    private AIScanHES entityScanHES;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerAlertWaitDelayPassed;


    [Header("Alert Values")]
    [SerializeField] private Vector2 randomAlertedWaitTime;
    private float alertTime = 0f;
    private float alertTimer = 0f;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        #region Get Initial Components
        if (entityBrain == null)
        {
            animator.TryGetComponent(out AIBrain thisEntityBrain);
            entityBrain = thisEntityBrain;

            if (entityBrain != null)
            {
                animator.TryGetComponent(out AIMoveAgent thisEntityMoveAgent);
                entityMoveAgent = thisEntityMoveAgent;

                animator.TryGetComponent(out AIScanHES thisEntityScanHES);
                entityScanHES = thisEntityScanHES;
            }
        }
        #endregion

        if (entityBrain != null && randomAlertedWaitTime != Vector2.zero)
        {
            alertTime = entityBrain.AI_ChooseRandomNumber(randomAlertedWaitTime);
        }

        if (entityMoveAgent != null)
        {
            entityMoveAgent.Agent_ClearDestination();
        }

        if (entityScanHES != null)
        {
            if (entityBrain is RangedEnemyBrain)
            {
                RangedEnemyBrain rangedEnemyBrain = entityBrain as RangedEnemyBrain;
                rangedEnemyBrain.RangedEnemy_ScanHES_EmitScan();
            }
            if (entityBrain is PredatorEnemyBrain)
            {
                PredatorEnemyBrain predatorEnemyBrain = entityBrain as PredatorEnemyBrain;
                predatorEnemyBrain.PredatorEnemy_ScanHES_EmitScan();
            }
        }

        alertTimer = Time.time;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time >= alertTimer + alertTime)
        {
            animator.SetTrigger(TriggerAlertWaitDelayPassed);
        }
    }
}
