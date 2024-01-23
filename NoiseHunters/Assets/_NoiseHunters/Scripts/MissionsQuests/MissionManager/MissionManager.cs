using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MissionManager : TemporalSingleton<MissionManager>
{
    #region Mission Manager Values
    #region Mission Manager Important Values
    [Header("Mission Manager List")]
    public string sceneName;
    public string sceneNameID;
    [SerializeField] private bool startFirstMissionOnStart = true;
    public List<Mission> sceneMissions;

    [Space(20)]

    [Header("Do not mind the lists below. Debug Stuff.")]
    public Mission currentMission;
    private int currentMissionIndex = 0;

    public IntermediateGoal currentIntermediateGoal;
    private int currentIntermediateGoalIndex = 0;
    #endregion

    #region Mission Manager Events
    // Mission Events
    public event Action sceneCompleted;

    public event Action<Mission> OnMissionStarted;
    public event Action<Mission> OnMissionCompleted;

    public event Action<IntermediateGoal> OnIntermediateGoalStarted;
    public event Action<IntermediateGoal> OnIntermediateGoalCompleted;
    #endregion
    #endregion


    #region Start
    private void Start()
    {
        SetEventsAndRepeatChecker();
    }

    private void SetEventsAndRepeatChecker()
    {
        List<QuestType> questTypesDetected = new List<QuestType>();
        List<Objective> repeatedObjectives = new List<Objective>();

        #region Missions Loop
        for (int i = 0; i < sceneMissions.Count; i++)
        {
            #region Intermediate Goals Loop
            for (int j = 0; j < sceneMissions[i].intermediateGoals.Count; j++)
            {
                #region Objectives Loop
                for (int k = 0; k < sceneMissions[i].intermediateGoals[j].objectives.Count; k++)
                {
                    if (RepeatedQuestsFound(ref questTypesDetected, ref repeatedObjectives, sceneMissions[i].intermediateGoals[j].objectives[k]))
                    {
                        // Set Objectives Transform
                        sceneMissions[i].intermediateGoals[j].objectives[k].objectivePositions = sceneMissions[i].intermediateGoals[j].objectives[k].objective.objectivesTransform;
                        sceneMissions[i].intermediateGoals[j].objectives[k].objectiveTaskCompletion = sceneMissions[i].intermediateGoals[j].objectives[k].objective.objectiveTaskCompletion;

                        // Set initial objective progress
                        sceneMissions[i].intermediateGoals[j].objectives[k].objectiveProgress = sceneMissions[i].intermediateGoals[j].objectives[k].objective.GetInitialProgress();
                    }
                    else
                    {
                        Debug.LogError($"Same <color=blue>{sceneMissions[i].intermediateGoals[j].objectives[k].objective.questName}</color> in the same <color=red>Intermediate Goal</color> is repeated!. Removing Quest from list.");
                    }
                }
                #endregion

                RepeatedQuestsRemoval(sceneMissions[i].intermediateGoals[j], repeatedObjectives);
                questTypesDetected.Clear();
                repeatedObjectives.Clear();
            }
            #endregion
        }
        #endregion
    }
    private bool RepeatedQuestsFound(ref List<QuestType> questsToCheck, ref List<Objective> repeatedObjectivesList, Objective checkingObjective)
    {
        for (int i = 0; i < questsToCheck.Count; i++)
        {
            if (checkingObjective.objective == questsToCheck[i])
            {
                repeatedObjectivesList.Add(checkingObjective);
                return false;
            }
        }

        questsToCheck.Add(checkingObjective.objective);

        return true;
    }
    private void RepeatedQuestsRemoval(IntermediateGoal igToRemoveRepeatedQuests, List<Objective> repeatedObjectives)
    {
        for (int i = 0; i < repeatedObjectives.Count; i++)
        {
            igToRemoveRepeatedQuests.objectives.Remove(repeatedObjectives[i]);
        }
    }
    public void Initialize()
    {
        SetMission(currentMissionIndex);
    }
    #endregion


    #region Mission Methods
    void SetMission(int index)
    {
        currentMission = sceneMissions[index];

        OnMissionStarted += MissionManagerUI.Instance.SetMissionText;
        OnMissionCompleted += MissionManagerUI.Instance.MissionComplete;

        OnMissionStarted?.Invoke(currentMission);
        SetIntermediateGoal(currentIntermediateGoalIndex);
    }
    void SetMissionComplete()
    {
        currentMission.missionCompleted = true;
        currentMission.OnThisMissionComplete?.Invoke();
        OnMissionCompleted?.Invoke(currentMission);
    }


    void MissionComplete()
    {
        SetMissionComplete();

        OnMissionStarted -= MissionManagerUI.Instance.SetMissionText;
        OnMissionCompleted -= MissionManagerUI.Instance.MissionComplete;

        if (currentMissionIndex + 1 < sceneMissions.Count)
        {
            if (currentMission.progressToNextMissionOnComplete)
            {
                NextMission();
            }          
        }
        else
        {
            if (currentMission.progressToNextMissionOnComplete)
            {
                SceneCompleted();
            }               
        }
    }
    public void NextMission()
    {
        if (currentMission.missionCompleted)
        {
            currentMissionIndex++;

            currentIntermediateGoalIndex = 0;
            SetMission(currentMissionIndex);
        }
    }


    public void SceneCompleted()
    {
        sceneCompleted?.Invoke();
    }
    #endregion

    #region Intermediate Goals Methods
    void SetIntermediateGoal(int index)
    {
        currentIntermediateGoal = currentMission.intermediateGoals[index];

        OnIntermediateGoalStarted += MissionManagerUI.Instance.SetIntermediateGoalText;
        OnIntermediateGoalCompleted += MissionManagerUI.Instance.IntermediateGoalComplete;

        for (int i = 0; i < currentIntermediateGoal.objectives.Count; i++)
        {
            currentIntermediateGoal.objectives[i].objective.questActive = true;

            // Suscribe to Objective Events such as Progress and Completion
            currentIntermediateGoal.objectives[i].objective.OnUpdateProgress += ObjectiveProgress;
            currentIntermediateGoal.objectives[i].objective.OnComplete += ObjectiveComplete;
        }

        OnIntermediateGoalStarted?.Invoke(currentIntermediateGoal);
    }
    void SetIntermediateGoalComplete()
    {
        currentIntermediateGoal.intermediateGoalCompleted = true;
        currentIntermediateGoal.OnThisIntermediateGoalComplete?.Invoke();
        OnIntermediateGoalCompleted?.Invoke(currentIntermediateGoal);

        for (int i = 0; i < currentIntermediateGoal.objectives.Count; i++)
        {
            currentIntermediateGoal.objectives[i].objective.questActive = false;

            // Suscribe to Objective Events such as Progress and Completion
            currentIntermediateGoal.objectives[i].objective.OnUpdateProgress -= ObjectiveProgress;
            currentIntermediateGoal.objectives[i].objective.OnComplete -= ObjectiveComplete;
        }
    }


    void IntermediateGoalComplete()
    {
        SetIntermediateGoalComplete();

        OnIntermediateGoalStarted -= MissionManagerUI.Instance.SetIntermediateGoalText;
        OnIntermediateGoalCompleted -= MissionManagerUI.Instance.IntermediateGoalComplete;

        if (currentIntermediateGoalIndex + 1 < currentMission.intermediateGoals.Count)
        {
            if (currentIntermediateGoal.progressToNextIntermediateGoalOnComplete)
            {
                NextIntermediateGoal();
            }
        }
        else
        {
            MissionComplete();
        }
    }
    public void NextIntermediateGoal()
    {
        if (currentIntermediateGoal.intermediateGoalCompleted)
        {
            currentIntermediateGoalIndex++;
            SetIntermediateGoal(currentIntermediateGoalIndex);
        }
    }
    #endregion

    #region Objective Methods  
    bool ContainsQuest(QuestType questExists, ref int index)
    {
        for (int i = 0; i < currentIntermediateGoal.objectives.Count; i++)
        {
            if (currentIntermediateGoal.objectives[i].objective == questExists)
            {
                index = i;
                return true;
            }
        }

        return false;
    }


    public void ObjectiveProgress(QuestType quest, int objectiveProgressedIndex, string progress, bool completed)
    {
        int index = 0;

        if (ContainsQuest(quest, ref index))
        {
            currentIntermediateGoal.objectives[index].objectiveProgress = progress;
            MissionManagerUI.Instance.UpdateObjective(currentIntermediateGoal.objectives[index], objectiveProgressedIndex);
        }
        else
        {
            Debug.Log($"Progressed unknown quest!");
        }
    }


    public void ObjectiveComplete(QuestType questCompleted)
    {
        int currentQuestIndex = 0;

        if (ContainsQuest(questCompleted, ref currentQuestIndex))
        {
            SetObjectiveComplete(currentQuestIndex);

            if (AllObjectivesCompleted())
            {
                IntermediateGoalComplete();
            }
            else
            {
                Debug.Log("Still not done.");
            }
        }
        else
        {
            Debug.LogError("Quest Completed but not in the current intermediate goal.");
        }
    }
    void SetObjectiveComplete(int currentQuestIndex)
    {
        currentIntermediateGoal.objectives[currentQuestIndex].objective.questActive = false;
        currentIntermediateGoal.objectives[currentQuestIndex].objectiveComplete = true;
        MissionManagerUI.Instance.ObjectiveCompleted(currentIntermediateGoal.objectives[currentQuestIndex]);
    }
    bool AllObjectivesCompleted()
    {
        for (int i = 0; i < currentIntermediateGoal.objectives.Count; i++)
        {
            if (!currentIntermediateGoal.objectives[i].objectiveComplete)
            {
                return false;
            }
        }

        return true;
    }
    #endregion
}


