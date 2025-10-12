using UnityEngine;
using UnityEngine.UI;

public class PerkButtonUI : MonoBehaviour
{
    [SerializeField] private PerkDataSO perk;
    private Image buttonImage;
    private Button button;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonColor();
        button = GetComponent<Button>();
        button.onClick.AddListener(TogglePerk);
    }

    public void TogglePerk()
    {
        perk.isActive = !perk.isActive;

        if (perk.isActive)
        {
            if (PlayerWallet.Instance.SpendMoney(perk.cost))
            {
                perk.ApplyEffect(true);
                perk.OnActivated.Invoke();
                UpdateButtonColor();
                Debug.Log(PlayerWallet.Instance.GetBalance());
            }
            
            
        }
        else
        {
            perk.ApplyEffect(false);
            perk.OnDeactivated.Invoke();
            UpdateButtonColor();
            perk.isActive = false;
        }

    }

    private void UpdateButtonColor()
    {
        buttonImage.color = perk.isActive ? perk.activeColor : perk.inactiveColor;
    }
}
