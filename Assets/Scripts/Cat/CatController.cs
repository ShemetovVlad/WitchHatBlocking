using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class CatController : MonoBehaviour
{
    [SerializeField] private float blinkInterval = 3f;
    [SerializeField] private float tailWagInterval = 5f;
    [SerializeField] private float cleaningInterval = 10f;
    [SerializeField] private HeadLookAt headLookAt;

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

        if (!isCleaning)
        {
            cleaningTimer -= Time.deltaTime;
            if (cleaningTimer <= 0)
            {
                cleaningTimer = cleaningInterval;
                StartCleaning();
            }
        }
    }
    private void StartCleaning()
    {
        StartCoroutine(StartCleaningRoutine());
    }

    private IEnumerator StartCleaningRoutine()
    {
        yield return StartCoroutine(headLookAt.DisableLookAt());
        isCleaning = true;
        animator.SetTrigger("Cleaning");

        Invoke("EndCleaning", 5f);
    }
    public void EndCleaning()
    {
        isCleaning = false;
        headLookAt.EnableLookAt();
        cleaningTimer = cleaningInterval; 
    }
 }