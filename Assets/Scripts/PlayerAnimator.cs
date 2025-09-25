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
            Debug.LogError("PlayerController �� �������� � ����������!", this);
            return;
        }
        
        player.OnWalkingStateChanged += HandleWalkingChange;
        player.OnKitchenObjectChanged += HandleKitchenObjectChange;
    }
    private void HandleWalkingChange(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }
    
    private void HandleKitchenObjectChange(bool hasObject)
    {
        animator.SetBool("HasObject", hasObject);
    }

    private void OnDestroy()
    {
        // ������������ ��� ����������� ������� (�����!)
        PlayerController player = GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.OnWalkingStateChanged -= HandleWalkingChange;
            player.OnKitchenObjectChanged -= HandleKitchenObjectChange;
        }
    }
    
    
    
    
    
    // ���������� PlayerController ������ ����, ����� � ��� ��������� (������������), �������� �������� ���������� ������ ����, ���� �� �� �� �����! ���������� �� ������� �� �������.
    //private void Update()
    //{
    //    animator.SetBool("IsWalking", player.IsWalking());
    //}
}
