using UnityEngine;
using UnityEngine.UI;

public class CauldronSingleUI : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
