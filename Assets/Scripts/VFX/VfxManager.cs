using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class EffectManager : MonoBehaviour
{
    [Header("Ссылки на игровые объекты")]
    [SerializeField] private PlayerController player;
    [SerializeField] private CauldronCounter cauldron;

    [Header("Партикл эффекты")]
    [SerializeField] private ParticleSystem[] recipeSuccess;
    [SerializeField] private ParticleSystem[] recipeFailed;

    void Start()
    {
        if (cauldron != null)
        {
            cauldron.OnRecipeSuccess += RecipeSuccessEffect;
            cauldron.OnRecipeFailed += RecipeFailedEffect;
        }
    }

    void OnDestroy()
    {
        if (cauldron != null)
        {
            cauldron.OnRecipeSuccess -= RecipeSuccessEffect;
            cauldron.OnRecipeFailed -= RecipeFailedEffect;
        }
    }

    void RecipeSuccessEffect(object sender, PotionRecipeSO recipe)
    {
        if (recipeSuccess != null && recipeSuccess.Length > 0)
        {
            foreach (var effect in recipeSuccess)
            {
                if (effect != null)
                    effect.Play();
            }
        }
    }
    void RecipeFailedEffect(object sender, System.EventArgs e)
    {
        if (recipeFailed != null && recipeFailed.Length > 0)
        {
            foreach (var effect in recipeFailed)
            {
                if (effect != null)
                    effect.Play();
            }
        }
    }
}