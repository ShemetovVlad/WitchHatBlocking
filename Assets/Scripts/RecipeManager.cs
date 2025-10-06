using UnityEngine;
using System;

public class RecipeManager : MonoBehaviour
{
    public Book recipeBook;  // Book reference
    public CauldronCounter cauldron;

    public event EventHandler OnRecipeUnlocked;

    void Start()
    {
        // Подписываемся на событие успешного создания зелья
        cauldron.OnRecipeSuccess += OnCauldronRecipeSuccess;
    }
    private void OnCauldronRecipeSuccess(object sender, KitchenObjectSO createdPotion)
    {
        // Ищем этот рецепт в книге
        for (int i = 0; i < recipeBook.bookPages.Length; i++)
        {
            if (recipeBook.bookPages[i] == createdPotion && !recipeBook.unlockedStates[i])
            {
                // Открываем рецепт БЕСПЛАТНО (игнорируем стоимость)
                UnlockRecipeByIndex(i, true);
                break;
            }
        }
    }

    // Метод для кнопки слева
    public void UnlockLeftRecipe()
    {
        int targetPage = recipeBook.GetLeftUnlockTarget();
        UnlockRecipeByIndex(targetPage);
    }

    // Метод для кнопки справа  
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
            recipeBook.UpdateUnlockButton(); // Обновляем обе кнопки
        }
    }

}
