using UnityEngine;
using System;

public class RecipeManager : MonoBehaviour
{
    public Book recipeBook;
    public CauldronCounter cauldron; 

    public event EventHandler OnRecipeUnlocked;

    void Start()
    {
        cauldron.OnRecipeSuccess += OnCauldronRecipeSuccess;
    }
    private void OnCauldronRecipeSuccess(object sender, PotionRecipeSO craftRecipe)
    {
        // Ищем этот рецепт в книге
        for (int i = 0; i < recipeBook.bookPages.Length; i++)
        {
            if (recipeBook.bookPages[i] == craftRecipe)
            {
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
        UnlockRecipeByIndex(targetPage);
    }

    // Right Button  
    public void UnlockRightRecipe()
    {
        int targetPage = recipeBook.GetRightUnlockTarget();
        UnlockRecipeByIndex(targetPage);
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
        }
    }

}
