using System;
using UnityEngine;

public class SaveInstance : PersistentSingleton<SaveInstance>
{
    public PlayerSave currentLoadedSave;
    public UserSave currentLoadedConfig;


    protected override void Awake()
    {
        base.Awake();

        LoadSave(SaveManager.LoadPlayerData());
        LoadConfig(SaveManager.LoadPlayerConfig());

        SetResolution(currentLoadedConfig.userResolution, currentLoadedConfig.userWindowedMode);
    }

    // Events
    private void OnEnable()
    {
        _LevelManager._Instance.OnSceneChange_StringContext += SetNewScene;
    }
    private void OnDisable()
    {
        _LevelManager._Instance.OnSceneChange_StringContext -= SetNewScene;
    }


    public void SaveData()
    {
        SaveManager.SavePlayerData(currentLoadedSave);
    }
    public void SaveConfig()
    {
        SetResolution(currentLoadedConfig.userResolution, currentLoadedConfig.userWindowedMode);

        AudioManager._Instance.ChangeMusicVolume(currentLoadedConfig.userMusicVolume);
        AudioManager._Instance.ChangeSoundSourcesVolume(currentLoadedConfig.userSoundVolume);

        LanguageManager._Instance.ChangeCurrentLanguage(currentLoadedConfig.userLanguage);

        SaveManager.SavePlayerConfig(currentLoadedConfig);
    }

    public void LoadSave(PlayerSave newSave)
    {
        currentLoadedSave = newSave;
    }
    public void LoadConfig(UserSave newSave)
    {
        currentLoadedConfig = newSave;
    }


    // Player Save
    public void SetNewScene(string currentScene, string nextScene)
    {
        if (LevelList.IsCurrentScenePlayable(nextScene))
        {
            bool newChanges = false;
            int lastScenePlayedIndex = LevelList.PlayableScene_List.IndexOf(currentLoadedSave.lastPlayedScene);
            int thisSceneIndex = LevelList.PlayableScene_List.IndexOf(nextScene);

            if (thisSceneIndex > lastScenePlayedIndex)
            {
                currentLoadedSave.lastPlayedScene = nextScene;
                newChanges = true;
            }
            if (!currentLoadedSave.unlockedLevels.Contains(nextScene))
            {
                currentLoadedSave.unlockedLevels.Add(nextScene);
                newChanges = true;
            }
            if (newChanges)
            {
                SaveData();
            }
        }
        else if (LevelList.IsSceneCinematic(nextScene))
        {
            if (nextScene == LevelList.FirstCinematic_Scene && !currentLoadedSave.unlockedCinematics.Contains(nextScene))
            {
                currentLoadedSave.unlockedCinematics.Add(nextScene);
                
                if (!currentLoadedSave.unlockedLevels.Contains(LevelList.Stage1_1_Scene))
                {
                    currentLoadedSave.unlockedLevels.Add(LevelList.Stage1_1_Scene);
                    currentLoadedSave.lastPlayedScene = LevelList.Stage1_1_Scene;
                }

                SaveData();
            }
            if (nextScene == LevelList.SecondCinematic_Scene && !currentLoadedSave.unlockedCinematics.Contains(nextScene))
            {
                currentLoadedSave.unlockedCinematics.Add(nextScene);
                
                if (!currentLoadedSave.unlockedLevels.Contains(LevelList.Stage2_1_Scene))
                {
                    currentLoadedSave.unlockedLevels.Add(LevelList.Stage2_1_Scene);
                    currentLoadedSave.lastPlayedScene = LevelList.Stage2_1_Scene;
                }

                SaveData();
            }
            if (nextScene == LevelList.ThirdCinematic_Scene && !currentLoadedSave.unlockedCinematics.Contains(nextScene))
            {
                currentLoadedSave.unlockedCinematics.Add(nextScene);

                SaveData();
            }
        }
    }
    public void AddNewCollectible(string collectibleID)
    {
        currentLoadedSave.collectiblesPicked.Add(collectibleID);
        SaveData();
    }


    // User Config
    public void SetResolution(Resolution newResolution, bool windowedMode)
    {
        currentLoadedConfig.userResolution = newResolution;
        currentLoadedConfig.userWindowedMode = windowedMode;

        if (!ScreenHandle.CheckScreenResolution(newResolution, !windowedMode))
        {
            ScreenHandle.SetNewResolution(newResolution.width, newResolution.height, !windowedMode);
        }
    }



    [ContextMenu("Cutscenes Unlock")]
    private void UnlockCutscenes()
    {
        if (!currentLoadedSave.unlockedCinematics.Contains(LevelList.FirstCinematic_Scene))
        {
            currentLoadedSave.unlockedCinematics.Add(LevelList.FirstCinematic_Scene);
        }
        if (!currentLoadedSave.unlockedCinematics.Contains(LevelList.SecondCinematic_Scene))
        {
            currentLoadedSave.unlockedCinematics.Add(LevelList.SecondCinematic_Scene);
        }
        if (!currentLoadedSave.unlockedCinematics.Contains(LevelList.ThirdCinematic_Scene))
        {
            currentLoadedSave.unlockedCinematics.Add(LevelList.ThirdCinematic_Scene);
        }

        SaveData();
    }

    [ContextMenu("Delete Data")]
    public void ResetSave()
    {
        SaveManager.DeletePlayerData();
    }

    [ContextMenu("Delete Config")]
    public void ResetConfig()
    {
        SaveManager.DeletePlayerConfig();
    }

    [ContextMenu("Music Full volume")]
    public void MusicFull()
    {
        currentLoadedConfig.userMusicVolume = 1f;
        SaveConfig();
    }
    [ContextMenu("Sounds Full volume")]
    public void SoundsFull()
    {
        currentLoadedConfig.userSoundVolume = 1f;
        SaveConfig();
    }
}
