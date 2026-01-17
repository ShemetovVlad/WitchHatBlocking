using System;
using UnityEngine;

public class LocalSaveManager : MonoBehaviour
{
    private bool isInitialized;
    private bool saveVolumeScheduled;
    private bool saveRecipesScheduled;
    [SerializeField] private RecipeManager recipeManager;
    [SerializeField] private Book book;

    // Добавляем статическую ссылку для доступа из других сцен
    public static LocalSaveManager Instance { get; private set; }

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
        
        // Загружаем данные при включении
        LoadAllData();
    }

    private void OnDisable()
    {
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

    private void Start()
    {
        isInitialized = true;
        Debug.Log("LocalSaveManager: Initialized successfully");
    }

    // Добавляем метод для принудительной перезагрузки данных при переходе на сцену игры
    public void ReloadDataForGameScene()
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        Debug.Log("LocalSaveManager: Loading all data...");

        // Загружаем баланс
        int savedBalance = PlayerPrefs.GetInt("playerBalance", 0);
        if (PlayerWallet.Instance != null)
        {
            PlayerWallet.Instance.SetBalanceFromSave(savedBalance);
            Debug.Log($"LocalSaveManager: Loaded balance: {savedBalance}");
        }

        // Загружаем громкость в SoundManager
        if (SoundManager.Instance != null)
        {
            float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
            SoundManager.Instance.SetMusicVolume(musicVolume);
            SoundManager.Instance.SetSfxVolume(sfxVolume);
            Debug.Log($"LocalSaveManager: Loaded volumes - Music: {musicVolume}, SFX: {sfxVolume}");
        }

        // Загружаем состояния рецептов
        LoadRecipes();
    }

    private void OnMoneyChanged(int oldBalance, int newBalance)
    {
        if (!isInitialized)
        {
            Debug.Log("LocalSaveManager: Not initialized, skipping save");
            return;
        }

        PlayerPrefs.SetInt("playerBalance", newBalance);
        PlayerPrefs.Save();
        Debug.Log($"LocalSaveManager: Saved balance: {newBalance}");
    }

    private void OnVolumeChanged(float volume)
    {
        if (!isInitialized) return;

        if (saveVolumeScheduled)
        {
            CancelInvoke(nameof(SaveVolumes));
        }

        Invoke(nameof(SaveVolumes), 1f);
        saveVolumeScheduled = true;
    }

    private void OnRecipeUnlocked(object sender, EventArgs e)
    {
        if (!isInitialized) return;
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
            Debug.Log($"LocalSaveManager: Saved volumes - Music: {musicVolume}, SFX: {sfxVolume}");
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
                Debug.Log($"LocalSaveManager: Saved {unlockedRecipes.Length} recipes");
            }
        }
    }

    private void LoadRecipes()
    {
        if (book != null)
        {
            string recipeData = PlayerPrefs.GetString("unlockedRecipes", "");
            if (!string.IsNullOrEmpty(recipeData))
            {
                string[] recipeStrings = recipeData.Split(',');
                bool[] unlockedRecipes = new bool[recipeStrings.Length];
                for (int i = 0; i < recipeStrings.Length; i++)
                {
                    bool.TryParse(recipeStrings[i], out unlockedRecipes[i]);
                }
                book.SetUnlockedStates(unlockedRecipes);
                Debug.Log($"LocalSaveManager: Loaded {unlockedRecipes.Length} recipes");
            }
            else
            {
                Debug.LogWarning("LocalSaveManager: No recipe data found");
            }
        }
    }
}