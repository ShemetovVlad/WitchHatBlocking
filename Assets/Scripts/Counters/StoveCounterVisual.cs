using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    // On object with animator

    [SerializeField] private GameObject particleGameObject;
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
        animator.SetBool("isCooking", isCooking);
        particleGameObject.SetActive(isCooking);
    }
}
