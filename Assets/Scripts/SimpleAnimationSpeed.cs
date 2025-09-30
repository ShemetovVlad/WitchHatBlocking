using UnityEngine;

public class SimpleAnimationSpeed : MonoBehaviour
{
    public float speed = 1.0f;

    void Start()
    {
        Animation animation = GetComponent<Animation>();
        if (animation == null) return;

        foreach (AnimationState state in animation)
        {
            state.speed = speed;
            break;
        }
    }
}