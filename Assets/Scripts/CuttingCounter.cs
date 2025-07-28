using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            // There is no objects here
            if (player.HasKitchenObject())
            {
                // Player carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }

        }
        else
        {
            // There is a object here
            if (!player.HasKitchenObject())
            {
                // Player dont carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(PlayerController player)
    {
        if (HasKitchenObject()) { 
            // There is a kitchen object here
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
