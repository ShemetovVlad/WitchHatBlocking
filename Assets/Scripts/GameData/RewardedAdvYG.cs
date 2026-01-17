using UnityEngine;

public class RewardedAdvYG : MonoBehaviour
{
    [SerializeField] private int coinsReward;
    public string idAdv;

    private void OnEnable()
    {
        MockRewardedAd.OnRewardReceived += OnRewarded;
    }

    private void OnDisable()
    {
        MockRewardedAd.OnRewardReceived -= OnRewarded;
    }

    private void OnRewarded(string id)
    {
        if (id == idAdv)
        {
            SetReward();
        }
    }

    public void SetReward()
    {
        Debug.Log($"Награда получена! +{coinsReward} монет");
        PlayerWallet.Instance.AddMoney(coinsReward);
    }
    
    // Method to show the rewarded ad
    public void ShowRewardedAd()
    {
        MockRewardedAd.ShowRewardedAd(idAdv);
    }
}
