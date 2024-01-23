using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    #region Audio Manager Variables
    [Header("AudioSources")]
    private AudioSource musicSource;

    [Header("Volume")]
    public float musicVolume;
    public float soundVolume;

    [Header("Default AudioManager Values")]
    public float defaultTimeToFade = 1.5f;

    // EVENTS
    public event Action<float> musicVolumeChanged;
    public event Action<float> soundVolumeChanged;
    public event Action soundVolumeReset;

    // COROUTINES
    private Coroutine musicChangeTrackCoroutine;
    #endregion


    protected override void Awake()
    {
        base.Awake();

        musicSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        musicVolume = SaveInstance._Instance.currentLoadedConfig.userMusicVolume;
        soundVolume = SaveInstance._Instance.currentLoadedConfig.userSoundVolume;

        _LevelManager._Instance.OnSceneChange += SceneChanged_LinkSoundSources;
        _LevelManager._Instance.OnSceneChange_StringContext += SceneChanged_BackgroundMusicChange;
    }
    private void OnDisable()
    {
        _LevelManager._Instance.OnSceneChange -= SceneChanged_LinkSoundSources;
        _LevelManager._Instance.OnSceneChange_StringContext -= SceneChanged_BackgroundMusicChange;
    }

    private void Start()
    {
        musicSource.volume = musicVolume;
    }


    #region Music Source Methods
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    public void ChangeMusicVolume(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
        musicSource.volume = musicVolume;

        musicVolumeChanged?.Invoke(newMusicVolume);
    }
    private void ChangeMusicSourceVolume(float newMusicVolume)
    {
        musicSource.volume = newMusicVolume;
    }

    public void ChangeMusicTrack(AudioClip newClip, bool direct, bool loop, float? timeToFade)
    {
        if (musicChangeTrackCoroutine != null)
        {
            StopCoroutine(musicChangeTrackCoroutine);
        }

        if (direct)
        {
            SetDirectMusicTrack(newClip, loop);
        }
        else
        {
            float trueTimeToFade = (timeToFade != null) ? (float) timeToFade : defaultTimeToFade;

            musicChangeTrackCoroutine = StartCoroutine(ChangeMusicTrack_FadeToTrack(newClip, trueTimeToFade, loop));
        }
    }
    public void SetDirectMusicTrack(AudioClip newClip, bool loop)
    {
        if (newClip != null)
        {
            musicSource.clip = newClip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
    }

    public IEnumerator ChangeMusicTrack_FadeToTrack(AudioClip newClip, float timeToFade, bool loop)
    {
        if (musicSource.clip != null)
        {
            if (musicSource.volume != 0f)
            {
                yield return MusicFadeBehaviour(0f, timeToFade);
            }
        }

        SetDirectMusicTrack(newClip, loop);

        if (newClip != null)
        {
            yield return MusicFadeBehaviour(musicVolume, timeToFade);
        }
        else
        {
            ChangeMusicSourceVolume(musicVolume);
        }
    }
    private IEnumerator MusicFadeBehaviour(float newVolume, float timeToFade)
    {
        float currentVolume = musicSource.volume;

        float lerp = 0;

        float timer = Time.time;
        float time = timeToFade * currentVolume;


        while (lerp != 1f)
        {
            lerp = Mathf.InverseLerp(timer, timeToFade + timer, Time.time);
            ChangeMusicSourceVolume(Mathf.Round(Mathf.Lerp(currentVolume, newVolume, lerp) * 100) / 100);
           
            yield return null;
        }

        ChangeMusicSourceVolume(newVolume);

        yield break;
    }
    #endregion

    #region Sound Sources Methods
    public void ChangeSoundSourcesVolume(float newSoundsVolume)
    {
        soundVolume = newSoundsVolume;
        soundVolumeChanged?.Invoke(newSoundsVolume);
    }
    public void ResetSounds()
    {
        soundVolumeReset?.Invoke();
    }

    private void LinkSoundSources()
    {
        List<AudioSource> soundSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());

        if (soundSources.Contains(musicSource))
        {
            soundSources.Remove(musicSource);
        }

        for (int i = 0; i < soundSources.Count; i++)
        {
            if (soundSources[i].GetComponent<AMLink>() == null)
            {
                soundSources[i].gameObject.AddComponent<AMLink>();
            }
        }
    }
    #endregion

    #region Scene Change Methods
    private void SceneChanged_BackgroundMusicChange(string currentScene, string nextScene)
    {
        if (nextScene == LevelList.MainMenu_Scene)
        {
            ChangeMusicTrack(AudioList.ReturnMusicClip(AudioList.MainMenu_Music), false, true, defaultTimeToFade);
        }
        else
        {
            ChangeMusicTrack(null, false, true, defaultTimeToFade);
        }
    }
    private void SceneChanged_LinkSoundSources()
    {
        LinkSoundSources();
    }
    #endregion
}