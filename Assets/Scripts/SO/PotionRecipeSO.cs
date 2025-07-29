using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PotionRecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> ingredients;  
    public KitchenObjectSO result;
}
