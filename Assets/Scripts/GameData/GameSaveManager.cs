using System;
using System.Collections;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }

    private const string SAVE_KEY = "GameSaveData";
    private GameSaveData currentSaveData;
    private bool isApplyingLoadedData = false;

    // Ссылки на системы, которые нужно сохранять
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private PlayerWallet playerWallet;
    [SerializeField] private Book recipeBook;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Debug.Log("=== GameSaveManager Awake ===");

        // Загружаем данные при старте
        LoadGame();
    }

    private void Start()
    {
        //Debug.Log("=== GameSaveManager Start ===");
        CheckReferences();
        //Debug.Log($"Перед подпиской: Музыка={currentSaveData.musicVolume}, SFX={currentSaveData.sfxVolume}, Баланс={currentSaveData.playerBalance}");
        // Подписываемся на события изменений
        SubscribeToEvents();

        // Применяем загруженные данные
        ApplyLoadedData();
    }

    private void SubscribeToEvents()
    {
        // Подписываемся на изменения звука
        if (soundManager != null)
        {
            soundManager.OnSfxVolumeChanged += OnSfxVolumeChanged;
            soundManager.OnMusicVolumeChanged += OnMusicVolumeChanged;
        }

        // Подписываемся на изменения денег
        if (playerWallet != null)
        {
            PlayerWallet.OnMoneyChanged += OnMoneyChanged;
        }
        // Подписываемся на открытие рецептов
        RecipeManager recipeManager = FindFirstObjectByType<RecipeManager>();
        if (recipeManager != null)
        {
            recipeManager.OnRecipeUnlocked += OnRecipeUnlocked;
            //Debug.Log("Подписались на события RecipeManager");
        }
        else
        {
            Debug.LogError("RecipeManager не найден на сцене!");
        }
    }
    
    private IEnumerator SaveAfterRecipeUnlock()
    {
        yield return new WaitForEndOfFrame(); // Ждем пока книга обновится
        SaveGame();
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (soundManager != null)
        {
            soundManager.OnSfxVolumeChanged -= OnSfxVolumeChanged;
            soundManager.OnMusicVolumeChanged -= OnMusicVolumeChanged;
        }

        if (playerWallet != null)
        {
            PlayerWallet.OnMoneyChanged -= OnMoneyChanged;
        }
        RecipeManager recipeManager = FindFirstObjectByType<RecipeManager>();
        if (recipeManager != null)
        {
            recipeManager.OnRecipeUnlocked -= OnRecipeUnlocked;
        }
    }

    // === СОБЫТИЯ ДЛЯ АВТОСОХРАНЕНИЯ ===

    private void OnSfxVolumeChanged(float volume)
    {
        if (isApplyingLoadedData) return;
        currentSaveData.sfxVolume = volume;
        SaveGame();
    }

    private void OnMusicVolumeChanged(float volume)
    {
        if (isApplyingLoadedData) return;
        currentSaveData.musicVolume = volume;
        SaveGame();
    }

    private void OnMoneyChanged(int oldBalance, int newBalance)
    {
        if (isApplyingLoadedData) return;
        currentSaveData.playerBalance = newBalance;
        SaveGame();
    }
    private void OnRecipeUnlocked(object sender, EventArgs e)
    {
        if (isApplyingLoadedData) return;

        //Debug.Log("Рецепт открыт - сохраняем игру");
        // Сохраняем когда открывается новый рецепт
        StartCoroutine(SaveAfterRecipeUnlock());
    }

    // === ОСНОВНЫЕ МЕТОДЫ СОХРАНЕНИЯ/ЗАГРУЗКИ ===

    public void SaveGame()
    {
        try
        {
            //Debug.Log("=== Сохранение игры ===");
            // Обновляем актуальные данные перед сохранением
            UpdateSaveDataFromSystems();
            //Debug.Log($"Сохраняемые данные: Музыка={currentSaveData.musicVolume}, SFX={currentSaveData.sfxVolume}, Баланс={currentSaveData.playerBalance}");

            string jsonData = JsonUtility.ToJson(currentSaveData);
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
            PlayerPrefs.Save();
            //Debug.Log("Игра сохранена успешно! JSON: " + jsonData);

            //Debug.Log("Игра сохранена успешно!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка сохранения: {e.Message}");
        }
    }

    public void LoadGame()
    {
        try
        {
            //Debug.Log("=== Загрузка игры ===");
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string jsonData = PlayerPrefs.GetString(SAVE_KEY);
                //Debug.Log("Найдено сохранение: " + jsonData);
                currentSaveData = JsonUtility.FromJson<GameSaveData>(jsonData);

                if (!currentSaveData.IsValid())
                {
                    Debug.LogWarning("Некорректные данные сохранения. Сброс к значениям по умолчанию.");
                    currentSaveData = new GameSaveData();
                }
                else
                {
                    //Debug.Log($"Загружено: Музыка={currentSaveData.musicVolume}, SFX={currentSaveData.sfxVolume}, Баланс={currentSaveData.playerBalance}");
                }
                //Debug.Log("Игра загружена успешно!");
            }
            else
            {
                // Первый запуск - создаем новые данные
                currentSaveData = new GameSaveData();
                //Debug.Log("Создано новое сохранение!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка загрузки: {e.Message}");
            currentSaveData = new GameSaveData();
        }
    }

    // === ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ===

    private void UpdateSaveDataFromSystems()
    {
        //Debug.Log("=== Обновление данных для сохранения ===");
        // Получаем актуальные данные из всех систем
        if (soundManager != null)
        {
            // Нужно добавить геттеры в SoundManager!
            float currentMusic = soundManager.GetMusicVolume();
            float currentSfx = soundManager.GetSfxVolume();
            //Debug.Log($"Текущие настройки звука: Музыка={currentMusic}, SFX={currentSfx}");
            currentSaveData.musicVolume = currentMusic;
            currentSaveData.sfxVolume = currentSfx;
        }
        else
        {
            Debug.LogError("SoundManager не найден!");
        }
        if (playerWallet != null)
        {
            int currentBalance = playerWallet.GetBalance();
            //Debug.Log($"Текущий баланс кошелька: {currentBalance}");
            currentSaveData.playerBalance = currentBalance;
        }
        else
        {
            //Debug.LogError("PlayerWallet не найден!");
        }
        if (recipeBook != null)
        {
            bool[] unlockedStates = recipeBook.GetUnlockedStates();
            currentSaveData.unlockedRecipes = unlockedStates;
            //Debug.Log($"Сохранено состояний рецептов: {unlockedStates?.Length ?? 0}");
        }
    }

    private void ApplyLoadedData()
    {
        //Debug.Log("=== Применение загруженных данных ===");
        isApplyingLoadedData = true;
        //Debug.Log($"Применяем: Музыка={currentSaveData.musicVolume}, SFX={currentSaveData.sfxVolume}, Баланс={currentSaveData.playerBalance}");
        // Применяем загруженные данные к игровым системам

        if (soundManager != null)
        {
            //Debug.Log($"Устанавливаем громкость: Музыка={currentSaveData.musicVolume}, SFX={currentSaveData.sfxVolume}");
            soundManager.SetMusicVolume(currentSaveData.musicVolume);
            soundManager.SetSfxVolume(currentSaveData.sfxVolume);
        }

        if (playerWallet != null)
        {
            //Debug.Log($"Устанавливаем баланс: {currentSaveData.playerBalance}");
            // Для кошелька нужно аккуратно установить баланс
            ApplyWalletBalance(currentSaveData.playerBalance);
        }
        if (recipeBook != null && currentSaveData.unlockedRecipes != null)
        {
            //Debug.Log($"Применяем состояния рецептов: {currentSaveData.unlockedRecipes.Length}");
            recipeBook.SetUnlockedStates(currentSaveData.unlockedRecipes);
        }
        else if (recipeBook != null)
        {
            //Debug.Log("Создаем начальные состояния рецептов");
            recipeBook.InitializeBook(); // Инициализируем если нет сохранения
        }
        StartCoroutine(EnableAutoSaveAfterDelay());
    }
    private IEnumerator EnableAutoSaveAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Ждем пока все системы применят данные
        isApplyingLoadedData = false;
        //Debug.Log("Автосохранение включено");
    }

    private void ApplyWalletBalance(int targetBalance)
    {
        // Получаем текущий баланс
        int currentBalance = playerWallet.GetBalance();
        //Debug.Log($"Корректировка баланса: Текущий={currentBalance}, Целевой={targetBalance}");

        // Если балансы отличаются - корректируем
        if (currentBalance != targetBalance)
        {
            int difference = targetBalance - currentBalance;
            //Debug.Log($"Разница: {difference}");

            if (difference > 0)
            {
                playerWallet.AddMoney(difference);
                //Debug.Log($"Добавлено денег: {difference}");
            }
            else
            {
                // Для уменьшения баланса используем SpendMoney
                // Но будем осторожны - если денег недостаточно, просто установим нужную сумму
                playerWallet.SpendMoney(-difference);
                //Debug.Log($"Потрачено денег: {-difference}");
            }
        }
        else
        {
            //Debug.Log("Баланс уже соответствует целевому");
        }
    }
    private void CheckReferences()
    {
        //Debug.Log("=== Проверка ссылок ===");
        //Debug.Log($"SoundManager: {soundManager != null}");
        //Debug.Log($"PlayerWallet: {playerWallet != null}");
        //Debug.Log($"RecipeBook: {recipeBook != null}");

        if (soundManager == null || playerWallet == null || recipeBook == null)
        {
            Debug.LogError("Не все ссылки установлены в инспекторе!");
        }
    }
    // Для отладки
    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
        currentSaveData = new GameSaveData();
        ApplyLoadedData();
        Debug.Log("Сохранение удалено!");
    }

    public GameSaveData GetCurrentSaveData() => currentSaveData;
}