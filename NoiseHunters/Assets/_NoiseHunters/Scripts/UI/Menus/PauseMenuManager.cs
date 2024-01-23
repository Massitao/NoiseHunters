using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MenuManager
{
    [Header("Character")]
    private ThirdPersonController character;

    protected override void Awake()
    {
        eventSystem = GetComponentInChildren<EventSystem>();
    }

    protected virtual void Start()
    {
        eventSystem = GetComponentInChildren<EventSystem>();
        character = FindObjectOfType<ThirdPersonController>();
        ResumeGame();
    }

    [ContextMenu("Pause")]
    public void PauseGame()
    {
        character.enabled = false;
        //eventSystem.enabled = true;
        primaryMenu.AccessMenu();
        CursorHandle.CursorLockHandle(CursorLockMode.Confined, true);
        TimescaleHandle.SetTimeScale(0f);
    }

    public void ResumeGame()
    {
        character.enabled = true;
        //eventSystem.enabled = false;
        CursorHandle.CursorLockHandle(CursorLockMode.Locked, false);
        TimescaleHandle.SetTimeScale(1f);
    }
}
