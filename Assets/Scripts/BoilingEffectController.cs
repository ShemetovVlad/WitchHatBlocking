using UnityEngine;

public class BoilingEffectController : MonoBehaviour
{
    [SerializeField] private CauldronCounter cauldronCounter;
    [SerializeField] private ParticleSystem boilingParticles;

    private void Start()
    {
        if (boilingParticles == null)
            boilingParticles = GetComponent<ParticleSystem>();

        cauldronCounter.OnIngredientAdded += OnIngredientAdded;
        cauldronCounter.OnCauldronCleared += OnCauldronCleared;

        // ON boiling at start
        if (boilingParticles != null && !boilingParticles.isPlaying)
        {
            boilingParticles.Play();
        }
    }

    private void OnIngredientAdded(object sender, KitchenObjectSO ingredientSO)
    {
        if (boilingParticles != null && boilingParticles.isPlaying)
        {
            boilingParticles.Stop();
        }
    }

    private void OnCauldronCleared(object sender, System.EventArgs e)
    {
        if (boilingParticles != null && !boilingParticles.isPlaying)
        {
            boilingParticles.Play();
        }
    }
}