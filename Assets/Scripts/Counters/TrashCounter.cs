using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<KitchenObjectSO> speedBoostItems; // KitchenObjects that trigger speed boost

    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float boostDuration = 5f;
    private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            if (IsSpeedBoostItem(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.SpeedBuster(speedMultiplier, boostDuration);
            }
            player.GetKitchenObject().DestroySelf();

            if (animator != null)
            {
                animator.SetTrigger(DestroyTrigger);
            }

            SoundManager.Instance.PlaySound(SoundType.Trash_Bite, PlayerController.Instance.transform.position);
        }
    }
    private bool IsSpeedBoostItem(KitchenObjectSO kitchenObjectSO)
    {
        return speedBoostItems.Contains(kitchenObjectSO);
    }
}
