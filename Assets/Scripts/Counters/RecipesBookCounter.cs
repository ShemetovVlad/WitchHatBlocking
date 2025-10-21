using UnityEngine;

public class RecipesBookCounter : BaseCounter
{
    [SerializeField] private GameObject bookCanvas;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameInput gameInput;

    private void Awake()
    {
        playerController.OnWalkingStateChanged += CloseBook;
        if (gameInput != null)
        {
            gameInput.OnPauseAction += GameInput_OnPauseAction;
        }
    }

    private void GameInput_OnPauseAction(object sender, System.EventArgs e)
    {
        // Просто передаем true (как будто игрок пошел)
        if (bookCanvas != null && bookCanvas.activeSelf)
        {
            CloseBook(true);
        }
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
        //Debug.Log("Recipes!");
    }
}
