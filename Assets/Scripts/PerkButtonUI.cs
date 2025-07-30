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
            perk.ApplyEffect(true);
            perk.OnActivated.Invoke();
        }
        else
        {
            perk.ApplyEffect(false);
            perk.OnDeactivated.Invoke();
        }

        UpdateButtonColor();
    }

    private void UpdateButtonColor()
    {
        buttonImage.color = perk.isActive ? perk.activeColor : perk.inactiveColor;
    }
}
