using UnityEngine;
using TMPro;

public class TextTranslation_TMPro : MonoBehaviour
{
    [Header("Text Component")]
    protected TextMeshProUGUI thisText;
    protected DeviceChangeChecker deviceChecker;

    [Header("Translation ID")]
    [SerializeField] public string textKey_KM;
    [SerializeField] public string textKey_XBOX;
    [SerializeField] public string textKey_PS;
    [SerializeField] public string textKey;


    protected void Awake()
    {
        if (TryGetComponent(out TextMeshProUGUI textInGO))
        {
            thisText = textInGO;
        }
        else
        {
            enabled = false;
            Debug.LogWarning("No text was found!", gameObject);
        }
        if (TryGetComponent(out DeviceChangeChecker checker))
        {
            deviceChecker = checker;
        }
    }

    protected void OnEnable()
    {
        deviceChecker.OnKMChange += ChangeKey_KM;
        deviceChecker.OnXBOXChange += ChangeKey_XBOX;
        deviceChecker.OnPSChange += ChangeKey_PS;
        LanguageManager._Instance.OnLanguageChange += GetText;
    }
    protected void OnDisable()
    {
        deviceChecker.OnKMChange -= ChangeKey_KM;
        deviceChecker.OnXBOXChange -= ChangeKey_XBOX;
        deviceChecker.OnPSChange -= ChangeKey_PS;
        LanguageManager._Instance.OnLanguageChange -= GetText;
    }


    protected void Start()
    {
        switch (deviceChecker.currentDevice)
        {
            case DeviceChangeChecker.CurrentDevice.KM:
                ChangeKey_KM();
                break;
            case DeviceChangeChecker.CurrentDevice.XBOX:
                ChangeKey_XBOX();
                break;
            case DeviceChangeChecker.CurrentDevice.PS:
                ChangeKey_PS();
                break;
        }
    }

    public void ChangeKey_KM()
    {
        textKey = textKey_KM;

        if (LanguageManager._Instance != null)
        {
            GetText(LanguageManager._Instance.currentLanguage);
        }
    }
    public void ChangeKey_XBOX()
    {
        textKey = textKey_XBOX;

        if (LanguageManager._Instance != null)
        {
            GetText(LanguageManager._Instance.currentLanguage);
        }
    }
    public void ChangeKey_PS()
    {
        textKey = textKey_PS;

        if (LanguageManager._Instance != null)
        {
            GetText(LanguageManager._Instance.currentLanguage);
        }
    }


    protected void GetText(GameLanguages currentLanguage)
    {
        string textToSet = LanguageManager._Instance.GetText(textKey);

        if (textToSet != string.Empty)
        {
            thisText.text = textToSet;
        }
    }
}