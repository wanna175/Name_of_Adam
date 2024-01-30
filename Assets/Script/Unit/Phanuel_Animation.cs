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
