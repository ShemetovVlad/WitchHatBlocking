using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public Book recipeBook;  // Book reference

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

    private void UnlockRecipeByIndex(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < recipeBook.bookPages.Length)
        {
            RecipePageSO recipe = recipeBook.bookPages[pageIndex];
            //recipe.isUnlocked = true;
            recipeBook.unlockedStates[pageIndex] = true;
            recipeBook.UpdateSprites();
            recipeBook.UpdateUnlockButton(); // Обновляем обе кнопки
        }
    }

}
