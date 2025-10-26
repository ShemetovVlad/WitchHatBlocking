using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        // Инициализируем слайдеры текущими значениями
        if (SoundManager.Instance != null)
        {
            musicVolumeSlider.value = SoundManager.Instance.GetMusicVolume();
            sfxVolumeSlider.value = SoundManager.Instance.GetSfxVolume();
        }

        // Подписываемся на изменения слайдеров
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
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
    }
}
