using UnityEngine;

public class HeadLookAt: MonoBehaviour
{
    [SerializeField] private Transform target;
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
        Y_forward,
    }

    [SerializeField] private Mode mode;
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(target.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - target.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = target.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -target.transform.forward;
                break;
            case Mode.Y_forward:
                Vector3 direction = target.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.up, direction);
                break;
        }

    }
}
