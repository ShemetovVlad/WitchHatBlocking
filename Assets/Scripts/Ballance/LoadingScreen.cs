using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("Настройки загрузки")]
    [SerializeField] private float showDuration = 3f;    // Общее время показа
    [SerializeField] private float fadeDuration = 1f;    // Длительность исчезновения

    [Header("Ссылки")]
    [SerializeField] private Image blackImage;           // Черный фон
    [SerializeField] private GameObject loadingText;     // Текст "Loading..."
    [SerializeField] private Slider progressBar;         // Прогресс-бар
    [SerializeField] private Text percentText;           // Текст процентов (опционально)

    [Header("Настройки прогресса")]
    private float[] progressStages = { 0.1f, 0.6f, 0.8f, 0.95f, 1f }; 
    private float[] stageDurations = { 0.5f, 1f, 0.8f, 0.7f };          

    private void Start()
    {
        // Сбрасываем прогресс-бар в 0
        if (progressBar != null)
            progressBar.value = 0f;

        if (percentText != null)
            percentText.text = "0%";

        StartCoroutine(AnimateLoading());
    }

    private IEnumerator AnimateLoading()
    {
        // Поэтапная загрузка
        for (int i = 0; i < progressStages.Length - 1; i++)
        {
            float startProgress = progressStages[i];
            float targetProgress = progressStages[i + 1];
            float stageDuration = stageDurations[i];

            // Мгновенно прыгаем к началу этапа
            UpdateProgress(startProgress);

            // Плавно заполняем до следующего этапа
            float stageTimer = 0f;
            while (stageTimer < stageDuration)
            {
                stageTimer += Time.deltaTime;
                float progress = Mathf.Lerp(startProgress, targetProgress, stageTimer / stageDuration);
                UpdateProgress(progress);
                yield return null;
            }

            // Случайная пауза между этапами
            if (i < progressStages.Length - 2)
            {
                float pauseDuration = Random.Range(0.1f, 0.3f);
                yield return new WaitForSeconds(pauseDuration);
            }
        }

        // Гарантируем 100% в конце
        UpdateProgress(1f);

        // Ждем немного на 100%
        yield return new WaitForSeconds(0.5f);

        // Убираем текст и прогресс-бар
        if (loadingText != null)
            loadingText.SetActive(false);
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);
        if (percentText != null)
            percentText.gameObject.SetActive(false);

        // Плавно убираем черный фон
        float fadeTimer = 0f;
        Color originalColor = blackImage.color;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

            Color newColor = originalColor;
            newColor.a = currentAlpha;
            blackImage.color = newColor;

            yield return null;
        }

        // Выключаем весь объект
        gameObject.SetActive(false);
    }

    private void UpdateProgress(float progress)
    {
        if (progressBar != null)
            progressBar.value = progress;

        if (percentText != null)
            percentText.text = Mathf.RoundToInt(progress * 100) + "%";
    }
}