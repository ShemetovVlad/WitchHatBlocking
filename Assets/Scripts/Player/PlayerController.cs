using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private SpeedPerk speedPerk;
    [SerializeField] private ParticleSystem speedBoosterVFX;
    private float currentMoveSpeed = 0f;
    public event Action<bool> OnWalkingStateChanged;
    public event Action<bool> OnKitchenObjectChanged;
    public event Action OnDestroyObjectAction;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    private bool isWalking;

    private float playerRadius = 0.2f;
    private float playerHeight = 2f;

    private Vector3 lastInteractDirection;
    private Coroutine speedBoostCoroutine;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        currentMoveSpeed = moveSpeed;
        speedBoosterVFX.gameObject.SetActive(false);

        if (Instance != null)
        {
            Debug.LogError("?????? ?????? ??????!");
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
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f); // .normalized ???? ????? ??????? ???????? ?? ?????????
            canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, currentMoveSpeed * Time.deltaTime);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z); //.normalized ???? ????? ??????? ???????? ?? ?????????
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
        OnKitchenObjectChanged?.Invoke(true);
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
        OnKitchenObjectChanged?.Invoke(false);
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    private void TryDestroyHeldObject()
    {
        if (HasKitchenObject())
        {
            float DestroyAnimationTime = 0.5f;
            
            StartCoroutine(DestroyObjectWithDelay(DestroyAnimationTime));
            OnDestroyObjectAction?.Invoke();
        }
    }
    private void OnEnable()
    {
        // ????????????? ?? ????????? ?????????
        speedPerk.OnSpeedMultiplierChanged.AddListener(SetSpeed);
    }

    private IEnumerator DestroyObjectWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();
            ClearKitchenObject();
        }
    }
    private void OnDisable()
    {
        // ????? ??????????!s
        speedPerk.OnSpeedMultiplierChanged.RemoveListener(SetSpeed);
    }

    public void SetSpeed(float multiplier)
    {
        currentMoveSpeed = moveSpeed * multiplier;
        //Debug.Log($"???????? ????????: {currentMoveSpeed}");
    }
    public void SpeedBuster(float multiplier, float duration)
    {
        // Останавливаем предыдущий бустер, если он активен
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        // Применяем бустер и запускаем корутину для сброса
        speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }
    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        currentMoveSpeed = moveSpeed * multiplier;
        speedBoosterVFX.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        // Возвращаем исходную скорость
        currentMoveSpeed = moveSpeed;
        speedBoosterVFX.gameObject.SetActive(false);
        speedBoostCoroutine = null;
    }
}