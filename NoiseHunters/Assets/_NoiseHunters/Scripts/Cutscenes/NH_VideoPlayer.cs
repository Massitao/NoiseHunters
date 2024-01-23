using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class NH_VideoPlayer : MonoBehaviour
{
    [Header("Components")]
    private VideoPlayer vidPlayer;
    private Omitter omitter;

    // Events
    [SerializeField] private UnityEvent OnSkip;


    private void Awake()
    {
        vidPlayer = GetComponent<VideoPlayer>();
        omitter = GetComponentInChildren<Omitter>();

        vidPlayer.targetCamera = FindObjectOfType<Camera>();
    }

    private void OnEnable()
    {
        vidPlayer.loopPointReached += VideoEnd;
    }
    private void OnDisable()
    {
        vidPlayer.loopPointReached -= VideoEnd;
        omitter.DisableInput();
    }


    private void Start()
    {
        CursorHandle.CursorLockHandle(CursorLockMode.Confined, false);
        omitter.EnableInput();
        vidPlayer.Play();
    }


    public void SkipCutscene()
    {
        vidPlayer.frame = (long)vidPlayer.clip.frameCount - 1;
    }
    private void VideoEnd(VideoPlayer vp)
    {
        OnSkip?.Invoke();
    }
}