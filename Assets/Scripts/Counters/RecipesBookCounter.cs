using UnityEngine;

public class RecipesBookCounter : BaseCounter
{
    [SerializeField] private GameObject bookCanvas;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        playerController.OnWalkingStateChanged += CloseBook;
    }

    private void CloseBook(bool isWalking)
    {
        if (isWalking) 
        { 
            bookCanvas.SetActive(false); 
        }
    }
    public override void Interact(PlayerController player)
    {
        bookCanvas.SetActive(true);
        Debug.Log("Recipes!");
    }
}
