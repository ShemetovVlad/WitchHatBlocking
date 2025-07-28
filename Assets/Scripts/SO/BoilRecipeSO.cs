using UnityEngine;

[CreateAssetMenu()]
public class BoilRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float boilTimerMax;
}
