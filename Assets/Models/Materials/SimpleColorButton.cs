using UnityEngine;
using UnityEngine.UI;

public class SimpleColorButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color buttonColor = Color.green;

    private Texture2D originalTexture;
    private Texture2D coloredTexture;

    void Start()
    {
        ApplyColorToTexture();
    }

    public void SetButtonColor(Color newColor)
    {
        buttonColor = newColor;
        ApplyColorToTexture();
    }

    private void ApplyColorToTexture()
    {
        // Берем оригинальную текстуру из спрайта
        originalTexture = buttonImage.sprite.texture;

        // Создаем временную текстуру
        coloredTexture = new Texture2D(originalTexture.width, originalTexture.height);

        // Копируем пиксели с применением цвета
        Color[] pixels = originalTexture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            // Режим Color: сохраняем яркость, применяем цвет
            float luminance = pixels[i].r * 0.299f + pixels[i].g * 0.587f + pixels[i].b * 0.114f;
            pixels[i] = new Color(
                luminance * buttonColor.r,
                luminance * buttonColor.g,
                luminance * buttonColor.b,
                pixels[i].a
            );
        }

        coloredTexture.SetPixels(pixels);
        coloredTexture.Apply();

        // Создаем новый спрайт с окрашенной текстурой
        Sprite newSprite = Sprite.Create(coloredTexture,
            buttonImage.sprite.rect,
            new Vector2(0.5f, 0.5f),
            buttonImage.sprite.pixelsPerUnit);

        buttonImage.sprite = newSprite;
    }
}