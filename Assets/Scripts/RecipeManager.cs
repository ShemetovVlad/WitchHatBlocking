using UnityEngine;
using System;

public class RecipeManager : MonoBehaviour
{
    public Book recipeBook;
    public CauldronCounter cauldron;

    //private float unlockRecipeSoundVolume = 2f;

    public event EventHandler OnRecipeUnlocked;
    public event EventHandler<int> OnNotEnoughMoney;

    void Start()
    {
        if (recipeBook != null)
        {
            recipeBook.InitializeBook();
        }
        cauldron.OnRecipeSuccess += OnCauldronRecipeSuccess;
    }
    private void OnCauldronRecipeSuccess(object sender, PotionRecipeSO craftRecipe)
    {
        // Ищем этот рецепт в книге
        for (int i = 0; i < recipeBook.bookPages.Length; i++)
        {
            if (recipeBook.bookPages[i] == craftRecipe)
            {
                Debug.Log($"RecipeManager: Recipe: {recipeBook.unlockedStates[i]}");
                //Debug.Log($"RecipeManager: Found unlock recipe on {i} page, unlocked: {recipeBook.unlockedStates[i]}");

                if (!recipeBook.unlockedStates[i])
                {
                    //Debug.Log($"RecipeManager: Recipe unlock!");
                    UnlockRecipeByIndex(i, true);
                }
                else
                {
                    //Debug.Log("RecipeManager: Recipe allready unlock");
                }
                break;
            }
        }
    }

    // Left Button
    public void UnlockLeftRecipe()
    {
        int targetPage = recipeBook.GetLeftUnlockTarget();
        int cost = recipeBook.GetLeftRecipeCost();
        if (PlayerWallet.Instance.HasEnoughMoney(cost))
        {
            if (PlayerWallet.Instance.SpendMoney(cost))
            {
                UnlockRecipeByIndex(targetPage, false);
                // bye sound
            }
           
            
        }
        else 
        { 
            OnNotEnoughMoney?.Invoke(this, cost);

            Debug.Log($"Не хватает денег! Нужно: {cost}, есть: {PlayerWallet.Instance.GetBalance()}");
        }
        
    }

    // Right Button  
    public void UnlockRightRecipe()
    {
        int targetPage = recipeBook.GetRightUnlockTarget();
        int cost = recipeBook.GetRightRecipeCost();

        if (PlayerWallet.Instance.HasEnoughMoney(cost))
        {
            if (PlayerWallet.Instance.SpendMoney(cost))
            {
                UnlockRecipeByIndex(targetPage, false);
            }
        }
        else
        {
            OnNotEnoughMoney?.Invoke(this, cost);
            Debug.Log($"Не хватает денег! Нужно: {cost}, есть: {PlayerWallet.Instance.GetBalance()}");
        }
    }

    private void UnlockRecipeByIndex(int pageIndex, bool isFree = false)
    {
        if (pageIndex >= 0 && pageIndex < recipeBook.bookPages.Length)
        {
            PotionRecipeSO recipe = recipeBook.bookPages[pageIndex];
            //recipe.isUnlocked = true;
            recipeBook.unlockedStates[pageIndex] = true;
            recipeBook.UpdateSprites();
            recipeBook.UpdateUnlockButton(); // Refresh buttons
            OnRecipeUnlocked?.Invoke(this, EventArgs.Empty);
            SoundManager.Instance.PlaySound(SoundType.RecipeUnlock, PlayerController.Instance.transform.position);
            
        }
    }

}
