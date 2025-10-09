using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

public class HeadLookAt : MonoBehaviour
{
    private LookAtConstraint lookAtConstraint;
    private Coroutine weightCoroutine;

    private void Start()
    {
        lookAtConstraint = GetComponent<LookAtConstraint>();
    }

    public void EnableLookAt()
    {
        if (weightCoroutine != null)
            StopCoroutine(weightCoroutine);

        lookAtConstraint.weight = 0f;
        lookAtConstraint.enabled = true;
        weightCoroutine = StartCoroutine(ChangeWeight(1f));
    }

    public IEnumerator DisableLookAt()
    {
        if (weightCoroutine != null)
            StopCoroutine(weightCoroutine);

        yield return StartCoroutine(ChangeWeight(0f));
    }

    private IEnumerator ChangeWeight(float targetWeight)
    {
        float startWeight = lookAtConstraint.weight;
        float time = 0f;

        while (time < 1f)
        {
            lookAtConstraint.weight = Mathf.Lerp(startWeight, targetWeight, time);
            time += Time.deltaTime;
            yield return null;
        }

        lookAtConstraint.weight = targetWeight;

        if (targetWeight == 0f)
        {
            lookAtConstraint.enabled = false;
        }
    }
}
