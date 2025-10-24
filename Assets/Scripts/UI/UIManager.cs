using UnityEngine;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("UI Windows")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private GameObject bookWindow;
    [SerializeField] private GameObject controlsWindow;

    [Header("Input")]
    [SerializeField] private GameInput gameInput;

    // Текущее открытое окно (null если нет открытых окон)
    private GameObject currentOpenWindow;

    // Окна, которые блокируют InGameUI
    private HashSet<GameObject> blockingWindows;

    // Окна, которые ставят игру на паузу
    private HashSet<GameObject> pausingWindows;

    private void Awake()
    {
        blockingWindows = new HashSet<GameObject> { settingsWindow, shopWindow };
        pausingWindows = new HashSet<GameObject> { settingsWindow, shopWindow };
    }

    private void Start()
    {
        // Подписываемся на событие ESC
        gameInput.OnPauseAction += GameInput_OnPauseAction;

        SetInitialUIState();
    }

    private void OnDestroy()
    {
        // Отписываемся от события
        if (gameInput != null)
            gameInput.OnPauseAction -= GameInput_OnPauseAction;
    }

    private void SetInitialUIState()
    {
        inGameUI.SetActive(true);
        settingsWindow.SetActive(false);
        shopWindow.SetActive(false);
        bookWindow.SetActive(false);
        controlsWindow.SetActive(false);

        currentOpenWindow = null;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        HandleEscapePress();
    }

    private void HandleEscapePress()
    {
        // Если есть открытое окно - закрываем его
        if (currentOpenWindow != null)
        {
            CloseWindow(currentOpenWindow);
        }
        else
        {
            // Если нет открытых окон - открываем настройки
            OpenSettings();
        }
    }

    // === Общие методы для работы с окнами ===

    private void OpenWindow(GameObject window)
    {
        
        // Закрываем текущее окно если оно есть
        if (currentOpenWindow != null && currentOpenWindow != window)
        {
            CloseWindow(currentOpenWindow);
        }

        // Открываем новое окно
        window.SetActive(true);
        currentOpenWindow = window;

        // Блокируем InGameUI если нужно
        if (blockingWindows.Contains(window))
        {
            SetInGameUIInteractable(false);
        }

        // Ставим на паузу если нужно
        if (pausingWindows.Contains(window))
        {
            SetPauseState(true);
        }
    }
    public void ToggleWindow(GameObject window)
    {
        if (window.activeInHierarchy)
        {
            CloseWindow(window);
        }
        else
        {
            OpenWindow(window);
        }
    }

    private void CloseWindow(GameObject window)
    {
        if (window == null) return;

        window.SetActive(false);

        // Если закрываем текущее окно - очищаем ссылку
        if (currentOpenWindow == window)
        {
            currentOpenWindow = null;
        }

        // Разблокируем InGameUI если нужно
        if (blockingWindows.Contains(window))
        {
            SetInGameUIInteractable(true);
        }

        // Снимаем с паузы если нужно и нет других паузирующих окон
        if (pausingWindows.Contains(window) && !IsAnyPausingWindowOpen())
        {
            SetPauseState(false);
        }
    }

    // === Публичные методы для кнопок ===

    public void OpenSettings() => OpenWindow(settingsWindow);
    public void CloseSettings() => CloseWindow(settingsWindow);

    public void OpenShop() => OpenWindow(shopWindow);
    public void CloseShop() => CloseWindow(shopWindow);

    public void OpenBook() => OpenWindow(bookWindow);
    public void CloseBook() => CloseWindow(bookWindow);

    public void OpenControls() => OpenWindow(controlsWindow);
    public void CloseControls() => CloseWindow(controlsWindow);

    // === Вспомогательные методы ===

    private void SetInGameUIInteractable(bool interactable)
    {
        // Получаем все кнопки InGameUI и делаем их неактивными
        var buttons = inGameUI.GetComponentsInChildren<UnityEngine.UI.Button>();
        foreach (var button in buttons)
        {
            button.interactable = interactable;
        }
    }

    private bool IsAnyPausingWindowOpen()
    {
        foreach (var window in pausingWindows)
        {
            if (window.activeInHierarchy)
                return true;
        }
        return false;
    }

    private void SetPauseState(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
    }
    public void CloseCurrentWindow()
    {
        if (currentOpenWindow != null)
        {
            CloseWindow(currentOpenWindow);
        }
    }

    public bool IsAnyWindowOpen() => currentOpenWindow != null;
}