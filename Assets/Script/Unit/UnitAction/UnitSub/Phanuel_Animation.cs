using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phanuel_Animation : MonoBehaviour
{
    [SerializeField] public RuntimeAnimatorController AnimatorController;
    [SerializeField] public RuntimeAnimatorController CorruptionAnimatorController;

    [SerializeField] private Animator _animator;

    public void SetBool(string varName, bool boolean)
    {
        _animator.SetBool(varName, boolean);
    }

    public void ChangeAnimator(Team team)
    {
        transform.position = new(0f, 2.2f, 0f);
        transform.localScale = new(2f, 2f, 1f); // 변경할 크기

        if (team == Team.Player)
        {
            _animator.runtimeAnimatorController = CorruptionAnimatorController;
        }
        else
        {
            _animator.runtimeAnimatorController = AnimatorController;
        }
    }
}
