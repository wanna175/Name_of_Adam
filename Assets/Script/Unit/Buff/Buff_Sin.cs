using UnityEngine;

public class Buff_Sin : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sin;

        _name = "대죄";

        _description = "공격 시 타락을 1 부여합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.ChangeFall(1);

        return false;
    }
}