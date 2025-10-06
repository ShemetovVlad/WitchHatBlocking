using UnityEngine;

[CreateAssetMenu(fileName = "RecipePageSO", menuName = "RecipePageSO")]
public class RecipePageSO : ScriptableObject
{
    public Sprite openRecipeSprite;    // Recipe sprite
    public Sprite closedRecipeSprite;  // Close recipe sprite
    public bool isUnlocked;            // Is the recipe unlocked?
    public int unlockCost;            // Cost to unlock the recipe
}
