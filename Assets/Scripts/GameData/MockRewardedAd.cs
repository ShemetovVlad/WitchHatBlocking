using System;
using UnityEngine;

public class MockRewardedAd : MonoBehaviour
{
    public static event Action<string> OnRewardReceived;
    public static event Action OnAdOpened;
    public static event Action OnAdClosed;
    public static event Action OnAdFailed;
    
    private static bool isAdReady = true;
    
    public static void ShowRewardedAd(string adId)
    {
        if (!isAdReady)
        {
            Debug.LogWarning("Rewarded ad is not ready yet!");
            OnAdFailed?.Invoke();
            return;
        }
        
        // Notify that ad is opening
        OnAdOpened?.Invoke();
        isAdReady = false;
        
        Debug.Log($"Showing mock rewarded ad with ID: {adId}");
        
        // Simulate ad showing with a delay
        Instance.StartCoroutine(Instance.SimulateAdShowing(adId));
    }
    
    public static bool IsAdReady()
    {
        return isAdReady;
    }
    
    private static MockRewardedAd instance;
    private static MockRewardedAd Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MockRewardedAd");
                instance = go.AddComponent<MockRewardedAd>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    
    private System.Collections.IEnumerator SimulateAdShowing(string adId)
    {
        // Simulate ad duration (2-3 seconds)
        float adDuration = UnityEngine.Random.Range(2f, 3f);
        yield return new WaitForSeconds(adDuration);
        
        // Simulate reward (80% success rate)
        bool rewardGiven = UnityEngine.Random.Range(0, 100) < 80;
        
        if (rewardGiven)
        {
            Debug.Log($"Reward given for ad ID: {adId}");
            OnRewardReceived?.Invoke(adId);
        }
        else
        {
            Debug.Log($"No reward given for ad ID: {adId}");
            OnAdFailed?.Invoke();
        }
        
        // Notify that ad is closed
        OnAdClosed?.Invoke();
        
        // Simulate ad cooldown
        yield return new WaitForSeconds(5f);
        isAdReady = true;
        Debug.Log("Mock rewarded ad is ready again");
    }
}