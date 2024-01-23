using UnityEngine;

public class GlitchTrigger : MonoBehaviour
{
    [Header("Camera")]
    private ThirdPersonCamera tpCamera;

    [Header("Guiding Description")]
    [TextArea]
    [SerializeField] private string glitchTriggerDescription;

    [Header("Glitch Effect To Set On Trigger")]
    [Range(0f, 1f)] [SerializeField] private float scanLineJitterToSet;
    [Range(0f, 1f)] [SerializeField] private float colorDriftToSet;
    [Range(0f, 1f)] [SerializeField] private float verticalJumpToSet;
    [SerializeField] private float timeToReachGlitchEffect;


    public void Awake()
    {
        tpCamera = FindObjectOfType<ThirdPersonCamera>();
    }

    public void GlitchTriggeringEnter()
    {
        if (tpCamera != null)
        {
            tpCamera.SetAnalogGlitchValues(scanLineJitterToSet, colorDriftToSet, verticalJumpToSet, timeToReachGlitchEffect, true);
        }
    }
    public void GlitchTriggeringExit()
    {
        if (tpCamera != null)
        {
            tpCamera.SetAnalogGlitchValues(scanLineJitterToSet, colorDriftToSet, verticalJumpToSet, timeToReachGlitchEffect, false);
        }
    }
}
