using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SellCounter : BaseCounter
{
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            
            PlayerWallet.Instance.AddMoney(50);
            int balance = PlayerWallet.Instance.GetBalance();
            Debug.Log(balance);
        }
    }
}
