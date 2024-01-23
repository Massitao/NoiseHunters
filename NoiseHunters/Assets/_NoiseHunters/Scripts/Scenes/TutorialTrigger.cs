using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Components")]
    private Animator tutorialCanvasAnimator;
    private Omitter omitter;
    private ThirdPersonController controller;
    private PlayerInput playerInput;

    [Header("Animator Values")]
    [SerializeField] private string tutorialShowTrigger;
    [SerializeField] private string tutorialHideTrigger;

    [Header("Slowdown Time")]
    [SerializeField] private float slowmoTime;
    [SerializeField] private float backToNormalTime;

    [Header("Sounds")]
    [SerializeField] private AudioClip showTutorialSound;
    [SerializeField] private AudioClip endTutorialSound;


    private void Awake()
    {
        tutorialCanvasAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        omitter = GetComponentInChildren<Omitter>();

        controller = FindObjectOfType<ThirdPersonController>();
    }


    public void ShowTutorialText()
    {
        DisableControllerInput();
        playerInput.enabled = true;
        TimescaleHandle.LerpToTimescaleWithTime(this, 0f, slowmoTime);
        tutorialCanvasAnimator?.SetTrigger(tutorialShowTrigger);
    }
    public void EndTutorialText()
    {
        TimescaleHandle.LerpToTimescaleWithTime(this, 1f, backToNormalTime);
        tutorialCanvasAnimator?.SetTrigger(tutorialHideTrigger);
    }

    public void EnableControllerInput()
    {
        controller.OnEnable();
    }
    public void DisableControllerInput()
    {
        controller.OnDisable();
    }

    public void EnableOmitterInput()
    {
        omitter.EnableInput();
    }
    public void DisableOmitterInput()
    {
        omitter.DisableInput();
    }



    public void PlayShowSound()
    {
        PlaySound(showTutorialSound);
    }
    public void PlayEndSound()
    {
        PlaySound(endTutorialSound);
    }
    public void PlaySound(AudioClip soundToPlay)
    {
        SoundDictionary.CreateOnlyAudioSource(this, soundToPlay, transform.position);
    }
}