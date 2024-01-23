using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _LevelManager : PersistentSingleton<_LevelManager>
{
    [Header("Transition")]
    [SerializeField] private float loadDelay;
    private SceneTransition sceneTransition;

    [Header("Level Load Operation")]
    private AsyncOperation loadSceneOperation;
    private Coroutine loadSceneCoroutine;

    // EVENTS
    public event Action OnSceneChange;
    public event Action<string, string> OnSceneChange_StringContext;
    public event Action<Scene, Scene> OnSceneChange_SceneContext;



    // Singleton start
    protected override void Awake()
    {
        base.Awake();

        sceneTransition = GetComponentInChildren<SceneTransition>();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneChanged;
    }
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneChanged;
    }


    // Will try to change the current Scene to input "levelToChange" Scene if it exists. Will directly load scene if "directLoad" is marked true.
    public void LevelChange(string levelToChange, bool directLoad)
    {
        if (loadSceneCoroutine == null)
        {
            loadSceneCoroutine = StartCoroutine(LevelChangeCoroutine(levelToChange, directLoad));
        }
    }

    // Async Level Loading - Will load a "Loading Scene" and wait until new level has been loaded
    IEnumerator LevelChangeCoroutine(string levelToChange, bool directLoad)
    {
        // Trigger Transition
        sceneTransition.TriggerTransitionEnter();
        yield return new WaitUntil(() => sceneTransition.doTransition);
        sceneTransition.doTransition = false;
        Time.timeScale = 1f;

        if (!directLoad)
        {
            // Load "Loading Scene"
            loadSceneOperation = SceneManager.LoadSceneAsync(LevelList.Loading_Scene, LoadSceneMode.Single);
            loadSceneOperation.allowSceneActivation = false;

            // Wait for Scene Load
            while (loadSceneOperation.progress < 0.9f)
            {
                yield return null;
            }

            // Scene Loaded - Wait for Seconds before activating scene
            yield return new WaitForSecondsRealtime(loadDelay);

            // Activate Scene
            loadSceneOperation.allowSceneActivation = true;

            // Trigger Load Transition Exit
            sceneTransition.TriggerTransitionExit_LoadScene();
        }


        // Load Wanted Scene
        loadSceneOperation = SceneManager.LoadSceneAsync(levelToChange, LoadSceneMode.Single);
        loadSceneOperation.allowSceneActivation = false;

        // Wait for Scene Load
        while (loadSceneOperation.progress < 0.9f)
        {
            yield return null;
        }

        if (!directLoad)
        {
            // Scene Loaded - Wait for Seconds before activating scene
            yield return new WaitForSecondsRealtime(3f);


            // Trigger Transition
            sceneTransition.TriggerTransitionEnter();
            yield return new WaitUntil(() => sceneTransition.doTransition);
            sceneTransition.doTransition = false;
        }

        // Activate Scene
        loadSceneOperation.allowSceneActivation = true;

        // Trigger Other Scene Transition Exit
        sceneTransition.TriggerTransitionExit_OtherScene();


        // Stop this coroutine
        loadSceneCoroutine = null;

        yield break;
    }



    // Calls LevelChange, giving the current active scene
    public void ResetScene()
    {
        LevelChange(SceneManager.GetActiveScene().name, true);
    }

    // Returns current scene name
    public string GetCurrentLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // Checks if Scene Exists by checking the name given.
    public bool DoesSceneExist(string levelToCheck)
    {
        return SceneManager.GetSceneByName(levelToCheck).IsValid();
    }

    // Scene Change Event
    public void SceneChanged(Scene current, Scene next)
    {
        OnSceneChange?.Invoke();
        OnSceneChange_StringContext?.Invoke(current.name, next.name);
        OnSceneChange_SceneContext?.Invoke(current, next);
    }
}
