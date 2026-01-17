using System;
using UnityEngine;

public enum Language
{
    Russian,
    English
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }
    
    [SerializeField] private Language defaultLanguage = Language.Russian;
    private Language currentLanguage;
    
    // Event for language change
    public static event Action<Language> OnLanguageChanged;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Load saved language or use default
        string savedLanguage = PlayerPrefs.GetString("language", defaultLanguage.ToString());
        if (Enum.TryParse<Language>(savedLanguage, out Language lang))
        {
            currentLanguage = lang;
        }
        else
        {
            currentLanguage = defaultLanguage;
        }
    }
    
    private void Start()
    {
        // Notify about initial language
        OnLanguageChanged?.Invoke(currentLanguage);
    }
    
    public Language GetCurrentLanguage()
    {
        return currentLanguage;
    }
    
    public string GetCurrentLanguageCode()
    {
        return currentLanguage == Language.Russian ? "ru" : "en";
    }
    
    public void SetLanguage(Language language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            PlayerPrefs.SetString("language", language.ToString());
            PlayerPrefs.Save();
            OnLanguageChanged?.Invoke(language);
            Debug.Log($"Language changed to: {language}");
        }
    }
    
    public void SetLanguageByCode(string code)
    {
        if (code == "ru")
        {
            SetLanguage(Language.Russian);
        }
        else
        {
            SetLanguage(Language.English);
        }
    }
}