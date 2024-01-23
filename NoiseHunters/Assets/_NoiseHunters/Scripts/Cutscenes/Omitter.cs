using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Omitter : MonoBehaviour
{
    [Header("Components")]
    private Animator omitAnimator;
    private Slider omitSlider;
    private PlayerControls playerControls;

    [Header("Omit State")]
    [SerializeField] private bool omitting;
    public bool Omitting
    {
        get { return omitting; }
        set
        {
            if (value != omitting)
            {
                omitting = value;

                if (omitting)
                {
                    if (omitResetCoroutine != null)
                    {
                        StopCoroutine(omitResetCoroutine);
                    }

                    omitProcessCoroutine = StartCoroutine(OmittingAction());
                }
                else
                {
                    if (omitProcessCoroutine != null)
                    {
                        StopCoroutine(omitProcessCoroutine);
                    }

                    omitResetCoroutine = StartCoroutine(StoppingOmitAction());
                }
            }
        }
    }


    [Header("Omit Values")]
    [SerializeField] private float omitTime;
    [SerializeField] private float omitStopDelay;
    [SerializeField] private float omitResetTime;

    private float omitProgress;
    private float OmitProgress
    {
        get { return omitProgress; }
        set
        {
            omitProgress = value;
            omitSlider.value = omitProgress;
        }
    }


    [Header("Animator Values")]
    [SerializeField] private string omitStartTrigger;
    [SerializeField] private string omitEndTrigger;
    [SerializeField] private string omittedTrigger;


    // Events
    [SerializeField] private UnityEvent OnOmit;

    // Coroutines
    private Coroutine omitProcessCoroutine;
    private Coroutine omitResetCoroutine;



    private void Awake()
    {
        omitAnimator = GetComponentInChildren<Animator>();
        omitSlider = GetComponentInChildren<Slider>();
        playerControls = new PlayerControls();
    }

    public void EnableInput()
    {
        playerControls.VideoMap.Enable();
        playerControls.VideoMap.StartOmit.started += doOmit => Omitting = true;
        playerControls.VideoMap.EndOmit.performed += stopOmit => Omitting = false;
    }
    public void DisableInput()
    {
        playerControls.VideoMap.Disable();
        playerControls.VideoMap.StartOmit.started -= doOmit => Omitting = true;
        playerControls.VideoMap.EndOmit.performed -= stopOmit => Omitting = false;
    }



    private IEnumerator OmittingAction()
    {
        if (!omitAnimator.GetBool(omitStartTrigger) && omitProgress == 0f)
        {
            omitAnimator?.SetTrigger(omitStartTrigger);
        }
        float timer = Mathf.Lerp(0f, omitTime, OmitProgress);

        while (omitProgress != 1f)
        {
            timer += Time.unscaledDeltaTime;
            OmitProgress = Mathf.InverseLerp(0f, omitTime, timer);
            yield return null;
        }

        omitAnimator?.SetTrigger(omittedTrigger);

        OnOmit?.Invoke();

        yield break;
    }
    private IEnumerator StoppingOmitAction()
    {
        float timer = Mathf.Lerp(0f, omitResetTime, OmitProgress);

        yield return new WaitForSecondsRealtime(omitStopDelay);

        while (omitProgress != 0)
        {
            timer -= Time.unscaledDeltaTime;
            OmitProgress = Mathf.InverseLerp(0f, omitResetTime, timer);
            yield return null;
        }
        omitAnimator?.SetTrigger(omitEndTrigger);

        yield break;
    }
}
