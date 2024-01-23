using System.Collections.Generic;
using UnityEngine;

public class RestoreGeneratorQuest : QuestType
{
    [Header("Restore Generator Quest Values")]
    public List<GeneratorProp> generatorsDepending;


    private void Awake()
    {
        for (int i = 0; i < generatorsDepending.Count; i++)
        {
            generatorsDepending[i].OnActiveStateChange += GeneratorActivated;
            objectivesTransform.Add(generatorsDepending[i].transform);
            objectiveTaskCompletion.Add(false);
        }

        if (ColorUtility.TryParseHtmlString("#00FFB4", out Color newColor))
        {
            questMarkerColor = newColor;
        }
        else
        {
            Debug.Log(2);
        }
    }


    public override string GetInitialProgress()
    {
        int progress = 0;
        AllGeneratorsActivated(ref progress);

        return $"— {progress}/{generatorsDepending.Count}";
    }

    private void GeneratorActivated(GeneratorProp generatorActivated)
    {
        for (int i = 0; i < objectivesTransform.Count; i++)
        {
            if (generatorActivated.transform == objectivesTransform[i])
            {
                UpdateProgress(i);
                break;
            }
        }
    }

    private bool AllGeneratorsActivated(ref int activatedGens)
    {
        int allGeneratorsActivated = generatorsDepending.Count;
        bool allActivated = true;

        for (int i = 0; i < generatorsDepending.Count; i++)
        {
            objectiveTaskCompletion[i] = generatorsDepending[i].IsActive;
            if (!generatorsDepending[i].IsActive)
            {
                allActivated = false;
                allGeneratorsActivated--;
            }
        }
        activatedGens = allGeneratorsActivated;

        return allActivated;
    }

    public override void UpdateProgress(int objectiveDoneIndex)
    {
        if (!questCompleted)
        {
            int progress = 0;
            bool allActivated = AllGeneratorsActivated(ref progress);

            ProgressUpdate(objectiveDoneIndex, progress, allActivated);

            if (allActivated)
            {
                questCompleted = true;

                QuestCompleted();
            }
        }
    }


    public void ProgressUpdate(int index, int progress, bool complete) => OnUpdateProgress?.Invoke(this, index, $"— {progress}/{generatorsDepending.Count}", complete);
}
