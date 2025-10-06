using UnityEngine;
using System.Collections;

public class BlinkController : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private string _blinkShapeKey = "Blink";
    [SerializeField, Range(2f, 10f)] private float _minBlinkInterval = 3f;
    [SerializeField, Range(0.05f, 1.0f)] private float _blinkDuration = 0.5f;

    private SkinnedMeshRenderer _faceMesh;
    private int _blinkShapeIndex;
    private float _nextBlinkTime;
    private bool _isBlinking;

    void Start()
    {
        // Находим меш с морганием (если их несколько, можно задать явно через [SerializeField])
        foreach (var smr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            _blinkShapeIndex = smr.sharedMesh.GetBlendShapeIndex(_blinkShapeKey);
            if (_blinkShapeIndex != -1)
            {
                _faceMesh = smr;
                break;
            }
        }

        if (_faceMesh == null)
        {
            Debug.LogError($"Blend shape '{_blinkShapeKey}' not found in children!");
            enabled = false;
            return;
        }

        _nextBlinkTime = Time.time + Random.Range(_minBlinkInterval, _minBlinkInterval * 1.5f);
    }

    void Update()
    {
        if (Time.time >= _nextBlinkTime && !_isBlinking)
        {
            StartCoroutine(BlinkRoutine());
            _nextBlinkTime = Time.time + Random.Range(_minBlinkInterval, _minBlinkInterval * 1.5f);
        }
    }

    private IEnumerator BlinkRoutine()
    {
        _isBlinking = true;

        // Быстро закрываем глаза (0 → 100)
        for (float t = 0; t < 1f; t += Time.deltaTime / (_blinkDuration))
        {
            _faceMesh.SetBlendShapeWeight(_blinkShapeIndex, Mathf.Lerp(0, 100, t));
            yield return null;
        }

        // Быстро открываем глаза (100 → 0)
        for (float t = 0; t < 1f; t += Time.deltaTime / (_blinkDuration))
        {
            _faceMesh.SetBlendShapeWeight(_blinkShapeIndex, Mathf.Lerp(100, 0, t));
            yield return null;
        }

        // Гарантированный сброс
        _faceMesh.SetBlendShapeWeight(_blinkShapeIndex, 0);
        _isBlinking = false;
        
    }

#if UNITY_EDITOR
    [ContextMenu("Test Blink")]
    private void TestBlink()
    {
        if (Application.isPlaying)
            StartCoroutine(BlinkRoutine());
    }
#endif
}