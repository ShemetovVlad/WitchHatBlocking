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

        cleaningTimer -= Time.deltaTime;
        if (cleaningTimer <= 0 && !isCleaning)
        {
            //StartCleaning();
        }
    }
    /*
    private void StartCleaning()
    {
        isCleaning = true;
        headLookAt.ClearTarget();
        animator.SetTrigger("Cleaning");

        float cleaningDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("EndCleaning", 8f);
    }
    public void EndCleaning()
    {
        isCleaning = false;
        headLookAt.SetTarget();
        cleaningTimer = cleaningInterval; 
    }
   */
 }