using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (player == null) 
        {
            Debug.LogError("PlayerController не присвоен в инспекторе!", this);
            return;
        }
        
        player.OnWalkingStateChanged += HandleWalkingChange;
        player.OnCarryStateChanged += HandleCarryChange;
    }
    private void HandleWalkingChange(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }
    private void HandleCarryChange(bool isCarry)
    {
        animator.SetBool("IsCarry", isCarry);
    }

    private void OnDestroy()
    {
        // ќтписываемс€ при уничтожении объекта (важно!)
        PlayerController player = GetComponentInParent<PlayerController>();
        if (player != null)
            player.OnWalkingStateChanged -= HandleWalkingChange;
    }
    
    
    
    
    
    // ќпрашивает PlayerController каждый кадр, знает о его состо€нии (инкапсул€ци€), измен€ет значение переменной каждый кадр, даже на то же самое! ѕеределать на реакцию на событие.
    //private void Update()
    //{
    //    animator.SetBool("IsWalking", player.IsWalking());
    //}
}
