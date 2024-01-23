using UnityEngine;

public class MainMenuManager : MenuManager
{
    protected virtual void Start()
    {
        primaryMenu.AccessMenu();
        CursorHandle.CursorLockHandle(CursorLockMode.Confined, true);
        TimescaleHandle.SetTimeScale(1f);
    }
}
