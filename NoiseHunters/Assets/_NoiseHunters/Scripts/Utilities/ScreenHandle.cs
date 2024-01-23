using UnityEngine;

public class ScreenHandle
{
    public static bool CheckScreenResolution(Resolution resolutionToCompare, bool windowedMode)
    {
        return (Screen.currentResolution.ToString() == resolutionToCompare.ToString() && Screen.fullScreen == windowedMode);
    }

    public static void SetNewResolution(int width, int height, bool windowedMode)
    {
        Screen.SetResolution(width, height, windowedMode);
    }
}