[Serializable]
public class Mission
{
    [Header("Mission")]
    [Tooltip("This name will be shown in the Mission List UI")]
    public string name;
    [Tooltip("This ID will return translated text")]
    public string missionID;
    [Multiline]
    [Tooltip("This description will NOT be shown in the Mission List UI. Its purpose is to guide you plan missions, so you can leave it blank if you want.")]
    public string missionDescription;

    [Space(20)]

    [HideInInspector] public bool missionCompleted;
    public bool progressToNextMissionOnComplete = true;
    public UnityEvent OnThisMissionComplete;


    [Space(10)]

    public List<IntermediateGoal> intermediateGoals;
}

[Serializable]
public class IntermediateGoal
{
    [Header("Intermediate Goal")]
    [Tooltip("This name will be shown in the Mission List UI")]
    public string name;
    [Tooltip("This ID will return translated text")]
    public string intermediateGoalID;
    [Multiline]
    [Tooltip("This description will NOT be shown in the Mission List UI. Its purpose is to guide you plan missions, so you can leave it blank if you want.")]
    public string intermediateGoalDescription;

    [Space(20)]

    [HideInInspector] public bool intermediateGoalCompleted;
    public bool progressToNextIntermediateGoalOnComplete = true;
    public UnityEvent OnThisIntermediateGoalComplete;

    [Space(10)]

    public List<Objective> objectives;
}

[Serializable]
public class Objective
{
    [Header("Objective")]
    [Tooltip("This name will NOT be used. It is only descriptive to help you name your objectives")]
    public string name;

    [HideInInspector] public string objectiveProgress;
    [HideInInspector] public bool objectiveComplete;
    [HideInInspector] public List<Transform> objectivePositions;
    [HideInInspector] public List<bool> objectiveTaskCompletion;

    public QuestType objective;

}