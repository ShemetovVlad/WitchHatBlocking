using UnityEngine;
using YG;

public class SaveManagerYG : MonoBehaviour
{
    private void OnEnable()
    {
        YG2.onGetSDKData += GetData;
    }

    // Отписываемся от ивента onGetSDKData
    private void OnDisable()
    {
        YG2.onGetSDKData -= GetData;
    }

    private void Awake()
    {
        GetData();
    }
    void Start()
    {
        PlayerWallet.OnMoneyChanged += onMoneyChanged;
        if (YG2.isSDKEnabled == true)
        {
            GetData();
        }
    }
    public void GetData()
    {
        if (PlayerWallet.Instance == null)
        {
            Debug.LogWarning("SaveManagerYG: PlayerWallet instance is null. Cannot load player balance.");
            Invoke(nameof(GetData), 0.2f); // Пытаемся снова через полсекунды
            return;
        }
        if (YG2.saves == null)
        {
            Debug.LogWarning("SaveManagerYG: YG2.saves is null. Cannot load player balance.");
            Invoke(nameof(GetData), 0.2f); // Пытаемся снова через полсекунды
            return;
        }
        // Получаем сохранённый баланс игрока из YG2.saves и устанавливаем его в PlayerWallet
        int savedBalance = YG2.saves.playerBalance;
        PlayerWallet.Instance.SetBalanceFromSave(savedBalance);
        Debug.Log("SaveManagerYG: loaded player balance: " + savedBalance);
    }
    private void onMoneyChanged(int oldBalance, int newBalance)
    {
        YG2.saves.playerBalance = newBalance;
        YG2.SaveProgress();
        Debug.Log("SaveManagerYG: saved player balance: " + newBalance);
    }

}
