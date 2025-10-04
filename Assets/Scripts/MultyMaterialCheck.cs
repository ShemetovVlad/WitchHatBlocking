// Добавь этот скрипт для диагностики Props
using UnityEngine;
using System.Collections.Generic;

public class PropsAnalyzer : MonoBehaviour
{
    void Start()
    {
        var props = FindObjectsOfType<Transform>();
        int multiMaterialObjects = 0;

        foreach (var prop in props)
        {
            if (prop.name.Contains("Prop") || prop.name.Contains("Prefab"))
            {
                Renderer renderer = prop.GetComponent<Renderer>();
                if (renderer != null && renderer.sharedMaterials.Length > 1)
                {
                    multiMaterialObjects++;
                    Debug.Log($"Объект {prop.name} имеет {renderer.sharedMaterials.Length} материалов: " +
                             string.Join(", ", System.Array.ConvertAll(renderer.sharedMaterials, m => m.name)));
                }
            }
        }

        Debug.Log($"Объектов с Multiple Materials: {multiMaterialObjects}");
    }
}