using UnityEngine;
using YG;

public class RewardedAdvYG : MonoBehaviour
{
    [SerializeField] private int coinsReward;
    public string idAdv;

    private void OnEnable()
    {
        YG2.onRewardAdv += OnRewarded;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewarded;
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
}
