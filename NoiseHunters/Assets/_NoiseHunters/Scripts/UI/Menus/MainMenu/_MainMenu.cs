using UnityEngine;

public class _MainMenu : Menu
{
    [Header("All Menus")]
    [SerializeField] private NewGameWarningMenu newGameWarningMenu;
    [SerializeField] private ContentMenu contentMenu;
    [SerializeField] private LevelSelectMenu levelMenu;
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private InputMenu inputMenu;
    [SerializeField] private ExitGameMenu exitGameMenu;

    [Header("Main Menu Animator")]
    [SerializeField] private string animatorAccessMenu;
    private Animator animator;


    protected override void GetMenuComponents()
    {
        base.GetMenuComponents();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        canLeaveMenu = false;
    }



    public override void AccessMenu()
    {
        base.AccessMenu();

        animator.SetTrigger(animatorAccessMenu);
    }

    public void NewGameButton()
    {
        if (!LevelList.IsCurrentScenePlayable(SaveInstance._Instance.currentLoadedSave.lastPlayedScene))
        {
            ButtonActive(false);
            SaveInstance._Instance.ResetSave();
            _LevelManager._Instance.LevelChange(LevelList.FirstCinematic_Scene, false);
        }
        else
        {
            newGameWarningMenu.AccessMenu();
        }
    }
    public void ContinueButton()
    {
        ButtonActive(false);

        if (LevelList.IsCurrentScenePlayable(SaveInstance._Instance.currentLoadedSave.lastPlayedScene))
        {
            if (SaveInstance._Instance.currentLoadedSave.lastPlayedScene == LevelList.Stage1_1_Scene)
            {
                _LevelManager._Instance.LevelChange(LevelList.FirstCinematic_Scene, false);
            }
            else if (SaveInstance._Instance.currentLoadedSave.lastPlayedScene == LevelList.Stage2_1_Scene)
            {
                _LevelManager._Instance.LevelChange(LevelList.SecondCinematic_Scene, false);
            }
            else
            {
                _LevelManager._Instance.LevelChange(SaveInstance._Instance.currentLoadedSave.lastPlayedScene, false);
            }
        }
        else
        {
            NewGameButton();
        }
    }
    public void ContentButton()
    {
        contentMenu.AccessMenu();
    }
    public void LevelSelectButton()
    {
        levelMenu.AccessMenu();
    }
    public void OptionsButton()
    {
        optionsMenu.AccessMenu();
    }
    public void InputButton()
    {
        inputMenu.AccessMenu();
    }
    public void CreditsButton()
    {
        ButtonActive(false);
        _LevelManager._Instance.LevelChange(LevelList.Credits_Scene, true);
    }
    public void ExitApplicationButton()
    {
        exitGameMenu.AccessMenu();
    }

    public override void ButtonActive(bool active)
    {
        for (int i = 0; i < allButtons.Count; i++)
        {
            if (allButtons[i].TryGetComponent(out ContinueButtonUI continueButton))
            {
                if (continueButton.blocked && active)
                {
                    continue;
                }
            }

            allButtons[i].GetThisButton().interactable = active;
        }
    }
}
