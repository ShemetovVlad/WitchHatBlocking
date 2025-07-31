using UnityEngine;
using System;

// Скрипт должен быть в папке Resources, если используем автоматическое создание
public class PlayerWallet : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private int startingBalance = 0;
    [SerializeField] private int maxBalance = 999999;

    [Header("Только для отладки")]
    [SerializeField] private int currentBalance;

    // События
    public static event Action<int, int> OnMoneyChanged;      // (old, new)
    public static event Action<int> OnMoneyAdded;
    public static event Action<int> OnMoneySpent;
    public static event Action<int> OnNotEnoughMoney;

    // Синглтон
    public static PlayerWallet Instance { get; private set; }

    private void Awake()
    {
        // Обеспечиваем единственный экземпляр
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentBalance = startingBalance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Уведомляем UI и другие системы о начальном балансе
        OnMoneyChanged?.Invoke(0, currentBalance);
    }

    /// <summary>
    /// Добавляет деньги. Если превышен лимит — устанавливается maxBalance.
    /// </summary>
    public void AddMoney(int amount)
    {
        if (amount <= 0) return;

        int oldBalance = currentBalance;
        currentBalance = Mathf.Clamp(currentBalance + amount, 0, maxBalance);

        OnMoneyChanged?.Invoke(oldBalance, currentBalance);
        OnMoneyAdded?.Invoke(amount);
    }

    /// <summary>
    /// Пытается потратить деньги. Возвращает true, если успешно.
    /// </summary>
    public bool SpendMoney(int amount)
    {
        if (amount <= 0) return false;

        if (currentBalance >= amount)
        {
            int oldBalance = currentBalance;
            currentBalance -= amount;

            OnMoneyChanged?.Invoke(oldBalance, currentBalance);
            OnMoneySpent?.Invoke(amount);
            return true;
        }
        else
        {
            OnNotEnoughMoney?.Invoke(amount);
            return false;
        }
    }

    // Геттеры
    public int GetBalance() => currentBalance;
    public bool HasEnoughMoney(int amount) => currentBalance >= amount;
}