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
    MortarKnock,
    BerryHarvest,
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
    [SerializeField] private CuttingCounter counterKnife;
    [SerializeField] private CuttingCounter counterMortar;
    [SerializeField] private StoveCounter counterLab;
    [SerializeField] private PlayerController player;
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;
    private Coroutine boilingSoundCoroutine;


    private void Start()
    {
        cauldronCounter.OnIngredientAdded += CauldronCounter_OnIngredientAdded;
        cauldronCounter.OnRecipeSuccess += CauldronCounter_OnRecipeSuccess;
        cauldronCounter.OnRecipeFailed += CauldronCounter_OnRecipeFailed;
        counterKnife.OnCut += CounterKnife_OnCut;
        counterMortar.OnCut += CounterMortar_OnCut;
        player.OnDestroyObjectAction += Player_OnDestroyObjectAction;
        counterLab.OnStateChanged += CounterLab_OnStateChanged;
    }
    private void CounterLab_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        StopBoilingSound(); // Всегда останавливаем предыдущий звук

        if (e.state == StoveCounter.State.Boiling)
        {
            StartBoilingSound(counterLab.transform.position);
        }
    }
    private void StartBoilingSound(Vector3 position)
    {
        if (boilingSoundCoroutine == null)
        {
            boilingSoundCoroutine = StartCoroutine(PlayBoilingLoop(position));
        }
    }

    private void StopBoilingSound()
    {
        if (boilingSoundCoroutine != null)
        {
            StopCoroutine(boilingSoundCoroutine);
            boilingSoundCoroutine = null;
        }
    }

    private IEnumerator PlayBoilingLoop(Vector3 position)
    {
        while (true)
        {
            PlaySound(SoundType.RecipeFail, position);
            yield return new WaitForSeconds(1f); // интервал между звуками
        }
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
    private void CounterKnife_OnCut(object sender, System.EventArgs e)
    {
        PlaySound(SoundType.Chop, counterKnife.transform.position);
    }
    private void CounterMortar_OnCut(object sender, System.EventArgs e)
    {
        PlaySound(SoundType.MortarKnock, counterMortar.transform.position);
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
    private void OnDestroy()
    {
        if (counterLab != null)
            counterLab.OnStateChanged -= CounterLab_OnStateChanged;
    }
}
