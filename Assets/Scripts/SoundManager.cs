using System.Collections;
using System.Linq;
using UnityEngine;

public enum SoundType
{
    IngredientAdd,
    RecipeSuccess,
    RecipeFail,
    Chop,
    ButtonClick,
    ObjectDestroy,
    SellingIngredient,
    SellingPotionCheap,
    SellingPotionExpensive,
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

    private void Start()
    {
        cauldronCounter.OnIngredientAdded += CauldronCounter_OnIngredientAdded;
        //cauldronCounter.OnRecipeSuccess += CauldronCounter_OnRecipeSuccess;
        cauldronCounter.OnRecipeFailed += CauldronCounter_OnRecipeFailed;
        cuttingCounter.OnCut += CuttingCounter_OnCut;
        player.OnDestroyObjectAction += Player_OnDestroyObjectAction;
    }
    private void CauldronCounter_OnIngredientAdded(object sender, KitchenObjectSO ingredientSO)
    {
        PlaySound(audioClipRefsSO.addIngredient, cauldronCounter.transform.position);
    }
    private void CauldronCounter_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.recipeSuccess, cauldronCounter.transform.position);
    }
    private void Player_OnDestroyObjectAction()
    {
        float delay = 0.3f;
        PlaySoundWithDelay(audioClipRefsSO.objectDestroy, player.transform.position, delay, 2f);
    }
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.chopping, cuttingCounter.transform.position);
    }
    private void CauldronCounter_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.recipeFail, Camera.main.transform.position);
    }
    public void PlaySound(SoundType soundType, Vector3 position, float volume = 1f)
    {
        //SoundManager.Instance.PlaySound(SoundType.IngredientAdd, transform.position);
        AudioClip[] clips = soundType switch
        {
            SoundType.IngredientAdd => audioClipRefsSO.addIngredient,
            SoundType.RecipeSuccess => audioClipRefsSO.recipeSuccess,
            SoundType.RecipeFail => audioClipRefsSO.recipeFail,
            SoundType.Chop => audioClipRefsSO.chopping,
            SoundType.ButtonClick => audioClipRefsSO.buttonClick,
            SoundType.ObjectDestroy => audioClipRefsSO.objectDestroy,
            SoundType.SellingIngredient => audioClipRefsSO.sellingIngredient,
            SoundType.SellingPotionCheap => audioClipRefsSO.sellingPotionCheap,
            SoundType.SellingPotionExpensive => audioClipRefsSO.sellingPotionExpensive,
            _ => null
        };

        if (clips != null && clips.Length > 0) 
        { 
            PlaySound(clips[Random.Range(0, clips.Length)], position, volume);
        }   
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
    PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }   
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    public void PlaySoundWithDelay(AudioClip[] audioClipArray, Vector3 position, float delay, float volume = 1f)
    {
        StartCoroutine(PlaySoundDelayedCoroutine(audioClipArray, position, delay, volume));
    }

    private IEnumerator PlaySoundDelayedCoroutine(AudioClip[] audioClipArray, Vector3 position, float delay, float volume)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(audioClipArray, position, volume);
    }
}
