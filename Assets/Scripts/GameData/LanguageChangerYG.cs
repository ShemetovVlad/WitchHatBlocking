using UnityEngine;
using UnityEngine.UI;

public class LanguageChanger : MonoBehaviour
{
    public string ru, en;

    private Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    private void OnEnable()
    {
        LanguageManager.OnLanguageChanged += OnLanguageChanged;
        // Set initial language
        if (LanguageManager.Instance != null)
        {
            SetTextByLanguage(LanguageManager.Instance.GetCurrentLanguageCode());
        }
    }
    
    private void OnDisable()
    {
        LanguageManager.OnLanguageChanged -= OnLanguageChanged;
    }
    
    private void OnLanguageChanged(Language language)
    {
        SetTextByLanguage(LanguageManager.Instance.GetCurrentLanguageCode());
    }

    private void SetTextByLanguage(string lang)
    {
        switch (lang)
        {
            case "ru":
                textComponent.text = ru;
                break;
            default:
                textComponent.text = en;
                break;
        }
    }
}