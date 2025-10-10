using UnityEngine;
using System.Collections.Generic;

public class RecipesChecking : MonoBehaviour
{
    [Header("Recipe Data")]
    [SerializeField] private RecipesSO allRecipes;

    private int currentRecipeIndex = 0;
    private Rect recipeWindowRect = new Rect(10, 10, 400, 500);
    private Vector2 scrollPosition;

    private void OnGUI()
    {
        if (allRecipes == null || allRecipes.recipesSOList.Count == 0)
        {
            GUI.Label(new Rect(10, 10, 300, 20), "No recipes assigned!");
            return;
        }

        // Окно с рецептом
        recipeWindowRect = GUI.Window(0, recipeWindowRect, DrawRecipeWindow, "Recipe Book");

        // Кнопки переключения рецептов (отдельно от окна)
        DrawNavigationButtons();
    }

    private void DrawRecipeWindow(int windowID)
    {
        var currentRecipe = allRecipes.recipesSOList[currentRecipeIndex];

        GUILayout.Space(10);

        // Название рецепта
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
        GUILayout.Label(currentRecipe.recipeName, titleStyle);

        GUILayout.Space(20);

        // Изображение рецепта (если есть)
        if (currentRecipe.openRecipeSprite != null)
        {
            GUILayout.Label(currentRecipe.openRecipeSprite.texture, GUILayout.Width(200), GUILayout.Height(200));
        }
        else
        {
            GUILayout.Box("No Image", GUILayout.Width(200), GUILayout.Height(200));
        }

        GUILayout.Space(20);

        // Список ингредиентов с прокруткой
        GUILayout.Label("Ингредиенты:", GUI.skin.label);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));

        foreach (var ingredient in currentRecipe.ingredients)
        {
            if (ingredient != null)
            {
                GUILayout.Label($"• {ingredient.objectName}");
            }
            else
            {
                GUILayout.Label($"• Missing ingredient!");
            }
        }

        GUILayout.EndScrollView();

        // Результат зелья
        GUILayout.Space(10);
        if (currentRecipe.result != null)
        {
            GUILayout.Label($"Результат: {currentRecipe.result.objectName}");
        }
    }

    private void DrawNavigationButtons()
    {
        // Кнопка "Назад" слева от окна
        if (GUI.Button(new Rect(10, recipeWindowRect.yMax + 10, 80, 30), "← Previous"))
        {
            PreviousRecipe();
        }

        // Информация о текущем рецепте
        string recipeInfo = $"{currentRecipeIndex + 1} / {allRecipes.recipesSOList.Count}";
        GUI.Label(new Rect(100, recipeWindowRect.yMax + 15, 200, 20), recipeInfo);

        // Кнопка "Вперед" справа от окна
        if (GUI.Button(new Rect(330, recipeWindowRect.yMax + 10, 80, 30), "Next →"))
        {
            NextRecipe();
        }
    }

    private void Update()
    {
        // Переключение рецептов по клавишам
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousRecipe();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextRecipe();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // Рандомный рецепт для быстрого тестирования
            currentRecipeIndex = Random.Range(0, allRecipes.recipesSOList.Count);
        }
    }

    private void NextRecipe()
    {
        currentRecipeIndex++;
        if (currentRecipeIndex >= allRecipes.recipesSOList.Count)
        {
            currentRecipeIndex = 0;
        }
    }

    private void PreviousRecipe()
    {
        currentRecipeIndex--;
        if (currentRecipeIndex < 0)
        {
            currentRecipeIndex = allRecipes.recipesSOList.Count - 1;
        }
    }

    // Метод для получения текущего рецепта
    public PotionRecipeSO GetCurrentRecipe()
    {
        if (allRecipes.recipesSOList.Count == 0) return null;
        return allRecipes.recipesSOList[currentRecipeIndex];
    }
}
