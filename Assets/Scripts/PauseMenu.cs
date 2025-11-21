using UnityEngine;
using UnityEngine.UI;
using YG;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle russianToggle;
    [SerializeField] private Toggle englishToggle;

    private bool isSettingToggles = false;

    private void Start()
    {
        // Инициализируем слайдеры
        if (SoundManager.Instance != null)
        {
            musicVolumeSlider.value = SoundManager.Instance.GetMusicVolume();
            sfxVolumeSlider.value = SoundManager.Instance.GetSfxVolume();
        }

        // Подписываемся на изменения слайдеров
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        // Подписываем тогглы на события
        if (russianToggle != null)
            russianToggle.onValueChanged.AddListener(OnRussianToggleChanged);
        if (englishToggle != null)
            englishToggle.onValueChanged.AddListener(OnEnglishToggleChanged);

        // Подписываемся на событие смены языка от Яндекс SDK
        YG2.onSwitchLang += OnLanguageChanged;

        // Если язык уже установлен, сразу синхронизируем тогглы
        if (!string.IsNullOrEmpty(YG2.lang))
        {
            SetToggleByLanguage(YG2.lang);
        }
    }

    private void OnLanguageChanged(string language)
    {
        SetToggleByLanguage(language);
    }

    private void SetToggleByLanguage(string language)
    {
        if (russianToggle == null || englishToggle == null) return;

        // Защита от бесконечного цикла
        if (isSettingToggles) return;

        isSettingToggles = true;

        // Временно отписываемся чтобы не вызывать лишние события
        russianToggle.onValueChanged.RemoveListener(OnRussianToggleChanged);
        englishToggle.onValueChanged.RemoveListener(OnEnglishToggleChanged);

        // Ставим правильный тоггл
        if (language == "ru")
        {
            russianToggle.isOn = true;
            englishToggle.isOn = false;
        }
        else
        {
            russianToggle.isOn = false;
            englishToggle.isOn = true;
        }

        // Снова подписываемся
        russianToggle.onValueChanged.AddListener(OnRussianToggleChanged);
        englishToggle.onValueChanged.AddListener(OnEnglishToggleChanged);

        isSettingToggles = false;
    }

    private void OnRussianToggleChanged(bool isOn)
    {
        if (isOn && !isSettingToggles)
        {
            YG2.SwitchLanguage("ru");
        }
    }

    private void OnEnglishToggleChanged(bool isOn)
    {
        if (isOn && !isSettingToggles)
        {
            YG2.SwitchLanguage("en");
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }

    private void OnDestroy()
    {
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);

        if (russianToggle != null)
            russianToggle.onValueChanged.RemoveListener(OnRussianToggleChanged);
        if (englishToggle != null)
            englishToggle.onValueChanged.RemoveListener(OnEnglishToggleChanged);

        // Отписываемся от события Яндекс SDK
        YG2.onSwitchLang -= OnLanguageChanged;
    }
}