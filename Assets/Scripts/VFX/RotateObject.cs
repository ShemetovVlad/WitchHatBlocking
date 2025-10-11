using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }

    [Header("Настройки вращения")]
    [SerializeField] private RotationAxis axis = RotationAxis.Y;
    [SerializeField] private float rotationTime = 5f; // время полного оборота в секундах
    [SerializeField] private bool rotateOnStart = true;
    [SerializeField] private Space coordinateSpace = Space.Self;

    [Header("Текущее состояние")]
    [SerializeField] private bool isRotating = false;

    private Vector3 startRotation;
    private float currentAngle = 0f;

    private void Start()
    {
        // Сохраняем начальное вращение
        startRotation = coordinateSpace == Space.Self ?
            transform.localEulerAngles : transform.eulerAngles;

        isRotating = rotateOnStart;
    }

    private void Update()
    {
        if (isRotating && rotationTime > 0)
        {
            // Вычисляем угол на основе времени
            float angleDelta = (360f / rotationTime) * Time.deltaTime;
            currentAngle += angleDelta;

            // Нормализуем угол чтобы избежать переполнения
            if (currentAngle >= 360f) currentAngle -= 360f;

            // Применяем вращение по выбранной оси
            ApplyRotation(currentAngle);
        }
    }

    private void ApplyRotation(float angle)
    {
        Vector3 newRotation = startRotation;

        switch (axis)
        {
            case RotationAxis.X:
                newRotation.x = startRotation.x + angle;
                break;
            case RotationAxis.Y:
                newRotation.y = startRotation.y + angle;
                break;
            case RotationAxis.Z:
                newRotation.z = startRotation.z + angle;
                break;
        }

        // Применяем вращение в выбранной системе координат
        if (coordinateSpace == Space.Self)
            transform.localEulerAngles = newRotation;
        else
            transform.eulerAngles = newRotation;
    }

    // === Публичные методы для управления ===

    /// <summary>
    /// Начать вращение
    /// </summary>
    public void StartRotation()
    {
        isRotating = true;
    }

    /// <summary>
    /// Остановить вращение
    /// </summary>
    public void StopRotation()
    {
        isRotating = false;
    }

    /// <summary>
    /// Переключить вращение (вкл/выкл)
    /// </summary>
    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    /// <summary>
    /// Установить время полного оборота
    /// </summary>
    /// <param name="time">Время в секундах</param>
    public void SetRotationTime(float time)
    {
        rotationTime = Mathf.Max(0.1f, time);
    }

    /// <summary>
    /// Установить ось вращения
    /// </summary>
    /// <param name="newAxis">Новая ось вращения</param>
    public void SetRotationAxis(RotationAxis newAxis)
    {
        // Сохраняем текущий прогресс перед сменой оси
        Vector3 currentRotation = coordinateSpace == Space.Self ?
            transform.localEulerAngles : transform.eulerAngles;
        startRotation = currentRotation;
        currentAngle = 0f;

        axis = newAxis;
    }

    /// <summary>
    /// Установить систему координат для вращения
    /// </summary>
    /// <param name="space">Local или World</param>
    public void SetCoordinateSpace(Space space)
    {
        coordinateSpace = space;
        // Обновляем стартовое вращение при смене системы координат
        startRotation = coordinateSpace == Space.Self ?
            transform.localEulerAngles : transform.eulerAngles;
    }

    /// <summary>
    /// Сбросить вращение к начальному состоянию
    /// </summary>
    public void ResetRotation()
    {
        currentAngle = 0f;
        if (coordinateSpace == Space.Self)
            transform.localEulerAngles = startRotation;
        else
            transform.eulerAngles = startRotation;
    }

    /// <summary>
    /// Получить текущее состояние вращения
    /// </summary>
    /// <returns>true если вращение активно</returns>
    public bool IsRotating()
    {
        return isRotating;
    }
    
    /// <summary>
    /// Получить текущее время полного оборота
    /// </summary>
    /// <returns>Время в секундах</returns>
    public float GetRotationTime()
    {
        return rotationTime;
    }
}