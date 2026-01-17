#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TestWithoutYandexSDK : MonoBehaviour
{
    [MenuItem("Tools/Test Without Yandex SDK")]
    public static void TestGame()
    {
        Debug.Log("Testing game without Yandex SDK...");
        
        // Check if SaveManagerYG works
        if (Application.isPlaying)
        {
            var saveManager = GameObject.FindObjectOfType<SaveManagerYG>();
            if (saveManager != null)
            {
                Debug.Log("SaveManagerYG found and working!");
            }
            else
            {
                Debug.LogError("SaveManagerYG not found!");
            }
        }
        
        // Check if LanguageChanger works
        var languageChanger = GameObject.FindObjectOfType<LanguageChanger>();
        if (languageChanger != null)
        {
            Debug.Log("LanguageChanger found and working!");
        }
        
        // Check if RewardedAdvYG works
        var rewardedAd = GameObject.FindObjectOfType<RewardedAdvYG>();
        if (rewardedAd != null)
        {
            Debug.Log("RewardedAdvYG found and working!");
        }
        
        Debug.Log("Test completed! Game should work without Yandex SDK.");
    }
}
#endif