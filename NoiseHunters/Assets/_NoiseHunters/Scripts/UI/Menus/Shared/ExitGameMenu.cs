using UnityEngine;

public class ExitGameMenu : Menu
{
    [Header("Exit Game Menu Animator")]
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
