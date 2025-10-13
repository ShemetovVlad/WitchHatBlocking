using UnityEngine;

public class ExitCounter : BaseCounter
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject exitPopUp;

    private void Awake()
    {
        playerController.OnWalkingStateChanged += CloseExitMenu;
    }

    private void CloseExitMenu(bool isWalking)
    {
        if (isWalking)
        {
            exitPopUp.SetActive(false);
        }
    }
    public override void Interact(PlayerController player)
    {
        exitPopUp.SetActive(true);
        //Debug.Log(exitPopUp.gameObject.active);
    }
}
