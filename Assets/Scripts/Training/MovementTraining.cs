using UnityEngine;
using System;

public class MovementTraining : MonoBehaviour
{
    public event EventHandler OnMovementTrainingCompleted;

    [SerializeField] private GameInput gameInput;

    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;

    private void Update()
    {
        CheckMovementKeys();
    }

    private void CheckMovementKeys()
    {
        Vector2 movement = gameInput.GetMovementVectorNormalized();

        if (movement.y > 0.1f) wPressed = true;
        if (movement.y < -0.1f) sPressed = true;
        if (movement.x < -0.1f) aPressed = true;
        if (movement.x > 0.1f) dPressed = true;

        if (wPressed && aPressed && sPressed && dPressed)
        {
            OnMovementTrainingCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}