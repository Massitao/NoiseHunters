using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] private string transitionEnterTrigger;
    [SerializeField] private string transitionExitToLoadScene;
    [SerializeField] private string transitionExitToOtherScene;
    [HideInInspector] public bool doTransition;
    private Animator transitionAnimator;


    private void Awake()
    {
        transitionAnimator = GetComponent<Animator>();
    }


    public void TriggerTransitionEnter()
    {
        transitionAnimator.SetTrigger(transitionEnterTrigger);
    }
    public void TriggerTransitionExit_LoadScene()
    {
        transitionAnimator.SetTrigger(transitionExitToLoadScene);
    }
    public void TriggerTransitionExit_OtherScene()
    {
        transitionAnimator.SetTrigger(transitionExitToOtherScene);
    }

    // Transition Methods
    public void StartTransition()
    {
        doTransition = true;
    }
}
