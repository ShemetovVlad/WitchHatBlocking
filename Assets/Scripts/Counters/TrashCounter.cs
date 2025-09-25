using UnityEngine;

public class TrashCounter : BaseCounter
{
    [SerializeField] private Animator animator;
    private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            
            if (animator != null)
            {
                animator.SetTrigger(DestroyTrigger);
            }
        }
    }
}
