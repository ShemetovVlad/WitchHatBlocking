using System;
using UnityEngine;
using YG;

public class SaveManagerYG : MonoBehaviour
{
    private bool isInitialized;
    private bool saveVolumeScheduled;
    private bool saveRecipesScheduled;
    [SerializeField] private RecipeManager recipeManager;
    [SerializeField] private Book book; 

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

        if (PlayerWallet.Instance != null && YG2.saves != null && SoundManager.Instance != null)
        {
            LoadAllData();
            isInitialized = true;
        }
        else
        {
            Invoke(nameof(OnSDKDataReceived), 0.1f);
        }
    }

    private void LoadAllData()
    {
        // Загружаем баланс
        int savedBalance = YG2.saves.playerBalance;
        PlayerWallet.Instance.SetBalanceFromSave(savedBalance);

        // Загружаем громкость в SoundManager
        SoundManager.Instance.SetMusicVolume(YG2.saves.musicVolume);
        SoundManager.Instance.SetSfxVolume(YG2.saves.sfxVolume);

        // Загружаем состояния рецептов
        LoadRecipes();
    }

    private void OnMoneyChanged(int oldBalance, int newBalance)
    {
        if (!isInitialized) return;
        YG2.saves.playerBalance = newBalance;
        YG2.SaveProgress();
    }

    // Вызывается при изменении громкости
    private void OnVolumeChanged(float volume)
    {
        if (!isInitialized) return;

        // Отменяем предыдущее запланированное сохранение
        if (saveVolumeScheduled)
        {
            CancelInvoke(nameof(SaveVolumes));
        }

        // Запланировать новое сохранение через секунду
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
        }
        saveVolumeScheduled = false;
    }
    private void SaveRecipes()
    {
        if (recipeManager != null && YG2.saves != null)
        {
            bool[] unlockedRecipes = book.GetUnlockedStates();
            if (unlockedRecipes != null)
            {
                YG2.saves.unlockedRecipes = unlockedRecipes;
                YG2.SaveProgress();
                Debug.Log($"SaveManagerYG: saved {unlockedRecipes.Length} recipes");
            }
        }
    }
    private void LoadRecipes()
    {
        if (book != null && YG2.saves != null && YG2.saves.unlockedRecipes != null)
        {
            book.SetUnlockedStates(YG2.saves.unlockedRecipes);
            Debug.Log($"SaveManagerYG: loaded {YG2.saves.unlockedRecipes.Length} recipes");
        }
    }
}