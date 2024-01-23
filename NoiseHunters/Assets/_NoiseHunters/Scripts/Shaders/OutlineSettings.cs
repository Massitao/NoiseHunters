using UnityEngine;

[CreateAssetMenu(fileName = "New Outline Setting", menuName = "NoiseHunters/Outline/Outline Settings")]
public class OutlineSettings : ScriptableObject
{
    [Header("Ping Interaction")]
    public float outline_FullWidth;

    [Space(10)]

    public float outline_PingStartTime;
    public float outline_PingMidTime;
    public float outline_PingEndTime;

    [Space(10)]

    public AnimationCurve outline_PingStartCurve;
    public AnimationCurve outline_PingEndCurve;
}
