using UnityEngine;
using System.Collections;

public class FlameTilt : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxTiltAngle = 80f;
    [SerializeField] private float tiltFactor = 30f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float returnDelay = 0.2f;
    [SerializeField] private float returnSpeed = 2f;

    [Header("Stretch Settings")]
    [SerializeField][Range(0f, 2f)] private float maxStretch = 1f;
    [SerializeField] private float stretchResponse = 1.5f;

    [Header("Billboard Settings")]
    [SerializeField] private bool useBillboard = true;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 billboardRotationOffset = Vector3.zero;

    private Transform candle;
    private Vector3 lastPosition;
    private Vector3 currentVelocity;
    private Quaternion baseRotation; // Базовое вращение в мировых координатах
    private Vector3 initialScale;
    private bool isReturning;
    private float currentTilt;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Awake()
    {
        candle = transform.parent;
        if (candle == null)
        {
            Debug.LogError("Flame must be a child of Candle!");
            return;
        }

        // Сохраняем начальное глобальное вращение как базовое
        baseRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        initialScale = transform.localScale;
        lastPosition = candle.position;
    }

    private void Update()
    {
        CalculateMovement();
        MaintainVerticalOrientation();
        ApplyTiltAndStretch();
    }

    private void CalculateMovement()
    {
        Vector3 currentPosition = candle.position;
        Vector3 rawVelocity = (currentPosition - lastPosition) / Time.deltaTime;
        currentVelocity = Vector3.Lerp(currentVelocity, rawVelocity, Time.deltaTime * 10f);
        lastPosition = currentPosition;
    }

    private void MaintainVerticalOrientation()
    {
        if (useBillboard && mainCamera != null)
        {
            // Билбординг с учетом вертикали и смещений
            Vector3 lookDirection = mainCamera.transform.position - transform.position;
            lookDirection.y = 0; // Сохраняем вертикальную ориентацию
            transform.rotation = Quaternion.LookRotation(-lookDirection) * Quaternion.Euler(billboardRotationOffset);
        }
        else
        {
            // Старая логика (если нужно отключить билбординг)
            Quaternion verticalRotation = Quaternion.Euler(0, candle.rotation.eulerAngles.y, 0);
            transform.rotation = verticalRotation;
        }
    }

    private void ApplyTiltAndStretch()
    {
        if (currentVelocity.magnitude > 0.1f)
        {
            // Рассчитываем наклон относительно глобальных осей
            Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
            float speedFactor = Mathf.Clamp01(horizontalVelocity.magnitude / maxSpeed);

            // Наклон в противоположную движению сторону
            Vector3 tiltAxis = Vector3.Cross(horizontalVelocity.normalized, Vector3.up);
            float tiltAngle = speedFactor * tiltFactor;
            currentTilt = Mathf.Clamp(tiltAngle, 0, maxTiltAngle);

            // Применяем наклон к базовому вращению
            Quaternion tiltRotation = Quaternion.AngleAxis(currentTilt, tiltAxis);
            transform.rotation = tiltRotation * transform.rotation;

            // Растяжение
            ApplyStretch(speedFactor);

            if (isReturning)
            {
                StopCoroutine(ReturnToInitialState());
                isReturning = false;
            }
        }
        else if (!isReturning)
        {
            StartCoroutine(ReturnToInitialState());
        }
    }

    private void ApplyStretch(float speedFactor)
    {
        float stretchFactor = 1 + speedFactor * maxStretch;
        Vector3 targetScale = new Vector3(
            initialScale.x,
            initialScale.y * stretchFactor,
            initialScale.z
        );

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            stretchResponse * Time.deltaTime
        );
    }

    private IEnumerator ReturnToInitialState()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay);

        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        Vector3 initialScale = this.initialScale;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f ||
               Vector3.Distance(transform.localScale, initialScale) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                returnSpeed * Time.deltaTime
            );

            transform.localScale = Vector3.Lerp(
                transform.localScale,
                initialScale,
                returnSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.rotation = targetRotation;
        transform.localScale = initialScale;
        isReturning = false;
    }
}