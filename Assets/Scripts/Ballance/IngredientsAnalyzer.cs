using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IngredientAnalyzer : MonoBehaviour
{
    [Header("Data References")]
    [SerializeField] private RecipesSO allRecipes;
    [SerializeField] private List<KitchenObjectSO> allIngredients; // Все существующие ингредиенты

    [Header("Analysis Settings")]
    [SerializeField] private bool analyzeOnStart = true;
    [SerializeField] private bool showUnusedIngredients = true;

    private void Start()
    {
        if (analyzeOnStart)
        {
            AnalyzeAllIngredients();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AnalyzeAllIngredients();
        }
    }

    [ContextMenu("Analyze All Ingredients")]
    public void AnalyzeAllIngredients()
    {
        if (allRecipes == null || allIngredients == null || allIngredients.Count == 0)
        {
            Debug.LogError("Missing data references!");
            return;
        }

        Debug.Log("=== ПОЛНЫЙ АНАЛИЗ ИНГРЕДИЕНТОВ ===");

        // Создаем словарь для учета использования
        Dictionary<KitchenObjectSO, List<PotionRecipeSO>> ingredientUsage = new Dictionary<KitchenObjectSO, List<PotionRecipeSO>>();

        // Инициализируем все ингредиенты (даже неиспользуемые)
        foreach (var ingredient in allIngredients)
        {
            if (ingredient != null)
            {
                ingredientUsage[ingredient] = new List<PotionRecipeSO>();
            }
        }

        // Заполняем данные об использовании
        foreach (var recipe in allRecipes.recipesSOList)
        {
            if (recipe.ingredients == null) continue;

            foreach (var ingredient in recipe.ingredients)
            {
                if (ingredient != null && ingredientUsage.ContainsKey(ingredient))
                {
                    ingredientUsage[ingredient].Add(recipe);
                }
            }
        }

        // Разделяем на используемые и неиспользуемые
        var usedIngredients = ingredientUsage.Where(x => x.Value.Count > 0)
                                           .OrderByDescending(x => x.Value.Count);
        var unusedIngredients = ingredientUsage.Where(x => x.Value.Count == 0)
                                             .OrderBy(x => x.Key.objectName);

        // Выводим используемые ингредиенты
        Debug.Log($"🟢 ИСПОЛЬЗУЕМЫЕ ИНГРЕДИЕНТЫ ({usedIngredients.Count()}):");
        foreach (var entry in usedIngredients)
        {
            var ingredient = entry.Key;
            var recipes = entry.Value;

            Debug.Log($"🍵 <color=cyan>{ingredient.objectName}</color>");
            Debug.Log($"   Используется в: <color=yellow>{recipes.Count}</color> рецептах");

            Debug.Log("   Рецепты:");
            foreach (var recipe in recipes)
            {
                string resultInfo = recipe.result != null ? $" → {recipe.result.objectName}" : " → ???";
                Debug.Log($"   • {recipe.recipeName}{resultInfo}");
            }
            Debug.Log("");
        }

        // Выводим неиспользуемые ингредиенты
        if (showUnusedIngredients && unusedIngredients.Any())
        {
            Debug.Log($"🔴 НЕИСПОЛЬЗУЕМЫЕ ИНГРЕДИЕНТЫ ({unusedIngredients.Count()}):");
            foreach (var entry in unusedIngredients)
            {
                var ingredient = entry.Key;
                Debug.Log($"❌ {ingredient.objectName} - цена: {ingredient.price}");
            }
            Debug.Log("");
        }

        // Статистика
        ShowStatistics(usedIngredients, unusedIngredients);
    }

    private void ShowStatistics(IOrderedEnumerable<KeyValuePair<KitchenObjectSO, List<PotionRecipeSO>>> usedIngredients,
                               IEnumerable<KeyValuePair<KitchenObjectSO, List<PotionRecipeSO>>> unusedIngredients)
    {
        Debug.Log("=== СТАТИСТИКА ===");
        Debug.Log($"📊 Всего ингредиентов: {allIngredients.Count}");
        Debug.Log($"🟢 Используется: {usedIngredients.Count()}");
        Debug.Log($"🔴 Не используется: {unusedIngredients.Count()}");

        float usagePercentage = (float)usedIngredients.Count() / allIngredients.Count * 100;
        Debug.Log($"📈 Процент использования: {usagePercentage:F1}%");

        if (usedIngredients.Any())
        {
            var mostUsed = usedIngredients.First();
            Debug.Log($"🏆 Самый популярный: <color=green>{mostUsed.Key.objectName}</color> " +
                      $"(в {mostUsed.Value.Count} рецептах)");

            // Топ-5 самых используемых
            Debug.Log("🥇 Топ-5 самых используемых:");
            int count = 0;
            foreach (var ingredient in usedIngredients.Take(5))
            {
                count++;
                Debug.Log($"   {count}. {ingredient.Key.objectName} - {ingredient.Value.Count} рецептов");
            }
        }

        // Анализ цен неиспользуемых ингредиентов
        if (unusedIngredients.Any())
        {
            var expensiveUnused = unusedIngredients.OrderByDescending(x => x.Key.price).Take(3);
            Debug.Log("💎 Самые дорогие неиспользуемые ингредиенты:");
            foreach (var ingredient in expensiveUnused)
            {
                Debug.Log($"   • {ingredient.Key.objectName} - цена: {ingredient.Key.price}");
            }
        }
    }

    // Метод для быстрой проверки конкретного ингредиента
    [ContextMenu("Find Unused Ingredients")]
    public void FindUnusedIngredients()
    {
        if (allRecipes == null || allIngredients == null) return;

        var unused = allIngredients.Where(ingredient =>
            !allRecipes.recipesSOList.Any(recipe =>
                recipe.ingredients != null && recipe.ingredients.Contains(ingredient))
        ).ToList();

        Debug.Log($"🔍 НЕИСПОЛЬЗУЕМЫЕ ИНГРЕДИЕНТЫ ({unused.Count}):");
        foreach (var ingredient in unused)
        {
            Debug.Log($"❌ {ingredient.objectName} (цена: {ingredient.price})");
        }
    }

    // Метод для поиска ингредиентов по минимальному количеству использований
    public void FindIngredientsWithMinUsage(int minUsage)
    {
        if (allRecipes == null || allIngredients == null) return;

        var result = allIngredients.Where(ingredient =>
        {
            var usageCount = allRecipes.recipesSOList.Count(recipe =>
                recipe.ingredients != null && recipe.ingredients.Contains(ingredient));
            return usageCount >= minUsage;
        }).ToList();

        Debug.Log($"🔍 Ингредиенты используемые хотя бы в {minUsage} рецептах ({result.Count}):");
        foreach (var ingredient in result)
        {
            var usageCount = allRecipes.recipesSOList.Count(recipe =>
                recipe.ingredients != null && recipe.ingredients.Contains(ingredient));
            Debug.Log($"✅ {ingredient.objectName} - {usageCount} рецептов");
        }
    }
}