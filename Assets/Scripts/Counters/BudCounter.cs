using UnityEngine;
using System;

public class BudCounter : BaseCounter
{
    public event EventHandler OnBerrySpawned;
    public event EventHandler OnBerryRemoved;

    [SerializeField] private KitchenObjectSO berryKitchenObjectSO;
    private float spawnBerryTimer;
    private float spawnBerryTimerMax = 4f;
    private int spawnBerryAmount;
    private int spawnBerryAmountMax = 4;

    private void Update()
    {
        spawnBerryTimer += Time.deltaTime;
        if (spawnBerryTimer > spawnBerryTimerMax) 
        { 
            spawnBerryTimer = 0f;
            if (spawnBerryAmount < spawnBerryAmountMax)
            {
                spawnBerryAmount++;
                OnBerrySpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            if (spawnBerryAmount > 0)
            {
                spawnBerryAmount--;
                SoundManager.Instance.PlaySound(SoundType.BerryHarvest, transform.position, 2f);
                KitchenObject.SpawnKitchenObject(berryKitchenObjectSO, player);
                OnBerryRemoved?.Invoke(this, EventArgs.Empty);
                spawnBerryTimer = 0f;
            }
        }
        ;
    }

}
