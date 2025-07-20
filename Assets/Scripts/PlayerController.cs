using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameInput gameInput;

    public event Action<bool> OnWalkingStateChanged;

    private bool isWalking;

    private float playerRadius = 0.2f;
    private float playerHeight = 2f;

    private Vector3 lastInteractDirection;
    private void Update()
    {
        HandleMovement(); 
        HandleInteractions();
        
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }

        float interactDistance = 0.5f;

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHitObject, interactDistance)) {
            if (raycastHitObject.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter component
                clearCounter.Interact();
            }
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveSpeed * Time.deltaTime);
        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f); // .normalized если нужна обычная скорость по диагонали
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveSpeed * Time.deltaTime);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z); //.normalized если нужна обычная скорость по диагонали
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveSpeed * Time.deltaTime);
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

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
