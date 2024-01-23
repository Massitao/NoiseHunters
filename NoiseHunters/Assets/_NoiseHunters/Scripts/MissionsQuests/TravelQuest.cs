using System;
using UnityEngine;

public class TravelQuest : QuestType
{
    [Header("Travel Quest Values")]
    [SerializeField] private OnTriggerEnterOverlap trigger;
    private ThirdPersonController player;

    private bool reachedPoint;
    private float _distance;
    private float distance
    {
        get { return _distance; }
        set
        {
            if (value != _distance)
            {
                _distance = value;
                UpdateProgress(-1);
            }
        }
    }

    private void Awake()
    {
        player = FindObjectOfType<ThirdPersonController>();
        objectivesTransform.Add(trigger.transform);
        objectiveTaskCompletion.Add(false);

        if (ColorUtility.TryParseHtmlString("#16B0CF", out Color newColor))
        {
            questMarkerColor = newColor;
        }
    }



    public void ReachedPoint()
    {
        if (questActive)
        {
            reachedPoint = true;
            UpdateProgress(0);
        }
    }


    public override string GetInitialProgress()
    {
        return $"";
    }

    public override void UpdateProgress(int objectiveDoneIndex)
    {
        if (!questCompleted)
        {
            string progress = GetInitialProgress();

            if (reachedPoint)
            {
                objectiveTaskCompletion[objectiveDoneIndex] = true;
                ProgressUpdate(objectiveDoneIndex, LanguageManager._Instance.GetText("ReachedObjectiveWarning"), reachedPoint);

                questCompleted = true;

                QuestCompleted();
            }
            else
            {
                ProgressUpdate(objectiveDoneIndex, progress, reachedPoint);
            }
        }
    }

    public void ProgressUpdate(int index, string progress, bool complete) => OnUpdateProgress?.Invoke(this, index, progress, complete);
}
