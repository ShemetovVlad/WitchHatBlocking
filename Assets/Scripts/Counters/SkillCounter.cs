using UnityEngine;

public class SkillCounter : BaseCounter
{
    public override void Interact(PlayerController player)
    {
        ;
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
