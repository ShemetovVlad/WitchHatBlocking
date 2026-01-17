#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RemoveYandexSDK : MonoBehaviour
{
    [MenuItem("Tools/Remove Yandex SDK")]
    public static void RemoveYandexDefine()
    {
        string[] defineSymbols = new string[]
        {
            "YandexGamesPlatform_yg",
            "Authorization_yg",
            "InterstitialAdv_yg",
            "Localization_yg",
            "Payments_yg",
            "Review_yg",
            "RewardedAdv_yg",
            "Storage_yg"
        };

        foreach (BuildTargetGroup buildTarget in System.Enum.GetValues(typeof(BuildTargetGroup)))
        {
            if (buildTarget == BuildTargetGroup.Unknown) continue;

            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);
            bool changed = false;

            foreach (string define in defineSymbols)
            {
                if (defines.Contains(define))
                {
                    defines = defines.Replace(define + ";", "");
                    defines = defines.Replace(define, "");
                    changed = true;
                }
            }

            if (changed)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, defines);
            }
        }

        Debug.Log("Yandex SDK define symbols removed successfully!");
    }
}
#endif