using UnityEngine;

public class Sandwatchjump : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool isCooking = e.state == StoveCounter.State.Boiling || e.state == StoveCounter.State.Boiled;

        if (isCooking)
        {
            animator.SetTrigger("Jump");
        }
    }
}