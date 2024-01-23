using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManagerUI : MonoBehaviour
{
    #region Components
    [Header("Mission Manager Instance")]
    public static MissionManagerUI Instance;


    [Header("Mission Manager Components")]
    [HideInInspector] public CompassHUD compassHUD;
    private Animator animator;
    private SoundSpeaker uiSpeaker;
    #endregion

    #region State
    [Header("Mission Manager State")]
    private bool showingUI;
    private bool onUpdatingObjectives;
    #endregion

    #region Animator
    [Header("Mission Manager Animator Values")]
    [SerializeField] private string missionUI_BoolShowHide;
    #endregion

    #region Intro Scene Text
    [Header("Intro Scene Text")]
    [SerializeField] private Text introSceneText;
    [SerializeField] private string animator_HideIntroText;
    #endregion

    #region Mission Texts
    [Header("Mission UI Texts")]
    [SerializeField] private Text missionText;

    [SerializeField] private Text intermediateGoalText;

    [SerializeField] private List<ObjectiveElementUI> objectivesText;
    [SerializeField] private List<Objective> objectivesGiven;

    [Space(10)]

    [SerializeField] private GameObject objectiveTextPrefab;
    [SerializeField] private GameObject objectiveMarkerPrefab;


    [Header("Mission Decode Strings")]
    [SerializeField] private List<string> randomDecryptCharacters;
    private const string consoleBlock = "█";
    private const string consoleStripped = "▒";
    private const string consoleSpace = " ";


    [Header("Mission Decode Values")]
    [SerializeField] private float msStartDelayTime;
    [SerializeField] private float msAwaitingInputTime;
    [SerializeField] private float msEndingInputTime;
    [SerializeField] private float msTypeTime;

    [Space(5)]

    [SerializeField] private int msTimesToRepeatAwaitInput;
    [SerializeField] private int msTimesToRepeatEndInput;
    [SerializeField] private int msTimesToRepeatDecodeType;

    [Space(5)]

    [SerializeField] private int msAwaitEraseRepetitions;
    [SerializeField] private float msAwaitEraseTime;
    [SerializeField] private float msEraseTime;


    [Header("Intermediate Goal and Objective Decode Values")]
    [SerializeField] private float igStartDelayTime;
    [SerializeField] private float igAwaitingInputTime;
    [SerializeField] private float igEndingInputTime;
    [SerializeField] private float igTypeTime;

    [Space(5)]

    [SerializeField] private int igTimesToRepeatAwaitInput;
    [SerializeField] private int igTimesToRepeatEndInput;
    [SerializeField] private int igTimesToRepeatDecodeType;

    [Space(5)]

    [SerializeField] private int igAwaitEraseRepetitions;
    [SerializeField] private float igAwaitEraseTime;
    [SerializeField] private float igEraseTime;
    #endregion

    #region Sounds
    [Header("Console Sounds")]
    [SerializeField] private SoundInfo typeSound;
    [SerializeField] private SoundInfo eraseSound;
    #endregion

    [Header("Mission UI Coroutines")]
    private Queue<IEnumerator> missionUI_QueueCoroutines = new Queue<IEnumerator>();



    private void Awake()
    {
        SetInstance();
        GetComponents();
        EmptyTexts();
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    private void GetComponents()
    {
        animator = GetComponent<Animator>();
        uiSpeaker = gameObject.AddComponent<SoundSpeaker>();
    }
    private void EmptyTexts()
    {
        introSceneText.text = string.Empty;
        missionText.text = string.Empty;
        intermediateGoalText.text = string.Empty;
    }

    private void Start()
    {
        showingUI = true;
        animator.SetBool(missionUI_BoolShowHide, showingUI);

        if (MissionManager.Instance != null)
        {
            StartCoroutine(DoIntroScene());
            StartCoroutine(UpdateMissionUI());
        }
    }

    private IEnumerator DoIntroScene()
    {
        string textToSet = LanguageManager._Instance.GetText(MissionManager.Instance.sceneNameID);
        textToSet = (textToSet != string.Empty) ? textToSet : MissionManager.Instance.sceneName;

        yield return Decrypt(introSceneText, textToSet, msStartDelayTime, msTimesToRepeatAwaitInput, msAwaitingInputTime, msTimesToRepeatDecodeType, msTypeTime, msTimesToRepeatEndInput, msEndingInputTime, true);
        animator.SetTrigger(animator_HideIntroText);

        compassHUD.ShowCompass();
        MissionManager.Instance.Initialize();

        yield break;
    }

    private IEnumerator UpdateMissionUI()
    {
        while (true)
        {
            while(missionUI_QueueCoroutines.Count > 0)
            {
                if (!onUpdatingObjectives)
                {
                    onUpdatingObjectives = true;
                    showingUI = true;
                    animator.SetBool(missionUI_BoolShowHide, showingUI);
                }


                yield return missionUI_QueueCoroutines.Dequeue();
            }

            onUpdatingObjectives = false;
            yield return null;
        }
    }



    public void ShowHideMissionPanel()
    {
        if (!onUpdatingObjectives)
        {
            showingUI = !showingUI;
            animator.SetBool(missionUI_BoolShowHide, showingUI);
        }
    }



    public void SetMissionText(Mission missionToSetText)
    {
        missionUI_QueueCoroutines.Enqueue(SetMissionTextBehaviour(missionToSetText));
    }
    private IEnumerator SetMissionTextBehaviour(Mission missionToSetText)
    {
        string textToSet = LanguageManager._Instance.GetText(missionToSetText.missionID);
        textToSet = (textToSet != string.Empty) ? textToSet : missionToSetText.name;

        yield return Decrypt(missionText, textToSet, msStartDelayTime, msTimesToRepeatAwaitInput, msAwaitingInputTime, msTimesToRepeatDecodeType, msTypeTime, msTimesToRepeatEndInput, msEndingInputTime, true);

        yield break;
    }

    public void MissionComplete(Mission missionCompleted)
    {
        MissionCompleteBehaviour(missionCompleted);
    }
    private IEnumerator MissionCompleteBehaviour(Mission missionToSetText)
    {
        string textToSet = LanguageManager._Instance.GetText(missionToSetText.missionID);
        textToSet = (textToSet != string.Empty) ? textToSet : missionToSetText.name;

        yield return EraseInfo(missionText, textToSet, msAwaitEraseTime, msAwaitEraseRepetitions, msEraseTime);

        yield break;
    }


    public void SetIntermediateGoalText(IntermediateGoal igToSetText)
    {
        missionUI_QueueCoroutines.Enqueue(SetIntermediateGoalTextBehaviour(igToSetText));
        SetObjectivesText(igToSetText.objectives);
    }
    private IEnumerator SetIntermediateGoalTextBehaviour(IntermediateGoal igToSetText)
    {
        string textToSet = LanguageManager._Instance.GetText(igToSetText.intermediateGoalID);
        textToSet = (textToSet != string.Empty) ? textToSet : igToSetText.name;

        yield return Decrypt(intermediateGoalText, textToSet, igStartDelayTime, igTimesToRepeatAwaitInput, igAwaitingInputTime, igTimesToRepeatDecodeType, igTypeTime, igEndingInputTime, igEndingInputTime, true);

        yield return null;
    }

    public void IntermediateGoalComplete(IntermediateGoal intermediateGoalCompleted)
    {
        missionUI_QueueCoroutines.Enqueue(IntermediateGoalCompleteBehaviour(intermediateGoalCompleted));
    }
    private IEnumerator IntermediateGoalCompleteBehaviour(IntermediateGoal igToSetText)
    {
        string textToSet = LanguageManager._Instance.GetText(igToSetText.intermediateGoalID);
        textToSet = (textToSet != string.Empty) ? textToSet : igToSetText.name;

        yield return EraseInfo(intermediateGoalText, textToSet, igAwaitEraseTime, igAwaitEraseRepetitions, igEraseTime);

        yield break;
    }




    public void SetObjectivesText(List<Objective> objectivesToSet)
    {
        missionUI_QueueCoroutines.Enqueue(SetObjectiveTextBehaviour(objectivesToSet));
    }
    private IEnumerator SetObjectiveTextBehaviour(List<Objective> objectivesToSet)
    {
        for (int i = 0; i < objectivesText.Count; i++)
        {
            Destroy(objectivesText[i].gameObject);
        }

        objectivesText.Clear();
        objectivesGiven.Clear();

        objectivesGiven = objectivesToSet;

        for (int j = 0; j < objectivesToSet.Count; j++)
        {
            string textToSet = LanguageManager._Instance.GetText(objectivesToSet[j].objective.objectiveID);
            textToSet = (textToSet != string.Empty) ? textToSet : objectivesToSet[j].objective.questName;

            string objectiveInfo = $"{textToSet} {objectivesToSet[j].objectiveProgress}";

            objectivesText.Add(Instantiate(objectiveTextPrefab, intermediateGoalText.gameObject.transform).GetComponent<ObjectiveElementUI>());

            objectivesText[j].SetImageColor(objectivesToSet[j].objective.questMarkerColor);


            for (int k = 0; k < objectivesToSet[j].objectivePositions.Count; k++)
            {
                if (!objectivesToSet[j].objectiveTaskCompletion[k])
                {
                    compassHUD.AddMarker(objectiveMarkerPrefab, objectivesToSet[j], k);
                }
            }

            if ((j + 1) < objectivesToSet.Count)
            {
                StartCoroutine(Decrypt(objectivesText[j].objectiveText, objectiveInfo, igStartDelayTime, igTimesToRepeatAwaitInput, igAwaitingInputTime, igTimesToRepeatDecodeType, igTypeTime, igTimesToRepeatEndInput, igEndingInputTime, false));
            }
            else
            {
                yield return
                Decrypt(objectivesText[j].objectiveText, objectiveInfo, igStartDelayTime, igTimesToRepeatAwaitInput, igAwaitingInputTime, igTimesToRepeatDecodeType, igTypeTime, igTimesToRepeatEndInput, igEndingInputTime, true);
            }
        }

        yield break;
    }

    public void UpdateObjective(Objective givenObjective, int objectiveCompleted)
    {
        for (int i = 0; i < objectivesGiven.Count; i++)
        {
            if (givenObjective.objective == objectivesGiven[i].objective)
            {
                string textToSet = LanguageManager._Instance.GetText(givenObjective.objective.objectiveID);
                textToSet = (textToSet != string.Empty) ? textToSet : givenObjective.objective.questName;

                objectivesText[i].SetTextContent($"{textToSet} {givenObjective.objectiveProgress}");
                objectivesText[i].ObjectiveProgress();

                compassHUD.RemoveMarker(givenObjective, objectiveCompleted);

                break;
            }
        }
    }

    public void ObjectiveCompleted(Objective givenObjective)
    {
        missionUI_QueueCoroutines.Enqueue(ObjectiveCompletedBehaviour(givenObjective));
    }
    private IEnumerator ObjectiveCompletedBehaviour(Objective givenObjective)
    {
        for (int i = 0; i < objectivesGiven.Count; i++)
        {
            if (givenObjective.objective == objectivesGiven[i].objective)
            {
                yield return EraseInfo(objectivesText[i].objectiveText, objectivesText[i].objectiveText.text, igAwaitEraseTime, igAwaitEraseRepetitions, igEraseTime);
                objectivesText[i].ObjectiveCompleted();
                objectivesText.Remove(objectivesText[i]);
                objectivesGiven.Remove(objectivesGiven[i]);

                break;
            }
        }

        yield break;
    }


    private IEnumerator Decrypt(Text textToChange, string textToSet, float startDelay, int awaitInputRepetitions, float awaitInputTime, int decodeRepetitions, float typingTime, float endInputRepetitions, float endInputTime, bool playAudio)
    {
        textToChange.text = "";
        yield return new WaitForSeconds(startDelay);

        int timesRepeated = 0;
        while (timesRepeated < awaitInputRepetitions)
        {
            timesRepeated++;

            textToChange.text = consoleBlock;
            if (playAudio) PlayDigitalTextSound();
            yield return new WaitForSeconds(awaitInputTime);

            textToChange.text = consoleSpace;
            yield return new WaitForSeconds(awaitInputTime);
        }

        yield return null;


        string textProgress = "";
        for (int i = 0; i < textToSet.Length; i++)
        {
            char previousChar = '_';
            char newChar = '_';
            int randomRange = 0;

            for (int j = 0; j < decodeRepetitions; j++)
            {
                while (previousChar == newChar)
                {
                    randomRange = Random.Range(0, randomDecryptCharacters.Count);
                    newChar = randomDecryptCharacters[randomRange].ToCharArray()[0];
                }
                previousChar = newChar;

                textToChange.text = textProgress + newChar + consoleStripped;
                yield return new WaitForSeconds(typingTime);
            }

            textProgress += textToSet[i].ToString();
            textToChange.text = textProgress + consoleBlock;
            if (playAudio) PlayDigitalTextSound();
            yield return new WaitForSeconds(typingTime);
        }

        timesRepeated = 0;
        while (timesRepeated < endInputRepetitions)
        {
            timesRepeated++;

            textToChange.text = textProgress + " " + consoleBlock;
            if (playAudio) PlayDigitalTextSound();
            yield return new WaitForSeconds(endInputTime);

            textToChange.text = textProgress + " " + consoleSpace;
            yield return new WaitForSeconds(endInputTime);
        }

        textToChange.text = textProgress;

        yield break;
    }
    private IEnumerator EraseInfo(Text textToChange, string initialText, float awaitingEraseTime, int awaitRepetitions, float eraseTime)
    {

        int timesRepeated = 0;
        while (timesRepeated < awaitRepetitions)
        {
            timesRepeated++;

            textToChange.text = initialText + consoleBlock;
            PlayDigitalTextSound();
            yield return new WaitForSeconds(awaitingEraseTime);

            textToChange.text = initialText + consoleSpace;
            yield return new WaitForSeconds(awaitingEraseTime);
        }

        yield return null;

        string textProgress = initialText;
        for (int i = 0; i < initialText.Length; i++)
        {
            textProgress = textProgress.Remove(textProgress.Length - 1);
            textToChange.text = textProgress + consoleBlock;
            PlayEraseTextSound();
            yield return new WaitForSeconds(eraseTime);
        }

        textToChange.text = "";

        yield break;
    }


    private void PlayDigitalTextSound()
    {
        if (typeSound != null)
        {
            uiSpeaker.CreateSoundBubble(typeSound, null, gameObject, true);
        }
    }
    private void PlayEraseTextSound()
    {
        if (typeSound != null)
        {
            uiSpeaker.CreateSoundBubble(eraseSound, null, gameObject, true);
        }
    }
}