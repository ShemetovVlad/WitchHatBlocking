using System;
using System.Collections;
using UnityEngine;

public class EffectPlay : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyEffect;
    [SerializeField] private PlayerController player;
    [SerializeField] private float delay = 0.4f;
    public void PlayDestroyEffect()
    {
        if (destroyEffect != null)
        {
            destroyEffect.Play();
        }
    }
    private void Start()
    {
        player.OnDestroyObjectAction += Player_OnDestroyObjectAction;
    }
    private void Player_OnDestroyObjectAction()
    {
        StartCoroutine(PlayEffectWithDelay(delay));
    }
    private IEnumerator PlayEffectWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayDestroyEffect();
        SoundManager.Instance.PlaySound(SoundType.Poof, player.transform.position);
    }
}