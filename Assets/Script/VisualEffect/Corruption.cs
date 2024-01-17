using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : MonoBehaviour
{
    Animator _animator;
    BattleUnit _unit;
    public Action<Corruption> StigmaSelectEvent;

    public void Init(BattleUnit unit, Action<Corruption> action)
    {
        _animator = GetComponent<Animator>();
        StigmaSelectEvent = action;
        _unit = unit;

        _animator.SetBool("LoopStart", false);
        _animator.SetBool("LoopExit", false);

        gameObject.SetActive(true);
        _animator.Play("Corruption_Start");
    }

    public void LoopStart()
    {
        _animator.SetBool("LoopStart", true);
        if (_unit.Team == Team.Enemy)
        {
            if (TutorialManager.Instance.IsEnable())
                TutorialManager.Instance.ShowNextTutorial();

            StigmaSelectEvent(this);
            if (BattleManager.Instance.IsExistedCorruptionPopup())
                BattleManager.Instance.ShowLastCorruptionPopup();
        }
        else
            LoopExit();
    }

    public void LoopExit()
    {
        _animator.SetBool("LoopExit", true);
        _unit.Corrupted();

        if (BattleManager.Instance.IsExistedCorruptionPopup())
            BattleManager.Instance.ShowLastCorruptionPopup();
    }

    public void CorruptionEnd()
    {
        GameManager.VisualEffect.RestoreEffect(AnimEffects.Corruption, gameObject);
        BattleManager.Instance.BattleOverCheck();
    }

    public BattleUnit GetTargetUnit()
    {
        return _unit;
    }
}