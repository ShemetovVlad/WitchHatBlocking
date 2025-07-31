using System;
using System.Collections.Generic;
using UnityEngine;

public class CauldronCounter : BaseCounter
{
    [SerializeField] private RecipesSO possibleRecipes;
    private List<KitchenObjectSO> cauldronIngredientsSOList;

    public event System.EventHandler<KitchenObjectSO> OnIngredientAdded;
    private void Awake()
    {
        cauldronIngredientsSOList = new List<KitchenObjectSO>();
    }
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObjectSO kitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
            
            AddIgredient(kitchenObjectSO);
            
            player.GetKitchenObject().DestroySelf();
 
        }
    }

    private void AddIgredient(KitchenObjectSO kitchenObjectSO)
    {
        cauldronIngredientsSOList.Add(kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, kitchenObjectSO);

        if (cauldronIngredientsSOList.Count == 3)
            {
                Cook();   
            }
    }

    private void Cook()
    {
        foreach (var recipe in possibleRecipes.recipesSOList)
        {
            if (IsRecipeMatch(recipe.ingredients, cauldronIngredientsSOList))
            {
                Debug.Log($"Сварен рецепт: {recipe.name}! Получено: {recipe.result.objectName}");
                // Тут можно создать результат (recipe.result) и выдать игроку.
                KitchenObject.SpawnKitchenObject(recipe.result,this);
                cauldronIngredientsSOList.Clear();
                return;
            }
        }
        Debug.Log("Неизвестный рецепт! Ингредиенты сброшены.");
        cauldronIngredientsSOList.Clear();
        
    }
    private bool IsRecipeMatch(List<KitchenObjectSO> recipeIngredients, List<KitchenObjectSO> cauldronIngredients)
    {
        if (recipeIngredients.Count != cauldronIngredients.Count)
            return false;

        // Создаём копии списков, чтобы не менять оригиналы.
        var recipeCopy = new List<KitchenObjectSO>(recipeIngredients);
        var cauldronCopy = new List<KitchenObjectSO>(cauldronIngredients);

        // Удаляем совпадающие элементы из копий.
        foreach (var ingredient in recipeIngredients)
        {
            if (!cauldronCopy.Contains(ingredient))
                return false;
            cauldronCopy.Remove(ingredient);
        }

        return cauldronCopy.Count == 0;
    }

    public List<KitchenObjectSO> GetIngredients() => new List<KitchenObjectSO>(cauldronIngredientsSOList);
}
