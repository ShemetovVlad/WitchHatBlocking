using UnityEngine;

public class StirringAnimationController : MonoBehaviour
{
    [SerializeField] private CauldronCounter cauldronCounter;
    private Animation stirringAnimation;

    private void Start()
    {
        stirringAnimation = GetComponent<Animation>();
        stirringAnimation.Stop();
        cauldronCounter.OnIngredientAdded += OnIngredientAdded;
        cauldronCounter.OnCauldronCleared += OnCauldronCleared;
    }

    private void OnIngredientAdded(object sender, KitchenObjectSO ingredientSO)
    {
        if (stirringAnimation != null)
        {
            stirringAnimation.Play();
        }
    }

    private void OnCauldronCleared(object sender, System.EventArgs e)
    {
        if (stirringAnimation != null)
        {
            stirringAnimation.Stop();
        }
    }
}