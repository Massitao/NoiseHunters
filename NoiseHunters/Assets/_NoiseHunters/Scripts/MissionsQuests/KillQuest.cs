using System.Collections.Generic;
using UnityEngine;

public class KillQuest : QuestType
{
    [Header("Kill Quest Values")]
    public List<LivingEntity> enemiesToKill;


    private void Awake()
    {
        for (int i = 0; i < enemiesToKill.Count; i++)
        {
            enemiesToKill[i].OnEntityDeath += EnemyKilled;
            objectivesTransform.Add(enemiesToKill[i].transform);
            objectiveTaskCompletion.Add(false);
        }

        if (ColorUtility.TryParseHtmlString("#FF0000", out Color newColor))
        {
            questMarkerColor = newColor;
        }
    }


    public override string GetInitialProgress()
    {
        int progress = 0;
        AllDead(ref progress);

        return $"— {progress}/{enemiesToKill.Count}";
    }

    private void EnemyKilled(LivingEntity enemyKilled)
    {
        for (int i = 0; i < objectivesTransform.Count; i++)
        {
            if (enemyKilled.transform == objectivesTransform[i])
            {
                UpdateProgress(i);
                break;
            }
        }
    }

    public override void UpdateProgress(int objectiveDoneIndex)
    {
        if (!questCompleted)
        {
            int progress = 0;
            bool allDead = AllDead(ref progress);

            objectiveTaskCompletion[objectiveDoneIndex] = true;
            ProgressUpdate(objectiveDoneIndex, progress, allDead);

            if (allDead)
            {
                questCompleted = true;

                QuestCompleted();
            }
        }
    }


    public void ProgressUpdate(int index, int progress, bool complete) => OnUpdateProgress?.Invoke(this, index, $"— {progress}/{enemiesToKill.Count}", complete);

    private bool AllDead(ref int enemiesKilled)
    {
        int ek = enemiesToKill.Count;
        bool allDead = true;

        for (int i = 0; i < enemiesToKill.Count; i++)
        {
            if (enemiesToKill[i].IsAlive)
            {
                allDead = false;
                ek--;
            }
        }
        enemiesKilled = ek;

        return allDead;
    }
}