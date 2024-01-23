using System.Collections;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    private ThirdPersonController controller;
    private LMLink lmLink;

    [SerializeField] private float newTimeScale = 0.25f;
    [SerializeField] private float timeToLoadNewScene = 1f;


    private void Awake()
    {
        controller = FindObjectOfType<ThirdPersonController>();
        lmLink = GetComponent<LMLink>();
    }


    public void LoadSceneTriggered()
    {
        StartCoroutine(LoadSceneCoroutine());
    }
    private IEnumerator LoadSceneCoroutine()
    {
        controller.OnDisable();

        TimescaleHandle.LerpToTimescaleWithTime(this, newTimeScale, timeToLoadNewScene);
        while (Time.timeScale != newTimeScale)
        {
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        lmLink.ChangeLevel();

        SceneCompletedAchievementCall();

        yield break;
    }

    private void SceneCompletedAchievementCall()
    {
        if (_LevelManager._Instance != null && SteamManager._Instance != null)
        {
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage1_1_Scene)
            {
                SteamManager._Instance.Stage1_1_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage1_2_Scene)
            {
                SteamManager._Instance.Stage1_2_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage1_3_Scene)
            {
                SteamManager._Instance.Stage1_3_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage1_4_Scene)
            {
                SteamManager._Instance.Stage1_4_Complete();
            }

            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage2_1_Scene)
            {
                SteamManager._Instance.Stage2_1_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage2_2_Scene)
            {
                SteamManager._Instance.Stage2_2_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage2_3_Scene)
            {
                SteamManager._Instance.Stage2_3_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage2_4_Scene)
            {
                SteamManager._Instance.Stage2_4_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage2_5_Scene)
            {
                SteamManager._Instance.Stage2_5_Complete();
            }
            if (_LevelManager._Instance.GetCurrentLevelName() == LevelList.Stage2_6_Scene)
            {
                SteamManager._Instance.Stage2_6_Complete();
            }
        }
    }
}