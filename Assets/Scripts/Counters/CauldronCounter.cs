using System;
using System.Collections.Generic;
using UnityEngine;

public class CauldronCounter : BaseCounter
{
    [SerializeField] private RecipesSO possibleRecipes;
    private List<KitchenObjectSO> cauldronIngredientsSOList;

    private PlayerController lastPlayer;

    public event System.EventHandler<KitchenObjectSO> OnIngredientAdded;
    public event System.EventHandler OnCauldronCleared;
    private void Awake()
    {
        cauldronIngredientsSOList = new List<KitchenObjectSO>();
    }
    public override void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObjectSO kitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
            
            AddIgredient(kitchenObjectSO, player);
            
            //player.GetKitchenObject().DestroySelf();
 
        }
    }

    private void AddIgredient(KitchenObjectSO kitchenObjectSO, PlayerController player)
    {
        cauldronIngredientsSOList.Add(kitchenObjectSO);
        player.GetKitchenObject().DestroySelf();
        lastPlayer = player;   

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
                //Debug.Log($"Сварен рецепт: {recipe.name}! Получено: {recipe.result.objectName}");
                // Тут можно создать результат (recipe.result) и выдать игроку.
                if (lastPlayer != null) 
                { 
                    KitchenObject.SpawnKitchenObject(recipe.result, lastPlayer); 
                }
                
                cauldronIngredientsSOList.Clear();
                OnCauldronCleared?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        Debug.Log("Unknown recipe! Ingredients clear.");
        cauldronIngredientsSOList.Clear();
        OnCauldronCleared?.Invoke(this, EventArgs.Empty);

    }
    private bool IsRecipeMatch(List<KitchenObjectSO> recipeIngredients, List<KitchenObjectSO> cauldronIngredients)
    {
        if (recipeIngredients.Count != cauldronIngredients.Count)
            return false;

        // Make copy of lists to dont change origin lists.
        var recipeCopy = new List<KitchenObjectSO>(recipeIngredients);
        var cauldronCopy = new List<KitchenObjectSO>(cauldronIngredients);

        // Delete matched elements from list copy.
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
