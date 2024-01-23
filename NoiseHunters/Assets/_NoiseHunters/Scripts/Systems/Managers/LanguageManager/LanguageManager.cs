using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : PersistentSingleton<LanguageManager>
{
    [Header("Lockit File")]
    [SerializeField] private TextAsset LockitFile;

    [Header("Text & Languages Dictionary")]
    private Dictionary<string, Dictionary<string, string>> LockitDictionary = new Dictionary<string, Dictionary<string, string>>();

    [Header("Current Language")]
    public GameLanguages currentLanguage;


    // Language Manager Events
    public event Action<GameLanguages> OnLanguageChange;


    protected override void Awake()
    {
        base.Awake();

        if (LockitFile != null)
        {
            ReadLockit();
        }
        else
        {
            Debug.LogWarning($"Lockit was not found!", gameObject);
        }
    }
    void ReadLockit()
    {
        string lockitText = LockitFile.text.ToString();
        int rowsNumber =  Regex.Split(lockitText, "\r\n").Length;
        int columnsNumber = Regex.Split(lockitText, "\r\n")[0].Split(';').Length;

        SetLanguageDictionary(lockitText, rowsNumber, columnsNumber);
    }
    void SetLanguageDictionary(string lockitText, int rowsNumbers, int columnsNumber)
    {
        for (int i = 1; i < columnsNumber; i++)
        {
            string language = Regex.Split(lockitText, "\r\n")[0].Split(';')[i].ToString();
            LockitDictionary.Add(language, new Dictionary<string, string>());

            for (int j = 1; j < rowsNumbers; j++)
            {
                string textKey = Regex.Split(lockitText, "\r\n")[j].Split(';')[0].ToString();
                string textValue = Regex.Split(lockitText, "\r\n")[j].Split(';')[i].ToString();

                LockitDictionary[language].Add(textKey, textValue);
                //Debug.Log($"Setting up {textKey} for current {language} language, which translation is {textValue}");
            }
        }
    }



    private void Start()
    {
        currentLanguage = SaveInstance._Instance.currentLoadedConfig.userLanguage;
    }


    public void ChangeCurrentLanguage(GameLanguages languageToChange)
    {
        if (currentLanguage != languageToChange)
        {
            currentLanguage = languageToChange;
            OnLanguageChange?.Invoke(languageToChange);
        }
    }


    public string GetText(string textKey)
    {
        if (LockitDictionary.ContainsKey(currentLanguage.ToString()))
        {
            if (LockitDictionary[currentLanguage.ToString()].ContainsKey(textKey))
            {
                LockitDictionary[currentLanguage.ToString()].TryGetValue(textKey, out string textValue);
                return textValue;
            }
            else
            {
                Debug.Log($"Failed to get ID! {textKey}");
                return string.Empty;
            }
        }
        else
        {
            Debug.Log("Failed to get Language!");
            return string.Empty;
        }
    }
}