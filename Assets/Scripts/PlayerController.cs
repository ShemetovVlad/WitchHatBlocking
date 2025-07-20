using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameInput gameInput;

    public event Action<bool> OnWalkingStateChanged;

    private bool isWalking;

    private float playerRadius = 0.2f;
    private float playerHeight = 2f;
    
    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); 
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveSpeed * Time.deltaTime);

        if (canMove)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        bool newState = moveDirection != Vector3.zero;                                    
        if (newState != isWalking)
        {
            isWalking = newState;
            OnWalkingStateChanged?.Invoke(isWalking);
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, -moveDirection, Time.deltaTime * rotateSpeed);
        
    }






    //public bool IsWalking()
    //{
    //    return isWalking;
    //}
}
