using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Perks/Speed")]
public class SpeedPerk : PerkDataSO
{
    public float speedMultiplier = 2f;

    // Событие с параметром множителя
    public UnityEvent<float> OnSpeedMultiplierChanged;

    public override void ApplyEffect(bool isActive)
    {
        float multiplier = isActive ? speedMultiplier : 1f;
        OnSpeedMultiplierChanged.Invoke(multiplier); // Передаём множитель!
    }
}
