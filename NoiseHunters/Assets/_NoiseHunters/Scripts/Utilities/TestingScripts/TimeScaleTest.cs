using UnityEngine;

public class TimeScaleTest : MonoBehaviour
{
    public float timescale;

    private void Start()
    {
        TimescaleHandle.SetTimeScale(timescale);
    }
}