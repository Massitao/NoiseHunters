using System.Collections;
using UnityEngine;

// THIS SCRIPTS NEEDS A MESH WITH THE EdgeColor SHADER / MATERIAL TO WORK.
public class OutlineAdder : MonoBehaviour
{
    #region Outline Adder Values
    [Header("Outline Settings")]
    [SerializeField] private OutlineSettings outlineSettings;

    [Header("Needed Components")]
    private Outline outlineShader;

    [Header("Ping Interaction")]
    private float outline_CurrentWidth;
 
    [Header("Ping State")]
    private Coroutine outline_StartPingCoroutine;
    private Coroutine outline_MidPingCoroutine;
    private Coroutine outline_EndPingCoroutine;


    // Get Components needed for the Shader to work
    private void Awake()
    {
        TryGetComponent(out Outline thisOutline);
        outlineShader = thisOutline;

        if (outlineShader != null && outlineSettings != null)
        {
            SetOutline(0f);
        }
        else
        {
            Debug.LogError("Outline Script and / or Outline Settings are Missing!", gameObject);
        }
    }


    [ContextMenu("Ping")]
    public void ObjectPinged()
    {
        if (outlineShader != null && outlineSettings != null)
        {
            if (outline_MidPingCoroutine != null)
            {
                StopCoroutine(outline_MidPingCoroutine);
                outline_MidPingCoroutine = StartCoroutine(PingMidLoop());
            }
            else if (outline_EndPingCoroutine != null)
            {
                StopCoroutine(outline_EndPingCoroutine);
                outline_EndPingCoroutine = StartCoroutine(PingEnd(true));
            }
            else if (outline_StartPingCoroutine == null)
            {
                outline_StartPingCoroutine = StartCoroutine(PingStart());
            }
        }
    }


    IEnumerator PingStart()
    {
        float lerp = 0f;
        float activationTime = Time.time;

        while (lerp < 1f)
        {
            lerp = Mathf.InverseLerp(activationTime, activationTime + outlineSettings.outline_PingStartTime, Time.time);
            outline_CurrentWidth = Mathf.Lerp(0f, outlineSettings.outline_FullWidth, outlineSettings.outline_PingStartCurve.Evaluate(lerp));
            SetOutline(outline_CurrentWidth);

            yield return null;
        }

        outline_CurrentWidth = outlineSettings.outline_FullWidth;
        SetOutline(outline_CurrentWidth);

        outline_StartPingCoroutine = null;
        outline_MidPingCoroutine = StartCoroutine(PingMidLoop());

        yield break;
    }
    IEnumerator PingMidLoop()
    {
        yield return new WaitForSeconds(outlineSettings.outline_PingMidTime);

        outline_MidPingCoroutine = null;
        outline_EndPingCoroutine = StartCoroutine(PingEnd(false));

        yield break;
    }
    IEnumerator PingEnd(bool playInReverse)
    {
        float initialWidth = outline_CurrentWidth;

        float reducedTimePercentage = Mathf.InverseLerp(0, outlineSettings.outline_FullWidth, outlineShader.OutlineWidth);
        float timeToReachFullWidth = (playInReverse) ? outlineSettings.outline_PingEndTime * (1 - reducedTimePercentage) : outlineSettings.outline_PingEndTime * reducedTimePercentage;

        float widthToReach = (playInReverse) ? outlineSettings.outline_FullWidth : 0f;

        float lerp = 0f;
        float activationTime = Time.time;


        while (lerp < 1f)
        {
            lerp = Mathf.InverseLerp(activationTime - timeToReachFullWidth, activationTime + timeToReachFullWidth, Time.time);
            outline_CurrentWidth = Mathf.Lerp(initialWidth, widthToReach, outlineSettings.outline_PingEndCurve.Evaluate(lerp));
            SetOutline(outline_CurrentWidth);

            yield return null;
        }

        outline_CurrentWidth = widthToReach;

        outline_EndPingCoroutine = null;
        if (playInReverse)
        {
            outline_MidPingCoroutine = StartCoroutine(PingMidLoop());
        }

        yield break;
    }


    void SetOutline(float outlineWidthToSet)
    {
        outlineShader.OutlineWidth = outlineWidthToSet;
    }
    #endregion
}
