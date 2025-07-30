using UnityEngine;
using UnityEngine.Events;

public class SkillTreeManager : MonoBehaviour
{
    public static UnityEvent<float> OnSpeedPerkActivated = new UnityEvent<float>();
}
