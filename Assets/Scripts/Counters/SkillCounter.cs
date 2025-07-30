using UnityEngine;

public class SkillCounter : BaseCounter
{
    [SerializeField] private GameObject skillTree;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        playerController.OnWalkingStateChanged += CloseSkillTree;
    }

    private void CloseSkillTree(bool isWalking)
    {
        if (isWalking)
        {
            skillTree.SetActive(false);
        }
    }
    public override void Interact(PlayerController player)
    {
        skillTree.SetActive(true);
        
    }
}
