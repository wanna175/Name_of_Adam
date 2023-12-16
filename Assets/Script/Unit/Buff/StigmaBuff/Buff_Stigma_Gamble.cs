using UnityEngine;

public class Buff_Stigma_Gamble : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Gamble;

        _name = "도박";

        _description = "도박.";

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
        if (6 >= Random.Range(0, 10))
        {
            _owner.ChangedDamage += _owner.BattleUnitTotalStat.ATK;
        }
        else
        {
            _owner.ChangedDamage -= _owner.BattleUnitTotalStat.ATK / 2;
        }

        return false;
    }
}