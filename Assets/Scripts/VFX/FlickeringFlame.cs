using UnityEngine;

public class FlickeringFlame: MonoBehaviour
{
    [Header("Light Flickering")]
    [SerializeField] private Light fireLight;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float flickerSpeed = 3f;

    [Header("Position Wobble")]
    [SerializeField] private bool enableWobble = true;
    [SerializeField] private float wobbleRadius = 0.1f;
    [SerializeField] private float wobbleSpeed = 2f;

    [Header("Color Variation")]
    [SerializeField] private bool enableColorChange = true;
    [SerializeField] private Color minColor = new Color(1f, 0.4f, 0.1f);
    [SerializeField] private Color maxColor = new Color(1f, 0.9f, 0.3f);

    private float baseIntensity;
    private Vector3 basePosition;
    private Vector3 randomOffsets;

    void Start()
    {
        if (fireLight == null)
            fireLight = GetComponent<Light>();

        baseIntensity = fireLight.intensity;
        basePosition = fireLight.transform.position;
        //basePosition = transform.localPosition;

        // Случайные смещения для разных осей шума
        randomOffsets = new Vector3(
            Random.Range(0f, 100f),
            Random.Range(0f, 100f),
            Random.Range(0f, 100f)
        );
    }

    void Update()
    {
        // Мерцание интенсивности
        float intensityNoise = Mathf.PerlinNoise(Time.time * flickerSpeed + randomOffsets.x, 0f);
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensityNoise) * baseIntensity;

        if (enableColorChange)
        {
            float colorNoise = Mathf.PerlinNoise(Time.time * flickerSpeed * 0.7f + randomOffsets.y, 0f);
            fireLight.color = Color.Lerp(minColor, maxColor, colorNoise);
        }

        // Дрожание позиции
        if (enableWobble)
        {
            WobblePosition();
        }
    }

    private void WobblePosition()
    {
        // Разные частоты для каждой оси для более натурального движения
        float xNoise = Mathf.PerlinNoise(Time.time * wobbleSpeed + randomOffsets.x, randomOffsets.y) * 2f - 1f;
        float yNoise = Mathf.PerlinNoise(Time.time * wobbleSpeed + randomOffsets.z, randomOffsets.x) * 2f - 1f;
        float zNoise = Mathf.PerlinNoise(Time.time * wobbleSpeed + randomOffsets.y, randomOffsets.z) * 2f - 1f;

        Vector3 wobble = new Vector3(xNoise, yNoise, zNoise) * wobbleRadius;
        fireLight.transform.localPosition = basePosition + wobble;
    }

    // Метод для сброса позиции (на случай если нужно двигать костер)
    public void ResetBasePosition()
    {
        basePosition = transform.localPosition;
    }

    // Метод для настройки параметров из других скриптов
    public void SetWobbleParameters(float radius, float speed)
    {
        wobbleRadius = radius;
        wobbleSpeed = speed;
    }
}