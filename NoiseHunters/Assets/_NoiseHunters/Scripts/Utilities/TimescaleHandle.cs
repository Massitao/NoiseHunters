using System.Collections;
using UnityEngine;

public class TimescaleHandle : MonoBehaviour
{
    private static float originalTimeScale => 1f;
    private static float pauseTimeScale => 0f;
    private static float initialFixedTimeStep => 0.02f;

    private static float defaultTickTime => 0.05f;

    private static Coroutine lerpToTimeScaleCoroutine;
    private static MonoBehaviour previousScriptCalling;

    public static void SetTimeScale(float newTimeScale)
    {
        ClearCoroutine();

        Time.timeScale = newTimeScale;
        Time.fixedDeltaTime = Time.timeScale * initialFixedTimeStep;
    }

    public static void ResetTimeScale()
    {
        ClearCoroutine();

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = initialFixedTimeStep;
    }

    private static void ClearCoroutine()
    {
        if (lerpToTimeScaleCoroutine != null)
        {
            previousScriptCalling.StopCoroutine(lerpToTimeScaleCoroutine);
            lerpToTimeScaleCoroutine = null;
        }
    }


    public static void LerpToTimescaleWithTick(MonoBehaviour scriptCalling, float newTimeScale, float tickValue, float tickTime)
    {
        ClearCoroutine();

        previousScriptCalling = scriptCalling;
        lerpToTimeScaleCoroutine = scriptCalling.StartCoroutine(LerpingToTimeScaleWithTicks(newTimeScale, tickValue, tickTime));
    }
    private static IEnumerator LerpingToTimeScaleWithTicks(float newTimeScale, float tickValue, float? tickTime)
    {
        float tickTimeChosen = (tickTime != null) ? (float)tickTime : defaultTickTime;

        if (tickValue <= 0)
        {
            Debug.LogWarning($"Tick value '{tickValue}' is not valid. Aborting coroutine.");
            yield break;
        }
        tickValue *= (Time.timeScale < newTimeScale) ? 1f : -1f;

        Vector2 clampValues = (Time.timeScale < newTimeScale) ? new Vector2(float.MinValue, newTimeScale) : new Vector2(newTimeScale, float.MaxValue);

        while (Time.timeScale != newTimeScale)
        {
            yield return new WaitForSecondsRealtime((float)tickTime);

            Time.timeScale = Mathf.Clamp(Time.timeScale + tickValue, clampValues.x, clampValues.y);
            Time.fixedDeltaTime = Time.timeScale * initialFixedTimeStep;

            yield return null;
        }

        ClearCoroutine();

        yield break;
    }



    public static void LerpToTimescaleWithTime(MonoBehaviour scriptCalling, float newTimeScale, float timeToReachTimeScale)
    {
        ClearCoroutine();

        previousScriptCalling = scriptCalling;
        lerpToTimeScaleCoroutine = scriptCalling.StartCoroutine(LerpingToTimeScaleWithTime(newTimeScale, timeToReachTimeScale));
    }
    private static IEnumerator LerpingToTimeScaleWithTime(float newTimeScale, float timeToReachTimeScale)
    {
        float timer = 0f;

        while (timer < timeToReachTimeScale || Time.timeScale != newTimeScale)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, newTimeScale, (timer / timeToReachTimeScale));
            Time.fixedDeltaTime = Time.timeScale * initialFixedTimeStep;

            timer += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        ClearCoroutine();

        yield break;
    }
}
