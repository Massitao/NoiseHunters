using System;
using System.Collections;
using UnityEngine;

public class DelayAction
{
    public static void DoDelayedAction(MonoBehaviour instigator, Action actionCallback, float delayTime)
    {
        instigator.StartCoroutine(DelayedAction(actionCallback, delayTime));
    }

    private static IEnumerator DelayedAction(Action actionCallback, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        actionCallback?.Invoke();

        yield break;
    }

    public static void DoDelayedActionUnscaled(MonoBehaviour instigator, Action actionCallback, float delayTime)
    {
        instigator.StartCoroutine(DelayedActionUnscaled(actionCallback, delayTime));
    }

    private static IEnumerator DelayedActionUnscaled(Action actionCallback, float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        actionCallback?.Invoke();

        yield break;
    }

}
