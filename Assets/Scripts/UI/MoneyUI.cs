using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private Animation moneyAnimation;
    [SerializeField] private Color addColor = Color.green;
    [SerializeField] private Color spendColor = Color.red;

    private Color originalColor;
    private int currentDisplayBalance;

    private void Start()
    {
        if (moneyText == null)
            moneyText = GetComponent<Text>();

        originalColor = moneyText.color;
        PlayerWallet.OnMoneyChanged += OnMoneyChanged;
        currentDisplayBalance = PlayerWallet.Instance.GetBalance();
        UpdateDisplay();
    }

    private void OnMoneyChanged(int oldBalance, int newBalance)
    {
        // Анимация изменения
        if (moneyAnimation != null)
            moneyAnimation.Play();

        // Цветовая индикация
        if (newBalance > oldBalance)
        {
            StartCoroutine(FlashColor(addColor));
            
        }
        else
        {
            StartCoroutine(FlashColor(spendColor));
        }
        currentDisplayBalance = newBalance;
        UpdateDisplay();
        
    }

    private void UpdateDisplay()
    {
        if (moneyText != null)
            moneyText.text = currentDisplayBalance.ToString("N0");
        Debug.Log(moneyText.text);
    }

    private System.Collections.IEnumerator FlashColor(Color color)
    {
        moneyText.color = color;
        yield return new WaitForSeconds(0.3f);
        moneyText.color = originalColor;
    }

    private void OnDestroy()
    {
        PlayerWallet.OnMoneyChanged -= OnMoneyChanged;
    }
}