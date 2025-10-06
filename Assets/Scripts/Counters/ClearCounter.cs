using UnityEngine;

public class ClearCounter : BaseCounter
{

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
        else {
            // There is a object here
            if (!player.HasKitchenObject())
            {
                // Player dont carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                
            }
        }
    }

}
