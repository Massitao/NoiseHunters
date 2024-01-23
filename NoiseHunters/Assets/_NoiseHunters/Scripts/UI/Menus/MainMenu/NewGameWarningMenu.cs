using UnityEngine;

public class NewGameWarningMenu : Menu
{
    [Header("New Game Warning Menu Animator")]
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

    public void DoNewGame()
    {
        ButtonActive(false);
        SaveInstance._Instance.ResetSave();
        _LevelManager._Instance.LevelChange(LevelList.FirstCinematic_Scene, false);
    }
}
