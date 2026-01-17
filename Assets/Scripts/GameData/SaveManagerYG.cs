using System;
using UnityEngine;

// SaveManagerYG - менеджер сохранений, который теперь работает с LocalSaveManager
// вместо Yandex SDK для обеспечения кроссплатформенности
public class SaveManagerYG : MonoBehaviour
{
    private bool isInitialized;
    private bool saveVolumeScheduled;
    private bool saveRecipesScheduled;
    [SerializeField] private RecipeManager recipeManager;
    [SerializeField] private Book book;

    // Добавляем статическую ссылку для доступа из других сцен
    public static SaveManagerYG Instance { get; private set; }

    private void Awake()
    {
        // Реализуем паттерн синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Важно: не уничтожаем при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Подписываемся на изменения баланса
        PlayerWallet.OnMoneyChanged += OnMoneyChanged;

        // Подписываемся на изменения громкости
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnMusicVolumeChanged += OnVolumeChanged;
            SoundManager.Instance.OnSfxVolumeChanged += OnVolumeChanged;
        }

        // Подписываемся на события книги рецептов
        if (recipeManager != null)
        {
            recipeManager.OnRecipeUnlocked += OnRecipeUnlocked;
        }
        
        // Инициализируем сразу, так как не зависим от Yandex SDK
        if (!isInitialized)
        {
            LoadAllData();
            isInitialized = true;
            Debug.Log("SaveManagerYG: Initialized successfully");
        }
    }

    private void OnDisable()
    {
        PlayerWallet.OnMoneyChanged -= OnMoneyChanged;

        // Отписываемся от изменений громкости
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnMusicVolumeChanged -= OnVolumeChanged;
            SoundManager.Instance.OnSfxVolumeChanged -= OnVolumeChanged;
        }
        if (recipeManager != null)
        {
            recipeManager.OnRecipeUnlocked -= OnRecipeUnlocked;
        }
    }

    // Добавляем метод для принудительной перезагрузки данных при переходе на сцену игры
    public void ReloadDataForGameScene()
    {
        if (isInitialized)
        {
            LoadAllData();
        }
    }

    private void LoadAllData()
    {
        Debug.Log("SaveManagerYG: Loading all data...");

        // Загружаем все данные через LocalSaveManager
        if (LocalSaveManager.Instance != null)
        {
            LocalSaveManager.Instance.ReloadDataForGameScene();
        }
        else
        {
            Debug.LogWarning("SaveManagerYG: LocalSaveManager not found!");
        }
    }

    private void OnMoneyChanged(int oldBalance, int newBalance)
    {
        // Сохраняем баланс через PlayerPrefs (как в LocalSaveManager)
        PlayerPrefs.SetInt("playerBalance", newBalance);
        PlayerPrefs.Save();
        Debug.Log($"SaveManagerYG: Saved balance: {newBalance}");
    }

    private void OnVolumeChanged(float volume)
    {
        if (saveVolumeScheduled)
        {
            CancelInvoke(nameof(SaveVolumes));
        }

        Invoke(nameof(SaveVolumes), 1f);
        saveVolumeScheduled = true;
    }

    private void OnRecipeUnlocked(object sender, EventArgs e)
    {
        SaveRecipes();
    }

    private void SaveVolumes()
    {
        if (SoundManager.Instance != null)
        {
            float musicVolume = SoundManager.Instance.GetMusicVolume();
            float sfxVolume = SoundManager.Instance.GetSfxVolume();
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
            PlayerPrefs.Save();
            Debug.Log($"SaveManagerYG: Saved volumes - Music: {musicVolume}, SFX: {sfxVolume}");
        }
        saveVolumeScheduled = false;
    }

    private void SaveRecipes()
    {
        if (book != null)
        {
            bool[] unlockedRecipes = book.GetUnlockedStates();
            if (unlockedRecipes != null)
            {
                string recipeData = string.Join(",", unlockedRecipes);
                PlayerPrefs.SetString("unlockedRecipes", recipeData);
                PlayerPrefs.Save();
                Debug.Log($"SaveManagerYG: Saved {unlockedRecipes.Length} recipes");
            }
        }
    }
}