using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Effect");

        StartCoroutine(AnimExit());
    }

    IEnumerator AnimExit()
    {
        while (true)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                break;

            yield return null;
        }

        GameManager.VisualEffect.RestoreEffect(AnimEffects.VisualEffect, gameObject);
    }
}
