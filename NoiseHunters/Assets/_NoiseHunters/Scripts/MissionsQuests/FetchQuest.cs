using System.Collections.Generic;
using UnityEngine;

public class FetchQuest : QuestType
{
    [Header("Fetch Quest Values")]
    public List<PickableProp> pickablesDepending;


    private void Awake()
    {
        for (int i = 0; i < pickablesDepending.Count; i++)
        {
            pickablesDepending[i].OnPickUpProp += PickedObject;

            if (pickablesDepending[i].pickableGameObject != null)
            {
                objectivesTransform.Add(pickablesDepending[i].pickableGameObject.transform);
                objectiveTaskCompletion.Add(false);
            }
            else
            {
                Debug.Log($"{pickablesDepending[i]} pickable GO hasn't been set! Quest won't work!", pickablesDepending[i]);
                break;
            }
        }

        if (ColorUtility.TryParseHtmlString("#FFA500", out Color newColor))
        {
            questMarkerColor = newColor;
        }
    }


    public override string GetInitialProgress()
    {
        int progress = 0;
        AllObjectsPicked(ref progress);

        return $"— {progress}/{pickablesDepending.Count}";
    }

    private void PickedObject(PickableProp pickedObject)
    {
        for (int i = 0; i < objectivesTransform.Count; i++)
        {
            if (pickedObject.pickableGameObject.transform == objectivesTransform[i])
            {
                UpdateProgress(i);
                break;
            }
        }
    }

    private bool AllObjectsPicked(ref int pickedObjects)
    {
        int allPickableProps = pickablesDepending.Count;
        bool allPicked = true;

        for (int i = 0; i < pickablesDepending.Count; i++)
        {
            if (!pickablesDepending[i].picked)
            {
                allPicked = false;
                allPickableProps--;
            }
        }
        pickedObjects = allPickableProps;

        return allPicked;
    }

    public override void UpdateProgress(int objectiveDoneIndex)
    {
        if (!questCompleted)
        {
            int progress = 0;
            bool allActivated = AllObjectsPicked(ref progress);

            objectiveTaskCompletion[objectiveDoneIndex] = true;
            ProgressUpdate(objectiveDoneIndex, progress, allActivated);

            if (allActivated)
            {
                questCompleted = true;

                QuestCompleted();
            }
        }
    }


    public void ProgressUpdate(int index, int progress, bool complete) => OnUpdateProgress?.Invoke(this, index, $"— {progress}/{pickablesDepending.Count}", complete);
}
