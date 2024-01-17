using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nimrod_Animation : MonoBehaviour
{
    [SerializeField] public RuntimeAnimatorController AnimatorController;
    [SerializeField] public RuntimeAnimatorController CorruptionAnimatorController;

    [SerializeField] private Animator _nimrodAnimator;

    public void SetBool(string varName, bool boolean)
    {
        _nimrodAnimator.SetBool(varName, boolean);
    }

    public void ChangeAnimator(Team team)
    {
        if (team == Team.Player)
        {
            _nimrodAnimator.runtimeAnimatorController = CorruptionAnimatorController;
        }
        else
        {
            _nimrodAnimator.runtimeAnimatorController = AnimatorController;
        }
    }
}
