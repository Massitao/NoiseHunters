using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class SteamManager : PersistentSingleton<SteamManager>
{
    protected override void Awake()
    {
        base.Awake();

        try
        {
            SteamClient.Init(1303440, true);
            
            Debug.Log($"Welcome {SteamClient.Name}! Your ID is {SteamClient.SteamId}.");
        }
        catch
        {
            Debug.Log($"Client, dll or permission missing!");
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
    }

    private void OnEnable()
    {
        // SteamUserStats.OnUserStatsStored += DebugUserStatsSaved;
        // SteamUserStats.OnUserStatsReceived += DebugUserStatsReceived;
        // SteamUserStats.OnAchievementProgress += DebugAchievementChanged;
    }
    private void OnDisable()
    {
        // SteamUserStats.OnUserStatsStored -= DebugUserStatsSaved;
        // SteamUserStats.OnUserStatsReceived -= DebugUserStatsReceived;
        // SteamUserStats.OnAchievementProgress -= DebugAchievementChanged;
        ExitSteamClient();
    }


    private void Update()
    {
        SteamClient.RunCallbacks();
    }


    [ContextMenu("Reset Progress")]
    private void ResetStatProgress()
    {
        if (SteamClient.IsValid)
        {
            SteamUserStats.ResetAll(true);
        }
    }
    [ContextMenu("Shutdown")]
    private void ExitSteamClient()
    {
        SteamClient.Shutdown();
    }

    public string GetClientUsername()
    {
        return SteamClient.Name;
    }


    public void UnlockAchievement(string achievementToUnlockName)
    {
        if (SteamClient.IsValid)
        {
            Achievement ach = new Achievement(achievementToUnlockName);
            Debug.Log($"{ach.State}");

            // If achievement is locked, unlock
            if (!ach.State)
            {
                Debug.Log($"Achievement {ach.Name} is unlocked: {ach.Trigger()}");
            }
        }
        else
        {
            Debug.Log($"Client is invalid!");
        }
    }


    [ContextMenu("Achievement Test: Stage 1.1 Complete")]
    public void Stage1_1_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_1_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 1.2 Complete")]
    public void Stage1_2_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_2_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 1.3 Complete")]
    public void Stage1_3_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_3_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 1.4 Complete")]
    public void Stage1_4_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_4_COMPLETE");
            CompletedAllStages();
        }
    }

    [ContextMenu("Achievement Test: Stage 2.1 Complete")]
    public void Stage2_1_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_1_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 2.2 Complete")]
    public void Stage2_2_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_2_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 2.3 Complete")]
    public void Stage2_3_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_3_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 2.4 Complete")]
    public void Stage2_4_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_4_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 2.5 Complete")]
    public void Stage2_5_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_5_COMPLETE");
            CompletedAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage 2.6 Complete")]
    public void Stage2_6_Complete()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_6_COMPLETE");
            CompletedAllStages();
        }
    }


    [ContextMenu("Achievement Test: Stage1.1 Collectibles Picked")]
    public void Stage1_1_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_1_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage1.2 Collectibles Picked")]
    public void Stage1_2_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_2_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage1.3 Collectibles Picked")]
    public void Stage1_3_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_3_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage1.4 Collectibles Picked")]
    public void Stage1_4_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE1_4_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }

    [ContextMenu("Achievement Test: Stage2.1 Collectibles Picked")]
    public void Stage2_1_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_1_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage2.2 Collectibles Picked")]
    public void Stage2_2_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_2_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage2.3 Collectibles Picked")]
    public void Stage2_3_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_3_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage2.4 Collectibles Picked")]
    public void Stage2_4_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_4_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage2.5 Collectibles Picked")]
    public void Stage2_5_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_5_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }
    [ContextMenu("Achievement Test: Stage2.6 Collectibles Picked")]
    public void Stage2_6_Collected()
    {
        if (SteamClient.IsValid)
        {
            UnlockAchievement("STAGE2_6_COLLECTED_INFO");
            CollectedInfoFromAllStages();
        }
    }


    [ContextMenu("Achievement Test: Enemy Killed")]
    public void EnemyKilled()
    {
        if (SteamClient.IsValid)
        {
            int enemiesKilled = SteamUserStats.GetStatInt("ENEMIES_KILLED") + 1;

            if (enemiesKilled == 1)
            {
                UnlockAchievement("ENEMY_KILLED_FIRST");
            }
            else if (enemiesKilled == 10)
            {
                UnlockAchievement("ENEMY_KILLED_10");
            }
            else if (enemiesKilled == 50)
            {
                UnlockAchievement("ENEMY_KILLED_50");
                CompletionistAchievement();
            }
            else
            {
                Debug.Log(enemiesKilled);
            }

            //PopUps
            if (enemiesKilled == 20 || enemiesKilled == 30 || enemiesKilled == 40 || enemiesKilled == 45)
            {
                Debug.Log(SteamUserStats.IndicateAchievementProgress("ENEMY_KILLED_50", enemiesKilled, 50));
            }

            SteamUserStats.SetStat("ENEMIES_KILLED", enemiesKilled);
        }
    }


    private void CompletedAllStages()
    {
        if (SteamClient.IsValid)
        {
            Achievement ach = new Achievement("STAGE1_1_COMPLETE");
            Achievement ach1 = new Achievement("STAGE1_2_COMPLETE");
            Achievement ach2 = new Achievement("STAGE1_3_COMPLETE");
            Achievement ach3 = new Achievement("STAGE1_4_COMPLETE");
            Achievement ach4 = new Achievement("STAGE2_1_COMPLETE");
            Achievement ach5 = new Achievement("STAGE2_2_COMPLETE");
            Achievement ach6 = new Achievement("STAGE2_3_COMPLETE");
            Achievement ach7 = new Achievement("STAGE2_4_COMPLETE");
            Achievement ach8 = new Achievement("STAGE2_5_COMPLETE");
            Achievement ach9 = new Achievement("STAGE2_6_COMPLETE");

            if (ach.State && ach1.State && ach2.State && ach3.State && ach4.State && ach5.State && ach6.State && ach7.State && ach.State && ach8.State && ach9.State)
            {
                UnlockAchievement("COMPLETED_STAGES");
                CompletionistAchievement();
            }
            else
            {
                Debug.Log("Not yet");
            }
        }
    }
    private void CollectedInfoFromAllStages()
    {
        if (SteamClient.IsValid)
        {
            Achievement ach = new Achievement("STAGE1_1_COLLECTED_INFO");
            Achievement ach1 = new Achievement("STAGE1_2_COLLECTED_INFO");
            Achievement ach2 = new Achievement("STAGE1_3_COLLECTED_INFO");
            Achievement ach3 = new Achievement("STAGE1_4_COLLECTED_INFO");
            Achievement ach4 = new Achievement("STAGE2_1_COLLECTED_INFO");
            Achievement ach5 = new Achievement("STAGE2_2_COLLECTED_INFO");
            Achievement ach6 = new Achievement("STAGE2_3_COLLECTED_INFO");
            Achievement ach7 = new Achievement("STAGE2_4_COLLECTED_INFO");
            Achievement ach8 = new Achievement("STAGE2_5_COLLECTED_INFO");
            Achievement ach9 = new Achievement("STAGE2_6_COLLECTED_INFO");

            if (ach.State && ach1.State && ach2.State && ach3.State && ach4.State && ach5.State && ach6.State && ach7.State && ach.State && ach8.State && ach9.State)
            {
                UnlockAchievement("COLLECTED_ALL_INFO");
                CompletionistAchievement();
            }
            else
            {
                Debug.Log("Not yet");
            }
        }
    }
    private void CompletionistAchievement()
    {
        if (SteamClient.IsValid)
        {
            List<Achievement> allAchievements = new List<Achievement>(SteamUserStats.Achievements);
            bool allUnlocked = true;

            for (int i = 0; i < allAchievements.Count; i++)
            {
                if (allAchievements[i].Identifier == "NOISE_HUNTER")
                {
                    continue;
                }
                else
                {
                    if (!allAchievements[i].State)
                    {
                        allUnlocked = false;
                        break;
                    }
                }
            }

            if (allUnlocked)
            {
                UnlockAchievement("NOISE_HUNTER");
            }
            else
            {
                Debug.Log("Not every achievement has been unlocked");
            }
        }
    }


    #region Debug Stuff
    /*  private void DebugUserStatsSaved(Result result)
    {
        Debug.Log($"Guardar datos con resultado {result}");

        foreach (Achievement a in SteamUserStats.Achievements)
        {
            Debug.Log($"{a.Name} is {a.State}");
        }
    }

    private void DebugUserStatsReceived(SteamId id, Result result) //Debugeo cuando carga stats desde Steam
    {
        Debug.Log($"{id} da resultado {result}");
    }
    private void DebugAchievementChanged(Achievement ach, int currentProgress, int progress)
    {
        if (ach.State)
        {
            Debug.Log($"{ach.Name} WAS UNLOCKED!");
        }
    }
    */
    #endregion
}