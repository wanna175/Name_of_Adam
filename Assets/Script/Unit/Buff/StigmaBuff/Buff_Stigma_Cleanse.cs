using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Cleanse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Cleanse;

        _name = "결신";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "해당 유닛이 버프가 두개 이상일때 공격시 피격 대상의 신앙을 1 떨어뜨립니다";

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
            caster.ChangeFall(1, FallAnimMode.On, 0.75f);

        return false;
    }
}
