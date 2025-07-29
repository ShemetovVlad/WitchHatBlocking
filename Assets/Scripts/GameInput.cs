using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnDestroyObjectAction;

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.DestroyObject.performed += DestroyObject_performed;
    }

    private void DestroyObject_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDestroyObjectAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        
    }

    public Vector2 GetMovementVectorNormalized() 
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>(); 
        
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
