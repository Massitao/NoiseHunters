using UnityEngine;

public class ReturnToMainMenuMenu : Menu
{
    [Header("Return to Main Menu Menu Animator")]
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
        animator.SetTrigger(animatorExitMenu);
    }

    public void BackToMenu()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.MainMenu_Scene, false);
    }
}
