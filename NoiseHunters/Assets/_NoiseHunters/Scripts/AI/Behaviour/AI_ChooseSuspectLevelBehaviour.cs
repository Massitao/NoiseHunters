using UnityEngine;

public class AI_ChooseSuspectLevelBehaviour : StateMachineBehaviour
{
    [Header("Behaviour Components Needed")]
    private AIBrain entityBrain;
    private AISuspect entitySuspect;


    [Header("Animator Strings Needed")]
    [SerializeField] private string TriggerSuspect;
    [SerializeField] private string TriggerAlert;



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
                animator.TryGetComponent(out AISuspect thisEntitySuspect);
                entitySuspect = thisEntitySuspect;
            }
        }

        if (entitySuspect != null)
        {
            if (entitySuspect.AlertLevel > 0f)
            {
                animator.SetTrigger(TriggerAlert);
            }

            /*
            else if (entitySuspect.SuspectLevel > 0f && entitySuspect.AlertLevel <= 0f)
            {
                animator.SetTrigger(TriggerSuspect);
            }
            */
        }
        #endregion
    }
}