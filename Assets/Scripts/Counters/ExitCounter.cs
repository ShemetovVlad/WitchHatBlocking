using UnityEngine;

public class ExitCounter : BaseCounter
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject exitPopUp;

    private void Awake()
    {
        playerController.OnWalkingStateChanged += CloseSkillTree;
    }

    private void CloseSkillTree(bool isWalking)
    {
        if (isWalking)
        {
            exitPopUp.SetActive(false);
        }
    }
    public override void Interact(PlayerController player)
    {
        exitPopUp.SetActive(true);

    }
}
