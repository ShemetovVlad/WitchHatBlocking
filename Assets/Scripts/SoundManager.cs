using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private CauldronCounter cauldronCounter;

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private void Start()
    {
        cauldronCounter.OnIngredientAdded += CauldronCounter_OnIngredientAdded;
        cauldronCounter.OnRecipeSuccess += CauldronCounter_OnRecipeSuccess;
        cauldronCounter.OnRecipeFailed += CauldronCounter_OnRecipeFailed;
    }
    private void CauldronCounter_OnIngredientAdded(object sender, KitchenObjectSO ingredientSO)
    {
        //PlaySound(ingredientSO.pickUpSound, cauldronCounter.transform.position);
    }
    private void CauldronCounter_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.recipeSuccess, cauldronCounter.transform.position);
    }

    private void CauldronCounter_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.recipeFail, cauldronCounter.transform.position);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
