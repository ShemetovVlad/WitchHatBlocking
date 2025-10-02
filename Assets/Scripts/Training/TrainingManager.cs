using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private MovementTraining movementTraining;
    [SerializeField] private GameObject interactTraining;
    [SerializeField] private GameObject interactAlternateTraining;
    [SerializeField] private GameObject destroyObjectTraining;

    private bool interactPressed = false;
    private bool interactAlternatePressed = false;
    private bool destroyObjectPressed = false;

    private void Start()
    {
        // Подписываемся на событие завершения движения
        movementTraining.OnMovementTrainingCompleted += OnMovementTrainingCompleted;

        // Подписываемся на остальные события
        if (interactTraining != null)
        {
            gameInput.OnInteractAction += OnInteract;
        }
        else
        {
            interactPressed = true;
        }

        if (interactTraining != null)
        {
            gameInput.OnInteractAlternateAction += OnInteractAlternate;
        }
        else
        {
            interactAlternatePressed = true;
        }

        if (interactTraining != null)
        {
            gameInput.OnDestroyObjectAction += OnDestroyObject;
        }
        else
        {
            destroyObjectPressed = true;
        }
        
    }

    private void OnMovementTrainingCompleted(object sender, System.EventArgs e)
    {
        movementTraining.gameObject.SetActive(false);
        CheckAllTrainingCompleted();
    }

    private void OnInteract(object sender, System.EventArgs e)
    {
        if (!interactPressed)
        {
            interactPressed = true;
            interactTraining.SetActive(false);
            CheckAllTrainingCompleted();
        }
    }

    private void OnInteractAlternate(object sender, System.EventArgs e)
    {
        if (!interactAlternatePressed)
        {
            interactAlternatePressed = true;
            interactAlternateTraining.SetActive(false);
            CheckAllTrainingCompleted();
        }
    }

    private void OnDestroyObject(object sender, System.EventArgs e)
    {
        if (!destroyObjectPressed)
        {
            destroyObjectPressed = true;
            destroyObjectTraining.SetActive(false);
            CheckAllTrainingCompleted();
        }
    }

    private void CheckAllTrainingCompleted()
    {
        if (!movementTraining.gameObject.activeSelf &&
            interactPressed &&
            interactAlternatePressed &&
            destroyObjectPressed)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (movementTraining != null)
            movementTraining.OnMovementTrainingCompleted -= OnMovementTrainingCompleted;

        if (gameInput != null)
        {
            gameInput.OnInteractAction -= OnInteract;
            gameInput.OnInteractAlternateAction -= OnInteractAlternate;
            gameInput.OnDestroyObjectAction -= OnDestroyObject;
        }
    }
}