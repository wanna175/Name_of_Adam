using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Mercy : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Mercy;

        _name = "자비";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Benediction_Sprite");

        _description = "공격력이 0이 되는 대신 피격 대상의 신앙을 1 떨어뜨립니다";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.DAMAGE_CONFIRM;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.ChangedDamage = 0;
        if (caster != null)
            caster.ChangeFall(1);

        return false;
    }
}
