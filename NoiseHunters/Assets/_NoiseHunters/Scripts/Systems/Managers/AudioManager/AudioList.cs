using UnityEngine;

public class AudioList : MonoBehaviour
{
    public static string MainMenu_Music = "MainMenu_BackgroundMusic";

    public static AudioClip ReturnMusicClip(string audioClipToReturn)
    {
        AudioClip musicToReturn = Resources.Load(audioClipToReturn) as AudioClip;

        return musicToReturn;
    }
}