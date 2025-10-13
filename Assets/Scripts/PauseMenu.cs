using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Button quiteButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private ExitPopUp exitPopUp;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        // Инициализируем слайдеры текущими значениями
        if (SoundManager.Instance != null)
        {
            musicVolumeSlider.value = SoundManager.Instance.musicVolume;
            sfxVolumeSlider.value = SoundManager.Instance.sfxVolume;
        }

        // Подписываемся на изменения слайдеров
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    private void Awake()
    {
        gameInput.OnPauseAction += GameInput_OnPauseAction;
        quiteButton.onClick.AddListener(() =>
        {
            exitPopUp.gameObject.SetActive(true);
        });
            gameObject.SetActive(false);
        resumeButton.onClick.AddListener(() =>
        {
            ResumeGame();
        });
    }
    private void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }
    private void OnEnable()
    {
        if (SoundManager.Instance != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(SoundManager.Instance.musicVolume);
            sfxVolumeSlider.SetValueWithoutNotify(SoundManager.Instance.sfxVolume);
        }
    }
    private void GameInput_OnPauseAction(object sender, System.EventArgs e)
    {
        bool isPaused = Time.timeScale == 0f;
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        exitPopUp.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
    }
}
