using UnityEngine;

public class CandleFlame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;

    [Header("Tilt Settings")]
    [SerializeField] private float maxTiltAngle = 80f;
    [SerializeField] private float tiltSensitivity = 50f;
    [SerializeField] private float maxSpeed = 1f;

    [Header("Stretch Settings")]
    [SerializeField][Range(0f, 2f)] private float maxStretch = 0.5f;
    [SerializeField] private float stretchSensitivity = 50f;

    [Header("Return Settings")]
    [SerializeField] private float returnDelay = 0.2f;
    [SerializeField] private float returnSpeed = 2f;

    private Vector3 initialScale;
    private Quaternion initialRotation;
    private Vector3 lastPlayerPosition;
    private Vector3 currentVelocity;
    private bool isMoving;
    private float idleTimer;
    private void Start()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        initialScale = transform.localScale;
        initialRotation = transform.localRotation;
        lastPlayerPosition = playerController.transform.position;
    }
    private void Update()
    {
        CalculateVelocity();
        UpdateFlame();
    }
    private void CalculateVelocity()
    {
        Vector3 currentPosition = playerController.transform.position;
        Vector3 rawVelocity = (currentPosition - lastPlayerPosition) / Time.unscaledDeltaTime;

        currentVelocity = Vector3.Lerp(currentVelocity, rawVelocity, Time.unscaledDeltaTime * 10f);
        lastPlayerPosition = currentPosition;

        isMoving = currentVelocity.magnitude > 0.1f;
    }
    private void UpdateFlame()
    {
        if (isMoving)
        {
            idleTimer = 0f;
            ApplyTiltAndStretch();
        }
        else
        {
            idleTimer += Time.unscaledDeltaTime;
            if (idleTimer >= returnDelay)
            {
                ReturnToInitialState();
            }
        }
    }
    private void ApplyTiltAndStretch()
    {
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        float speedFactor = Mathf.Clamp01(horizontalVelocity.magnitude / maxSpeed);

        if (horizontalVelocity.magnitude > 0.1f)
        {
            // Ось вращения - перпендикуляр между направлением движения и вертикалью
            Vector3 tiltAxis = Vector3.Cross(horizontalVelocity.normalized, Vector3.up);

            // Угол наклона
            float tiltAngle = speedFactor * tiltSensitivity;
            tiltAngle = Mathf.Clamp(tiltAngle, 0, maxTiltAngle);

            // Вращение от начальной позиции
            Quaternion tiltRotation = Quaternion.AngleAxis(tiltAngle, tiltAxis);
            transform.rotation = initialRotation * tiltRotation;
        }

        // Stretching
        float stretchAmount = 1 + speedFactor * maxStretch;
        Vector3 targetScale = new Vector3(
            initialScale.x,
            initialScale.y * stretchAmount,
            initialScale.z
        );

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            stretchSensitivity * Time.unscaledDeltaTime
        );
    }
    private void ReturnToInitialState()
    {
        // Stretching return
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            initialScale,
            returnSpeed * Time.unscaledDeltaTime
        );
        // Возвращаем наклон, но сохраняем текущий поворот вокруг Y
        Vector3 currentEuler = transform.rotation.eulerAngles;
        Vector3 targetEuler = initialRotation.eulerAngles;

        // Сохраняем текущий Y, берем начальные X и Z
        Quaternion targetRotation = Quaternion.Euler(
            targetEuler.x,    // начальный X
            currentEuler.y,   // текущий Y (не меняем)
            targetEuler.z     // начальный Z
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            returnSpeed * Time.unscaledDeltaTime
        );
    }
}