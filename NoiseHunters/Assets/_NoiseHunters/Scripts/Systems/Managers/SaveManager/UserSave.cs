using UnityEngine;

[System.Serializable]
public class UserSave
{   
    // Save Version
    public string saveVersion;

    // Screen
    public Resolution userResolution;
    public bool userWindowedMode;

    // Game Language
    public GameLanguages userLanguage;

    // Game Volume
    [Range(0f, 1f)] public float userMusicVolume;
    [Range(0f, 1f)] public float userSoundVolume;

    // Camera Sensibility
    public float userSensibility;
    public float userAimingSensibility;

    // Wave Brightness
    public BrightnessEnum userWaveBrightness;
}
