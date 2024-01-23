using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu : Menu
{
    [Header("Level Select Menu Animator")]
    [SerializeField] private string animatorAccessMenu;
    [SerializeField] private string animatorExitMenu;
    private Animator animator;

    [Header("Level Select Buttons")]
    [SerializeField] private Button stage1_1Button;
    [SerializeField] private Button stage1_2Button;
    [SerializeField] private Button stage1_3Button;
    [SerializeField] private Button stage1_4Button;

    [SerializeField] private Button stage2_1Button;
    [SerializeField] private Button stage2_2Button;
    [SerializeField] private Button stage2_3Button;
    [SerializeField] private Button stage2_4Button;
    [SerializeField] private Button stage2_5Button;
    [SerializeField] private Button stage2_6Button;

    [SerializeField] private Button backToMenuButton;


    protected override void GetMenuComponents()
    {
        base.GetMenuComponents();
        animator = GetComponent<Animator>();

        if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage1_1_Scene))
        {
            defaultElement = backToMenuButton.gameObject;
        }
    }

    protected override void Start()
    {
        base.Start();
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


    public void LoadScene_1_1()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage1_1_Scene, false);
    }
    public void LoadScene_1_2()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage1_2_Scene, false);
    }
    public void LoadScene_1_3()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage1_3_Scene, false);
    }
    public void LoadScene_1_4()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage1_4_Scene, false);
    }
    public void LoadScene_2_1()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage2_1_Scene, false);
    }
    public void LoadScene_2_2()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage2_2_Scene, false);
    }
    public void LoadScene_2_3()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage2_3_Scene, false);
    }
    public void LoadScene_2_4()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage2_4_Scene, false);
    }
    public void LoadScene_2_5()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage2_5_Scene, false);
    }
    public void LoadScene_2_6()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Stage2_6_Scene, false);
    }



    public override void ButtonActive(bool active)
    {
        base.ButtonActive(active);

        if (active)
        {
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage1_1_Scene))
            {
                stage1_1Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage1_2_Scene))
            {
                stage1_2Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage1_3_Scene))
            {
                stage1_3Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage1_4_Scene))
            {
                stage1_4Button.interactable = false;
            }


            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage2_1_Scene))
            {
                stage2_1Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage2_2_Scene))
            {
                stage2_2Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage2_3_Scene))
            {
                stage2_3Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage2_4_Scene))
            {
                stage2_4Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage2_5_Scene))
            {
                stage2_5Button.interactable = false;
            }
            if (!SaveInstance._Instance.currentLoadedSave.unlockedLevels.Contains(LevelList.Stage2_6_Scene))
            {
                stage2_6Button.interactable = false;
            }
        }
    }
}
