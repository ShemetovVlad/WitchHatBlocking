using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipRefsSO", menuName = "AudioClipRefsSO")]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] footstep;
    public AudioClip[] chopping;
    public AudioClip[] recipeSuccess;
    public AudioClip[] recipeFail;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip[] labOn;
    public AudioClip[] labOff;
    public AudioClip[] warning;
}
