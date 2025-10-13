using System.Linq;
using UnityEngine;

public class SaveManagerTest : MonoBehaviour
{
    public void DebugSaveInfo()
    {
        var data = GameSaveManager.Instance.GetCurrentSaveData();
        Debug.Log($"💰 Деньги: {data.playerBalance}");
        Debug.Log($"🎵 Музыка: {data.musicVolume}");
        Debug.Log($"🔊 Звуки: {data.sfxVolume}");
        Debug.Log($"📖 Рецептов открыто: {data.unlockedRecipes?.Count(x => x)}/{data.unlockedRecipes?.Length}");
    }
}
