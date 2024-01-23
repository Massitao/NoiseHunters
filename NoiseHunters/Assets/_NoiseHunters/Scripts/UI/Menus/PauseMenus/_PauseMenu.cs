using UnityEngine;

public class _PauseMenu : Menu
{
    [Header("Menus")]
    [SerializeField] private RestartMenu restartMenu;
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private InputMenu inputMenu;
    [SerializeField] private ReturnToMainMenuMenu returnToMainMenuMenu;
    [SerializeField] private ExitGameMenu exitGameMenu;

    [Header("Pause Menu Animator")]
    [SerializeField] private string animatorAccessMenu;
    [SerializeField] private string animatorExitMenu;
    private Animator animator;


    protected override void GetMenuComponents()
    {
        base.GetMenuComponents();
        animator = GetComponent<Animator>();
    }


    public override void AccessMenu()
    {
        base.AccessMenu();
        animator.SetTrigger(animatorAccessMenu);
    }

    public override void ExitMenu()
    {
        base.ExitMenu();
        manager.GetComponent<PauseMenuManager>().ResumeGame();
        animator.SetTrigger(animatorExitMenu);
    }

    public void ResumeButton()
    {
        ExitMenu();
    }
    public void RestartButton()
    {
        restartMenu.AccessMenu();
    }
    public void OptionsButton()
    {
        optionsMenu.AccessMenu();
    }
    public void InputButton()
    {
        inputMenu.AccessMenu();
    }
    public void ReturnToMainMenuButton()
    {
        returnToMainMenuMenu.AccessMenu();
    }
    public void ExitGameButton()
    {
        exitGameMenu.AccessMenu();
    }
}
