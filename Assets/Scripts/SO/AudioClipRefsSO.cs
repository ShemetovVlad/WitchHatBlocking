using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipRefsSO", menuName = "AudioClipRefsSO")]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] footstep;
    [Range(0f, 2f)]
    public float footstepVolume = 1f;
    public AudioClip[] chopping;
    [Range(0f, 2f)]
    public float choppingVolume = 1f;
    public AudioClip[] recipeSuccess;
    [Range(0f, 2f)]
    public float recipeSuccessVolume = 1f;
    public AudioClip[] recipeFail;
    [Range(0f, 2f)]
    public float recipeFailVolume = 1f;
    public AudioClip[] objectDrop;
    [Range(0f, 2f)]
    public float objectDropVolume = 1f;
    public AudioClip[] objectPickup;
    [Range(0f, 2f)]
    public float objectPickupVolume = 1f;
    public AudioClip[] labOn;
    [Range(0f, 2f)]
    public float labOnVolume = 1f;
    public AudioClip[] labOff;
    [Range(0f, 2f)]
    public float labOffVolume = 1f;
    public AudioClip[] warning;
    [Range(0f, 2f)]
    public float warningVolume = 1f;
    public AudioClip[] addIngredient;
    [Range(0f, 2f)]
    public float addIgredientVolume = 1f;
    public AudioClip[] buttonClick;
    [Range(0f, 2f)]
    public float buttonClickVolume = 1f;
}
