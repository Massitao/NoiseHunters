using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class QuestType : MonoBehaviour
{
    [Header("Quest Descriptions")]
    public string questName;
    [Tooltip("This ID will return translated text")]
    public string objectiveID;
    [Tooltip("Only serves as guiding text so you can know which quest is which.")]
    [Multiline]
    public string questDescription;
    [HideInInspector] public Color questMarkerColor;

    [HideInInspector] public bool questActive;
    [HideInInspector] public bool questCompleted;

    [HideInInspector] public List<Transform> objectivesTransform;
    public List<bool> objectiveTaskCompletion;
    public Action<QuestType, int, string, bool> OnUpdateProgress;
    public Action<QuestType> OnComplete;

    [Space(10)]

    [Tooltip("Scene Events on this quest complete")]
    public UnityEvent OnQuestComplete;


    public abstract string GetInitialProgress();
    public abstract void UpdateProgress(int objectiveDoneIndex);
    protected virtual void QuestCompleted()
    {
        OnComplete?.Invoke(this);
        OnQuestComplete?.Invoke();
    }
}