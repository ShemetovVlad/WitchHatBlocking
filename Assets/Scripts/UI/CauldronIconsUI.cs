using Unity.VisualScripting;
using UnityEngine;

public class CauldronIconsUI : MonoBehaviour
{
    [SerializeField] private CauldronCounter cauldronCounter;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        cauldronCounter.OnIngredientAdded += OnIngredientAddedToCauldron;
        cauldronCounter.OnCauldronCleared += OnCauldronCleared;
    }

    private void OnIngredientAddedToCauldron(object sender, KitchenObjectSO ingredientSO)
    {
        CancelInvoke(nameof(ClearIconsImmediately));
        UpdateVisual();
    }
    private void OnCauldronCleared(object sender, System.EventArgs e)
    {
        ClearIconsWithDelay();
    }

    private void ClearIconsWithDelay()
    {
        Invoke(nameof(ClearIconsImmediately), 1f);
    }

    private void ClearIconsImmediately()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
    }
    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in cauldronCounter.GetIngredients())
        {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);   
            iconTransform.GetComponent<CauldronSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
            
    }
}
