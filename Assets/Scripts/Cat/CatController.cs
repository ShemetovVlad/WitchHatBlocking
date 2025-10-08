using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class CatController : MonoBehaviour
{
    [SerializeField] private float blinkInterval = 3f;
    [SerializeField] private float tailWagInterval = 5f;
    [SerializeField] private float cleaningInterval = 10f;
    [SerializeField] private Behaviour headAimConstraint; // Прокастываем Aim Constraint

    private Animator animator;
    private float blinkTimer;
    private float tailWagTimer;
    private float cleaningTimer;
    private bool isCleaning;

    private void Start()
    {
        animator = GetComponent<Animator>();
        blinkTimer = blinkInterval;
        tailWagTimer = tailWagInterval;
        cleaningTimer = cleaningInterval;
    }

    private void Update()
    {
        // Таймер для моргания
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0)
        {
            animator.SetTrigger("Blink");
            blinkTimer = blinkInterval;
        }

        // Таймер для хвоста
        tailWagTimer -= Time.deltaTime;
        if (tailWagTimer <= 0)
        {
            animator.SetTrigger("TailWag");
            tailWagTimer = tailWagInterval;
        }

        cleaningTimer -= Time.deltaTime;
        if (cleaningTimer <= 0 && !isCleaning)
        {
            StartCoroutine(DisableAimConstraintSmoothly());
        }
    }
    private void StartCleaning()
    {
        isCleaning = true;
        animator.SetTrigger("Cleaning");

        float cleaningDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("EndCleaning", 8f);
    }
    public void EndCleaning()
    {
        isCleaning = false;
        StartCoroutine(EnableAimConstraintSmoothly());
        cleaningTimer = cleaningInterval; 
    }
    private IEnumerator DisableAimConstraintSmoothly()
    {
        AimConstraint aimConstraint = headAimConstraint as AimConstraint;
        float startWeight = aimConstraint.weight;
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            aimConstraint.weight = Mathf.Lerp(startWeight, 0f, timer / duration);
            yield return null;
        }

        aimConstraint.enabled = false;
        StartCleaning();
    }

    private IEnumerator EnableAimConstraintSmoothly()
    {
        headAimConstraint.enabled = true;

        AimConstraint aimConstraint = headAimConstraint as AimConstraint;
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            aimConstraint.weight = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }
    }
}