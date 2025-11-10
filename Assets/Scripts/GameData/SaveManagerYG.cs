using System;
using UnityEngine;
using YG;
using YG.Insides;

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
        YG2.onGetSDKData += OnSDKDataReceived;
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
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= OnSDKDataReceived;
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

    private void OnSDKDataReceived()
    {
        if (isInitialized) return;

        // Ждем пока все компоненты будут готовы
        if (AreAllComponentsReady())
        {
            LoadAllData();
            isInitialized = true;
            Debug.Log("SaveManagerYG: Initialized successfully");
        }
        else
        {
            // Пробуем снова через кадр
            Invoke(nameof(OnSDKDataReceived), 0.1f);
        }
    }

    private bool AreAllComponentsReady()
    {
        bool ready = PlayerWallet.Instance != null &&
                    YG2.saves != null &&
                    SoundManager.Instance != null &&
                    book != null;

        if (!ready)
        {
            Debug.Log($"Components ready: PlayerWallet={PlayerWallet.Instance != null}, " +
                     $"YG2.saves={YG2.saves != null}, " +
                     $"SoundManager={SoundManager.Instance != null}, " +
                     $"Book={book != null}");
        }

        return ready;
    }

    // Добавляем метод для принудительной перезагрузки данных при переходе на сцену игры
    public void ReloadDataForGameScene()
    {
        if (isInitialized && YG2.saves != null)
        {
            LoadAllData();
        }
    }

    private void LoadAllData()
    {
        Debug.Log("SaveManagerYG: Loading all data...");

        // Загружаем баланс
        int savedBalance = YG2.saves.playerBalance;
        if (PlayerWallet.Instance != null)
        {
            PlayerWallet.Instance.SetBalanceFromSave(savedBalance);
            Debug.Log($"SaveManagerYG: Loaded balance: {savedBalance}");
        }

        // Загружаем громкость в SoundManager
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMusicVolume(YG2.saves.musicVolume);
            SoundManager.Instance.SetSfxVolume(YG2.saves.sfxVolume);
            Debug.Log($"SaveManagerYG: Loaded volumes - Music: {YG2.saves.musicVolume}, SFX: {YG2.saves.sfxVolume}");
        }

        // Загружаем состояния рецептов
        LoadRecipes();
    }

    private void OnMoneyChanged(int oldBalance, int newBalance)
    {
        if (!isInitialized)
        {
            Debug.Log("SaveManagerYG: Not initialized, skipping save");
            return;
        }

        YG2.saves.playerBalance = newBalance;
        YG2.SaveProgress();
        Debug.Log($"SaveManagerYG: Saved balance: {newBalance}");
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
        if (SoundManager.Instance != null && YG2.saves != null)
        {
            YG2.saves.musicVolume = SoundManager.Instance.GetMusicVolume();
            YG2.saves.sfxVolume = SoundManager.Instance.GetSfxVolume();
            YG2.SaveProgress();
            Debug.Log($"SaveManagerYG: Saved volumes - Music: {YG2.saves.musicVolume}, SFX: {YG2.saves.sfxVolume}");
        }
        saveVolumeScheduled = false;
    }

    private void SaveRecipes()
    {
        if (recipeManager != null && YG2.saves != null && book != null)
        {
            bool[] unlockedRecipes = book.GetUnlockedStates();
            if (unlockedRecipes != null)
            {
                YG2.saves.unlockedRecipes = unlockedRecipes;
                YG2.SaveProgress();
                Debug.Log($"SaveManagerYG: Saved {unlockedRecipes.Length} recipes");
            }
        }
    }

    private void LoadRecipes()
    {
        if (book != null && YG2.saves != null && YG2.saves.unlockedRecipes != null)
        {
            book.SetUnlockedStates(YG2.saves.unlockedRecipes);
            Debug.Log($"SaveManagerYG: Loaded {YG2.saves.unlockedRecipes.Length} recipes");
        }
        else
        {
            Debug.LogWarning("SaveManagerYG: Could not load recipes - missing components or data");
        }
    }
}