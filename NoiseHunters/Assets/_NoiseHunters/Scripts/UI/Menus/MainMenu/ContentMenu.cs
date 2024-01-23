using UnityEngine;
using UnityEngine.UI;

public class ContentMenu : Menu
{
    [Header("Content Menu Animator")]
    [SerializeField] private string animatorAccessMenu;
    [SerializeField] private string animatorExitMenu;
    private Animator animator;

    [Header("Content Menu Values")]
    [SerializeField] private Button firstCinematicButton;
    [SerializeField] private Button secondCinematicButton;
    [SerializeField] private Button thirdCinematicButton;


    protected override void GetMenuComponents()
    {
        base.GetMenuComponents();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        if (SaveInstance._Instance.currentLoadedSave.unlockedCinematics.Contains(LevelList.FirstCinematic_Scene))
        {
            defaultElement = firstCinematicButton.gameObject;
        }
    }

    public void FirstCinematicButton()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.HopeCinematic_Scene, true);
    }
    public void SecondCinematicButton()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.RequestCinematic_Scene, true);
    }
    public void ThirdCinematicButton()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.FailureCinematic_Scene, true);
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

    public override void ButtonActive(bool active)
    {
        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].GetThisButton().interactable = active;
        }

        if (active)
        {
            if (!SaveInstance._Instance.currentLoadedSave.unlockedCinematics.Contains(LevelList.FirstCinematic_Scene))
            {
                firstCinematicButton.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedCinematics.Contains(LevelList.SecondCinematic_Scene))
            {
                secondCinematicButton.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedCinematics.Contains(LevelList.ThirdCinematic_Scene))
            {
                thirdCinematicButton.interactable = false;
            }
        }
    }
}