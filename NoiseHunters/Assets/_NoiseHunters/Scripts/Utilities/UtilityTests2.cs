using UnityEngine;

public class UtilityTests : MonoBehaviour
{
    public GameObject playtestingCanvas;

    public void PlaytestingCompleted()
    {
        Debug.Log("Start Animation");
        playtestingCanvas.SetActive(true);
    }
}