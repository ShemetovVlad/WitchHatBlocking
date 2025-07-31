using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu()]
public class RecipesSO : ScriptableObject
{
    public List<PotionRecipeSO> recipesSOList;
}
