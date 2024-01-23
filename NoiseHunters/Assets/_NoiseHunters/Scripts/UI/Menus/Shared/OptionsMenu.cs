using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Menu
{
    [Header("Options Menu Animator")]
    [SerializeField] private string animatorAccessMenu;
    [SerializeField] private string animatorExitMenu;
    private Animator animator;


    [Header("Options Menu State")]
    [SerializeField] private bool unappliedChanges;
    private bool isGameLevel;


    [Header("Video Panel")]
    [SerializeField] private Text videoResolutionText;
    [SerializeField] private Slider videoResolutionSlider;
    [SerializeField] private Toggle videoWindowedModeToggle;

    public List<Resolution> allResolutions = new List<Resolution>();
    private int currentIndexResolution;

    private bool isWindowedMode;


    [Header("Audio Panel")]
    [SerializeField] private Slider audioMusicSlider;
    private float currentMusicVolume;

    [SerializeField] private Slider audioSoundSlider;
    private float currentSoundsVolume;


    [Header("Game Panel")]
    [SerializeField] private Slider gameBrightnessSlider;
    [SerializeField] private Text gameBrightnessText;
    [SerializeField] private TextTranslation gameBrightnessTextTranslation;

    [SerializeField] private List<string> gameBrightnessTextTranslations;
    private int currentGameBrightness;

    [Space(5)]

    [SerializeField] private Slider gameSensibilitySlider;
    private float currentGameSensibility;
    [SerializeField] private Slider gameAimingSensibilitySlider;
    private float currentAimingGameSensibility;

    [Space(10)]

    [SerializeField] private GameLanguages currentLanguage;
    [SerializeField] private Text languageInGameWarning;
    [SerializeField] private ColorBlock languageButtonColorBlock;
    [SerializeField] private Color selectedLanguageColor;

    [SerializeField] private Button currentButtonLanguage;
    [SerializeField] private Button englishButton;
    [SerializeField] private Button spanishButton;
    [SerializeField] private Button portugueseButton;
    [SerializeField] private Button frenchButton;
    [SerializeField] private Button germanButton;
    [SerializeField] private Button russianButton;


    [Header("Other Buttons")]
    [SerializeField] private Button applyChangesButton;
    [SerializeField] private Button returnToMenuButton;    


    protected override void GetMenuComponents()
    {
        base.GetMenuComponents();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        GetOptionValues();
    }
    protected virtual void GetOptionValues()
    {
        GetVideoResolution();
        GetGameLanguage();
        ResetChanges();
    }
    protected virtual void GetVideoResolution()
    {
        string initialResolution = $"{Screen.currentResolution.width} x {Screen.currentResolution.height}";
        string loadedResolution = $"{SaveInstance._Instance.currentLoadedConfig.userResolution.width} x {SaveInstance._Instance.currentLoadedConfig.userResolution.height}";

        allResolutions.AddRange(Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct());

        if ((initialResolution != loadedResolution) || Screen.fullScreen != SaveInstance._Instance.currentLoadedConfig.userWindowedMode)
        {
            SaveInstance._Instance.SetResolution(Screen.currentResolution, SaveInstance._Instance.currentLoadedConfig.userWindowedMode);
            SaveInstance._Instance.SaveConfig();
        }

        for (int i = 0; i < allResolutions.Count; i++)
        {
            string currentLoadedScreen = $"{SaveInstance._Instance.currentLoadedConfig.userResolution.width} x {SaveInstance._Instance.currentLoadedConfig.userResolution.height}";
            string indexString = $"{allResolutions[i].width} x {allResolutions[i].height}";

            if (currentLoadedScreen == indexString)
            {
                currentIndexResolution = i;
                break;
            }
        }

        videoResolutionSlider.maxValue = allResolutions.Count - 1;
        videoResolutionText.text = allResolutions[currentIndexResolution].ToString();
    }
    protected virtual void GetGameLanguage()
    {
        currentLanguage = SaveInstance._Instance.currentLoadedConfig.userLanguage;
        currentButtonLanguage = ReturnLanguageButton();
        SetSelectedColor_LanguageButton(currentButtonLanguage);

        isGameLevel = LevelList.PlayableScene_List.Contains(_LevelManager._Instance.GetCurrentLevelName());
        languageInGameWarning.enabled = isGameLevel;

        if (isGameLevel)
        {
            BlockLanguageButtons();
        }
        else
        {
            SetNewLanguageNavigation();
        }        
    }


    #region Video Methods
    public virtual void SetResolutionIndex()
    {
        currentIndexResolution = (int)videoResolutionSlider.value;
        videoResolutionText.text = $"{allResolutions[currentIndexResolution].width} x {allResolutions[currentIndexResolution].height}";

        CheckChanges();
    }
    public virtual void SetWindowedMode()
    {
        isWindowedMode = videoWindowedModeToggle.isOn;

        CheckChanges();
    }
    #endregion

    #region Audio Methods
    public virtual void SetMusicVolume()
    {
        currentMusicVolume = audioMusicSlider.value;

        CheckChanges();
    }
    public virtual void SetSoundsVolume()
    {
        currentSoundsVolume = audioSoundSlider.value;

        CheckChanges();
    }
    #endregion

    #region Game Methods
    public virtual void SetBrightness()
    {
        currentGameBrightness = (int)gameBrightnessSlider.value;
        gameBrightnessTextTranslation.textKey = gameBrightnessTextTranslations[currentGameBrightness];
        gameBrightnessText.text = LanguageManager._Instance.GetText(gameBrightnessTextTranslations[currentGameBrightness]);


        CheckChanges();
    }
    public virtual void SetSensibility()
    {
        currentGameSensibility = gameSensibilitySlider.value;

        CheckChanges();
    }
    public virtual void SetAimingSensibility()
    {
        currentAimingGameSensibility = gameAimingSensibilitySlider.value;

        CheckChanges();
    }
    #endregion

    #region Language Methods
    protected virtual Button ReturnLanguageButton()
    {
        switch (currentLanguage)
        {
            case GameLanguages.ENG:
                return englishButton;

            case GameLanguages.ESP:
                return spanishButton;

            case GameLanguages.PT:
                return portugueseButton;

            case GameLanguages.FR:
                return frenchButton;

            case GameLanguages.DE:
                return germanButton;

            case GameLanguages.RUS:
                return russianButton;
        }

        return null;
    }

    protected virtual void SetSelectedColor_LanguageButton(Button selectedButton)
    {
        ColorBlock selectedColorBlock = languageButtonColorBlock;
        selectedColorBlock.normalColor = selectedLanguageColor;
        selectedButton.colors = selectedColorBlock;
    }
    protected virtual void SetUnselectedColor_LanguageButton(Button unselectedButton)
    {
        unselectedButton.colors = languageButtonColorBlock;
    }

    public virtual void SetEnglishLanguage()
    {
        if (currentLanguage != GameLanguages.ENG)
        {
            currentLanguage = GameLanguages.ENG;
            ChangeLanguageButton(englishButton);
            LanguageManager._Instance.ChangeCurrentLanguage(currentLanguage);

            SaveInstance._Instance.currentLoadedConfig.userLanguage = currentLanguage;
            SaveInstance._Instance.SaveConfig();
        }
    }
    public virtual void SetSpanishLanguage()
    {
        if (currentLanguage != GameLanguages.ESP)
        {
            currentLanguage = GameLanguages.ESP;
            ChangeLanguageButton(spanishButton);
            LanguageManager._Instance.ChangeCurrentLanguage(currentLanguage);

            SaveInstance._Instance.currentLoadedConfig.userLanguage = currentLanguage;
            SaveInstance._Instance.SaveConfig();
        }
    }
    public virtual void SetPortugueseLanguage()
    {
        if (currentLanguage != GameLanguages.PT)
        {
            currentLanguage = GameLanguages.PT;
            ChangeLanguageButton(portugueseButton);
            LanguageManager._Instance.ChangeCurrentLanguage(currentLanguage);

            SaveInstance._Instance.currentLoadedConfig.userLanguage = currentLanguage;
            SaveInstance._Instance.SaveConfig();
        }
    }
    public virtual void SetFrenchLanguage()
    {
        if (currentLanguage != GameLanguages.FR)
        {
            currentLanguage = GameLanguages.FR;
            ChangeLanguageButton(frenchButton);
            LanguageManager._Instance.ChangeCurrentLanguage(currentLanguage);

            SaveInstance._Instance.currentLoadedConfig.userLanguage = currentLanguage;
            SaveInstance._Instance.SaveConfig();
        }
    }
    public virtual void SetGermanLanguage()
    {
        if (currentLanguage != GameLanguages.DE)
        {
            currentLanguage = GameLanguages.DE;
            ChangeLanguageButton(germanButton);
            LanguageManager._Instance.ChangeCurrentLanguage(currentLanguage);

            SaveInstance._Instance.currentLoadedConfig.userLanguage = currentLanguage;
            SaveInstance._Instance.SaveConfig();
        }
    }
    public virtual void SetRussianLanguage()
    {
        if (currentLanguage != GameLanguages.RUS)
        {
            currentLanguage = GameLanguages.RUS;
            ChangeLanguageButton(russianButton);
            LanguageManager._Instance.ChangeCurrentLanguage(currentLanguage);

            SaveInstance._Instance.currentLoadedConfig.userLanguage = currentLanguage;
            SaveInstance._Instance.SaveConfig();
        }
    }

    protected virtual void ChangeLanguageButton(Button languageButton)
    {
        SetSelectedColor_LanguageButton(languageButton);
        if (currentButtonLanguage != null)
        {
            SetUnselectedColor_LanguageButton(currentButtonLanguage);
        }

        currentButtonLanguage = languageButton;
        SetNewLanguageNavigation();
    }
    protected virtual void BlockLanguageButtons()
    {
        Navigation newNavigation = new Navigation();
        newNavigation.mode = Navigation.Mode.Explicit;

        // New Aiming Sensibility Slider Navigation
        newNavigation.selectOnUp = gameSensibilitySlider;
        newNavigation.selectOnDown = returnToMenuButton;
        newNavigation.selectOnLeft = null;
        newNavigation.selectOnRight = null;
        gameAimingSensibilitySlider.navigation = newNavigation;

        // New Apply Changes Button Navigation
        newNavigation.selectOnUp = gameAimingSensibilitySlider;
        newNavigation.selectOnDown = null;
        newNavigation.selectOnLeft = null;
        newNavigation.selectOnRight = returnToMenuButton;
        applyChangesButton.navigation = newNavigation;

        // New Return to Menu Button Navigation
        newNavigation.selectOnUp = gameAimingSensibilitySlider;
        newNavigation.selectOnDown = null;
        newNavigation.selectOnLeft = applyChangesButton;
        newNavigation.selectOnRight = null;
        returnToMenuButton.navigation = newNavigation;
    }
    protected virtual void SetNewLanguageNavigation()
    {
        Navigation newNavigation = new Navigation();
        newNavigation.mode = Navigation.Mode.Explicit;

        // New Aiming Sensibility Slider Navigation
        newNavigation.selectOnUp = gameSensibilitySlider;
        newNavigation.selectOnDown = currentButtonLanguage;
        newNavigation.selectOnLeft = null;
        newNavigation.selectOnRight = null;
        gameAimingSensibilitySlider.navigation = newNavigation;

        // New Apply Changes Button Navigation
        newNavigation.selectOnUp = currentButtonLanguage;
        newNavigation.selectOnDown = null;
        newNavigation.selectOnLeft = null;
        newNavigation.selectOnRight = returnToMenuButton;
        applyChangesButton.navigation = newNavigation;

        // New Return to Menu Button Navigation
        newNavigation.selectOnUp = currentButtonLanguage;
        newNavigation.selectOnDown = null;
        newNavigation.selectOnLeft = applyChangesButton;
        newNavigation.selectOnRight = null;
        returnToMenuButton.navigation = newNavigation;
    }
    #endregion


    protected virtual void CheckChanges()
    {
        unappliedChanges = ChangeChecker();
        SetApplyButtonActive(unappliedChanges);
    }
    protected virtual bool ChangeChecker()
    {
        bool changesMade = false;

        if ($"{allResolutions[currentIndexResolution].width} x {allResolutions[currentIndexResolution].height}" != $"{SaveInstance._Instance.currentLoadedConfig.userResolution.width} x {SaveInstance._Instance.currentLoadedConfig.userResolution.height}")
        {
            //Debug.Log("res diff");
            changesMade = true;
        }
        if (isWindowedMode != SaveInstance._Instance.currentLoadedConfig.userWindowedMode)
        {
            //Debug.Log("wind diff");
            changesMade = true;
        }
        if (currentMusicVolume != SaveInstance._Instance.currentLoadedConfig.userMusicVolume)
        {
            //Debug.Log("music diff");
            changesMade = true;
        }
        if (currentSoundsVolume != SaveInstance._Instance.currentLoadedConfig.userSoundVolume)
        {
            //Debug.Log("sounds diff");
            changesMade = true;
        }
        if (currentGameSensibility != SaveInstance._Instance.currentLoadedConfig.userSensibility)
        {
            //Debug.Log("sensi diff");
            changesMade = true;
        }
        if (currentAimingGameSensibility != SaveInstance._Instance.currentLoadedConfig.userAimingSensibility)
        {
            //Debug.Log("aim sensi diff");
            changesMade = true;
        }
        if (currentGameBrightness != (int)SaveInstance._Instance.currentLoadedConfig.userWaveBrightness)
        {
            //Debug.Log("bright diff");
            changesMade = true;
        }

        return changesMade;
    }


    protected virtual void SetApplyButtonActive(bool setActive)
    {
        applyChangesButton.interactable = setActive;
    }
    public virtual void ApplyChanges()
    {
        SaveInstance._Instance.currentLoadedConfig.userResolution = allResolutions[currentIndexResolution];
        SaveInstance._Instance.currentLoadedConfig.userWindowedMode = isWindowedMode;
        SaveInstance._Instance.currentLoadedConfig.userMusicVolume = currentMusicVolume;
        SaveInstance._Instance.currentLoadedConfig.userSoundVolume = currentSoundsVolume;
        SaveInstance._Instance.currentLoadedConfig.userSensibility = currentGameSensibility;
        SaveInstance._Instance.currentLoadedConfig.userAimingSensibility = currentAimingGameSensibility;
        SaveInstance._Instance.currentLoadedConfig.userWaveBrightness = (BrightnessEnum)currentGameBrightness;

        SaveInstance._Instance.SaveConfig();

        CheckChanges();
        manager.SetSelectedGameObject(returnToMenuButton.gameObject);
    }
    protected virtual void ResetChanges()
    {
        // VIDEO PANEL
        for (int i = 0; i < allResolutions.Count; i++)
        {
            string currentLoadedScreen = $"{SaveInstance._Instance.currentLoadedConfig.userResolution.width} x {SaveInstance._Instance.currentLoadedConfig.userResolution.height}";
            string indexString = $"{allResolutions[i].width} x {allResolutions[i].height}";

            if (currentLoadedScreen == indexString)
            {
                currentIndexResolution = i;
                break;
            }
        }

        videoResolutionSlider.SetValueWithoutNotify(currentIndexResolution);
        videoResolutionText.text = $"{allResolutions[currentIndexResolution].width} x {allResolutions[currentIndexResolution].height}";

        isWindowedMode = SaveInstance._Instance.currentLoadedConfig.userWindowedMode;
        videoWindowedModeToggle.SetIsOnWithoutNotify(isWindowedMode);


        // AUDIO PANEL
        currentMusicVolume = SaveInstance._Instance.currentLoadedConfig.userMusicVolume;
        audioMusicSlider.SetValueWithoutNotify(currentMusicVolume);

        currentSoundsVolume = SaveInstance._Instance.currentLoadedConfig.userSoundVolume;
        audioSoundSlider.SetValueWithoutNotify(currentSoundsVolume);


        // GAME PANEL
        currentGameBrightness = (int)SaveInstance._Instance.currentLoadedConfig.userWaveBrightness;
        gameBrightnessSlider.SetValueWithoutNotify(currentGameBrightness);
        gameBrightnessTextTranslation.textKey = gameBrightnessTextTranslations[currentGameBrightness];
        gameBrightnessText.text = LanguageManager._Instance.GetText(gameBrightnessTextTranslations[currentGameBrightness]);

        currentGameSensibility = SaveInstance._Instance.currentLoadedConfig.userSensibility;
        gameSensibilitySlider.SetValueWithoutNotify(currentGameSensibility);

        currentAimingGameSensibility = SaveInstance._Instance.currentLoadedConfig.userAimingSensibility;
        gameAimingSensibilitySlider.SetValueWithoutNotify(currentAimingGameSensibility);
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

        if (unappliedChanges)
        {
            ResetChanges();
            CheckChanges();
        }
    }

    public override void ButtonActive(bool active)
    {
        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].GetThisButton().interactable = active;            
        }

        if (active)
        {
            if (!unappliedChanges)
            {
                applyChangesButton.interactable = false;
            }
        }

        if (isGameLevel)
        {
            englishButton.interactable = false;
            spanishButton.interactable = false;
            portugueseButton.interactable = false;
            frenchButton.interactable = false;
            germanButton.interactable = false;
            russianButton.interactable = false;
        }

    }
}