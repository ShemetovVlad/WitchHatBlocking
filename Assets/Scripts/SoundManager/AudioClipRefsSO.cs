using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Sound System/Audio Clip Refs")]
public class AudioClipRefsSO : ScriptableObject
{
    public SoundCategorySO[] soundCategories;

    // Метод для получения категории по типу
    public SoundCategorySO GetCategory(SoundType soundType)
    {
        return soundCategories.FirstOrDefault(category => category.soundType == soundType);
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (soundCategories != null)
        {
            var uniqueCategories = soundCategories.Distinct().ToArray();
            if (uniqueCategories.Length != soundCategories.Length)
            {
                soundCategories = uniqueCategories;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
