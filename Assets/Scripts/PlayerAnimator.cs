using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private ParticleSystem holdObject;
    [SerializeField] private ParticleSystem[] destroyEffect;
    [SerializeField] private float delay = 0.4f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (player == null) 
        {
            Debug.LogError("PlayerController �� �������� � ����������!", this);
            return;
        }
        
        player.OnWalkingStateChanged += HandleWalkingChange;
        player.OnKitchenObjectChanged += HandleKitchenObjectChange;
        player.OnDestroyObjectAction += HandleDestroyHeldObject;

    }
    private void HandleWalkingChange(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }
    
    private void HandleKitchenObjectChange(bool hasObject)
    {
        animator.SetBool("HasObject", hasObject);
        var emission = holdObject.emission;
        emission.enabled = hasObject;

        if (hasObject)
        {
            holdObject.Play();
        }
        else
        {
            holdObject.Stop();
        }
    }

    private void HandleDestroyHeldObject()
    {
        animator.SetTrigger("Destroy");
        StartCoroutine(PlayEffectWithDelay(delay));
    }

    private void OnDestroy()
    {
        // ������������ ��� ����������� ������� (�����!)
        PlayerController player = GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.OnWalkingStateChanged -= HandleWalkingChange;
            player.OnKitchenObjectChanged -= HandleKitchenObjectChange;
            player.OnDestroyObjectAction -= HandleDestroyHeldObject;
        }
    }
    public void PlayDestroyEffect()
    {
        if (destroyEffect != null)
        {
            foreach (ParticleSystem destroyEffect in destroyEffect)
            {
                destroyEffect.Play();
            }

        }
    }
    private IEnumerator PlayEffectWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayDestroyEffect();
        SoundManager.Instance.PlaySound(SoundType.Poof, player.transform.position);
    }
}
