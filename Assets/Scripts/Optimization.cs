// Добавь этот скрипт для диагностики
using UnityEngine;
using System.Collections.Generic;

public class BatchAnalyzer : MonoBehaviour
{
    void Start()
    {
        var materials = new Dictionary<Material, int>();
        var renderers = FindObjectsOfType<Renderer>();

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.sharedMaterials)
            {
                if (material != null)
                {
                    if (materials.ContainsKey(material))
                        materials[material]++;
                    else
                        materials[material] = 1;
                }
            }
        }

        Debug.Log($"Уникальных материалов: {materials.Count}");
        Debug.Log($"Всего рендереров: {renderers.Length}");

        // Сортируем по частоте использования
        var sorted = new List<KeyValuePair<Material, int>>(materials);
        sorted.Sort((a, b) => b.Value.CompareTo(a.Value));

        foreach (var pair in sorted)
        {
            Debug.Log($"Material: {pair.Key.name}, Used by: {pair.Value} objects");
        }
    }
}