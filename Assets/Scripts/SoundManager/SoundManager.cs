using System.Collections;
using System.Linq;
using UnityEngine;

public enum SoundType
{
    AddIngredient,
    RecipeSuccess,
    RecipeFail,
    Chop,
    ButtonClick,
    ObjectDestroy,
    SellingIngredient,
    SellingPotionCheap,
    SellingPotionExpensive,
    RecipeUnlock,
    SecretRecipe,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private CauldronCounter cauldronCounter;
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private PlayerController player;
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;

    private void Start()
    {
        cauldronCounter.OnIngredientAdded += CauldronCounter_OnIngredientAdded;
        cauldronCounter.OnRecipeSuccess += CauldronCounter_OnRecipeSuccess;
        cauldronCounter.OnRecipeFailed += CauldronCounter_OnRecipeFailed;
        cuttingCounter.OnCut += CuttingCounter_OnCut;
        player.OnDestroyObjectAction += Player_OnDestroyObjectAction;
    }
    private void CauldronCounter_OnIngredientAdded(object sender, KitchenObjectSO ingredientSO)
    {
        PlaySound(SoundType.AddIngredient, cauldronCounter.transform.position);
    }
    private void CauldronCounter_OnRecipeSuccess(object sender, PotionRecipeSO potionRecipeSO)
    {
        if (potionRecipeSO.recipeName.Contains("Secret"))
        {
            PlaySound(SoundType.SecretRecipe, cauldronCounter.transform.position);
        }
        else
        {
            PlaySound(SoundType.RecipeSuccess, cauldronCounter.transform.position);
        }
    }
    private void Player_OnDestroyObjectAction()
    {
        float delay = 0.3f;
        PlaySoundWithDelay(SoundType.ObjectDestroy, player.transform.position, delay, 2f);
    }
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        PlaySound(SoundType.Chop, cuttingCounter.transform.position);
    }
    private void CauldronCounter_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(SoundType.RecipeFail, Camera.main.transform.position);
    }
    public void PlaySound(SoundType soundType, Vector3 position, float volumeMultiplier = 1f)
    {
        SoundCategorySO category = audioClipRefsSO.GetCategory(soundType);

        if (category != null && category.clips != null && category.clips.Length > 0)
        {
            float finalVolume = volumeMultiplier * category.volume * sfxVolume * masterVolume;
            AudioClip randomClip = category.clips[Random.Range(0, category.clips.Length)];
            AudioSource.PlayClipAtPoint(randomClip, position, finalVolume);
        }
        else
        {
            Debug.LogWarning($"Sound category for {soundType} not found!");
        }
    }
    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip != null)
        {
            float finalVolume = volume * sfxVolume * masterVolume;
            AudioSource.PlayClipAtPoint(clip, position, finalVolume);
        }
    }
    public void PlaySoundWithDelay(SoundType soundType, Vector3 position, float delay, float volumeMultiplier = 1f)
    {
        StartCoroutine(PlaySoundDelayedCoroutine(soundType, position, delay, volumeMultiplier));
    }

    private IEnumerator PlaySoundDelayedCoroutine(SoundType soundType, Vector3 position, float delay, float volumeMultiplier)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(soundType, position, volumeMultiplier);
    }
}
