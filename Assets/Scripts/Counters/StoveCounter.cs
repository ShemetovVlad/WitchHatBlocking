using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Boiling,
        Boiled,
        Burned
    } 

    [SerializeField] private BoilRecipeSO[] boilRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float boilTimer;
    private float burningTimer;
    private BoilRecipeSO boilRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
        {
                case State.Idle:
                     
                break;
                
                case State.Boiling:
                    boilTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = boilTimer / boilRecipeSO.boilTimerMax
                    });

                    if (boilTimer > boilRecipeSO.boilTimerMax)
                    {
                    // boil
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(boilRecipeSO.output, this);
                        
                        state = State.Boiled;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                break;
            
                case State.Boiled:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        // burn
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
            
                case State.Burned:
                    
                break;
        }
            //Debug.Log(state);
        }
    }
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            // There is no objects here
            if (player.HasKitchenObject())
            {
                // Player carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // if player carrying something that can be boil
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    boilRecipeSO = GetBoilRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Boiling;
                    boilTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = boilTimer / boilRecipeSO.boilTimerMax
                    });
                }

            }

        }
        else
        {
            // There is a object here
            if (!player.HasKitchenObject())
            {
                // Player dont carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        BoilRecipeSO boilRecipeSO = GetBoilRecipeSOWithInput(inputKitchenObjectSO);
        return boilRecipeSO != null;
    }
    private BoilRecipeSO GetBoilRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BoilRecipeSO boilRecipeSO in boilRecipeSOArray)
        {
            if (boilRecipeSO.input == inputKitchenObjectSO)
            {
                return boilRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
