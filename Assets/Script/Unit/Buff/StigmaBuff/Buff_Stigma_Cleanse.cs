using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Cleanse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Cleanse;

        _name = "���";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "�ش� ������ ������ �ΰ� �̻��϶� ���ݽ� �ǰ� ����� �ž��� 1 ����߸��ϴ�";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && _owner.Buff.GetHasBuffNum() >= 2)
            caster.ChangeFall(1);

        return false;
    }
}
