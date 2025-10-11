using UnityEngine;

[CreateAssetMenu(fileName = "SoundCategory", menuName = "Sound System/Sound Category")]
public class SoundCategorySO : ScriptableObject
{
    public AudioClip[] clips;
    [Range(0f, 2f)] public float volume = 1f;
    public SoundType soundType; // ← связываем категорию с enum
}