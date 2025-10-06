using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PotionRecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObjectSO> ingredients;  
    public KitchenObjectSO result;

    public Sprite openRecipeSprite;    
    public Sprite closedRecipeSprite;   
    public bool isUnlocked;            
    public int unlockCost;
}
