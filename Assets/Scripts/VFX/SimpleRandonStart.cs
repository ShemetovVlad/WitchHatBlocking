using UnityEngine;

public class SimpleRandomStart : MonoBehaviour
{
    [Header("Случайное начало анимации")]
    [SerializeField] private bool randomizeOnStart = true;
    [SerializeField] private float maxStartTime = 10f; // Максимальное время смещения

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (randomizeOnStart && animator != null)
        {
            // Просто устанавливаем случайное нормализованное время
            float randomTime = Random.Range(0f, maxStartTime);
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(state.fullPathHash, 0, randomTime);

            Debug.Log($"Book {gameObject.name} starting at time: {randomTime}");
        }
    }
}