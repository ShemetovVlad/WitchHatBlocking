using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class SellCounter : BaseCounter
{
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            int sellPrice = player.GetKitchenObject().GetKitchenObjectSO().price;
            player.GetKitchenObject().DestroySelf();
            if (sellPrice <= 5)
            {
                SellingIngredient();
            }
            else if (sellPrice > 5 && sellPrice <= 200)
            {
                SellingPotionCheap();
            }
            else
            {
                SellingPotionExpensive();
            }
            PlayerWallet.Instance.AddMoney(sellPrice);
            int balance = PlayerWallet.Instance.GetBalance();
            //Debug.Log(balance);
        }
    }

    private void SellingIngredient()
    {
        EffectManager.Instance.SellIngredientEffect();
        SoundManager.Instance.PlaySound(SoundType.SellingIngredient, Camera.main.transform.position);
    }
    private void SellingPotionCheap()
    {
        EffectManager.Instance.SellPotionCheapEffect();
        SoundManager.Instance.PlaySound(SoundType.SellingPotionCheap, Camera.main.transform.position);
    }
    private void SellingPotionExpensive()
    {   
        EffectManager.Instance.SellPotionExpensiveEffect();
        SoundManager.Instance.PlaySound(SoundType.SellingPotionExpensive, Camera.main.transform.position);
    }
}
