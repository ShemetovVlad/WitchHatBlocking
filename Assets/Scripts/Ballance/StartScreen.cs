using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : MonoBehaviour
{

    [SerializeField] private float fadeOutDuration = 1f;    // Длительность исчезновения
    [SerializeField] private Image blackScreen;           // Черный фон          
    [SerializeField] private GameObject instructionImages; // Изображения с инструкциями
    public void StartGame()
    {
        instructionImages.SetActive(false);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        // Плавно убираем черный фон
        float fadeTimer = 0f;
        Color originalColor = blackScreen.color;

        while (fadeTimer < fadeOutDuration)
        {
            fadeTimer += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeOutDuration);

            Color newColor = originalColor;
            newColor.a = currentAlpha;
            blackScreen.color = newColor;

            yield return null;
        }

        // Выключаем весь объект
        gameObject.SetActive(false);
    }

}