using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private SpeedPerk speedPerk;
    private float currentMoveSpeed = 0f;
    public event Action<bool> OnWalkingStateChanged;
    public event Action<bool> OnCarryStateChanged;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    private bool isWalking;
    private bool isCarry;

    private float playerRadius = 0.2f;
    private float playerHeight = 2f;

    private Vector3 lastInteractDirection;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        currentMoveSpeed = moveSpeed;

        if (Instance != null)
        {
            Debug.LogError("Больше одного игрока!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnDestroyObjectAction += GameInput_OnDestroyObjectAction;
    }

    private void GameInput_OnDestroyObjectAction(object sender, EventArgs e)
    {
        TryDestroyHeldObject();
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
            
        }
    }

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

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHitObject, interactDistance))
        {
            if (raycastHitObject.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter); // Has ClearCounter component

                }

            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);

        }
        //Debug.Log(selectedCounter);
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, currentMoveSpeed * Time.deltaTime);
        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f); // .normalized если нужна обычная скорость по диагонали
            canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, currentMoveSpeed * Time.deltaTime);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z); //.normalized если нужна обычная скорость по диагонали
                canMove = moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, currentMoveSpeed * Time.deltaTime);
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
            transform.position += moveDirection * currentMoveSpeed * Time.deltaTime;
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

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    private void TryDestroyHeldObject()
    {
        if (HasKitchenObject())
        {
            // Уничтожаем объект
            GetKitchenObject().DestroySelf();

            // Очищаем ссылку на объект
            ClearKitchenObject();
        }
    }
    private void OnEnable()
    {
        // Подписываемся на изменение множителя
        speedPerk.OnSpeedMultiplierChanged.AddListener(SetSpeed);
    }

    private void OnDisable()
    {
        // Важно отписаться!
        speedPerk.OnSpeedMultiplierChanged.RemoveListener(SetSpeed);
    }

    private void SetSpeed(float multiplier)
    {
        currentMoveSpeed = moveSpeed * multiplier;
        //Debug.Log($"Скорость изменена: {currentMoveSpeed}");
    }
    
    public void SetCarryState(bool isCarry)
    {
        OnCarryStateChanged?.Invoke(isCarry);
        Debug.Log(isCarry);
    }

}