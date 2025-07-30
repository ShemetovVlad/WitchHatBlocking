using UnityEngine;
using UnityEngine.UI;

public class PerkButtonUI : MonoBehaviour
{
    [SerializeField] private SpeedUpSO speedPerk;
    private Image buttonImage;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        button.onClick.AddListener(TogglePerk);
    }

    private void TogglePerk()
    {
        if (!speedPerk.isActive)
        {
            // Активируем перк
            SkillTreeManager.OnSpeedPerkActivated.Invoke(speedPerk.speedMultiplier);
            speedPerk.isActive = true;
            buttonImage.color = speedPerk.activeColor;
        }
        else
        {
            // Деактивируем перк (возвращаем базовую скорость)
            SkillTreeManager.OnSpeedPerkActivated.Invoke(1f);
            speedPerk.isActive = false;
            buttonImage.color = speedPerk.inactiveColor;
        }
    }
}
