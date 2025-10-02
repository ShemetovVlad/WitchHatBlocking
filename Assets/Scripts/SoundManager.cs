using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private CauldronCounter cauldronCounter;
    [SerializeField] private CuttingCounter cuttingCounter;

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    private void Start()
    {
        cauldronCounter.OnIngredientAdded += CauldronCounter_OnIngredientAdded;
        cauldronCounter.OnRecipeSuccess += CauldronCounter_OnRecipeSuccess;
        cauldronCounter.OnRecipeFailed += CauldronCounter_OnRecipeFailed;
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }
    private void CauldronCounter_OnIngredientAdded(object sender, KitchenObjectSO ingredientSO)
    {
        //PlaySound(ingredientSO.pickUpSound, cauldronCounter.transform.position);
    }
    private void CauldronCounter_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.recipeSuccess, cauldronCounter.transform.position);
    }
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.chopping, cuttingCounter.transform.position);
    }
    private void CauldronCounter_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.recipeFail, Camera.main.transform.position);
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
