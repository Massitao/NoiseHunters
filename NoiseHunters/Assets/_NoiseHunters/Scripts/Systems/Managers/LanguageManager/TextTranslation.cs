using UnityEngine;
using UnityEngine.UI;

public class TextTranslation : MonoBehaviour
{
    [Header("Text Component")]
    protected Text thisText;

    [Header("Translation ID")]
    [SerializeField] public string textKey;


    protected void Awake()
    {
        if (TryGetComponent(out Text textInGO))
        {
            thisText = textInGO;
        }
        else
        {
            enabled = false;
            Debug.LogWarning("No text was found!", gameObject);
        }
    }

    protected void OnEnable()
    {
        LanguageManager._Instance.OnLanguageChange += GetText;
    }

    protected void OnDisable()
    {
        LanguageManager._Instance.OnLanguageChange -= GetText;
    }


    protected void Start()
    {
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