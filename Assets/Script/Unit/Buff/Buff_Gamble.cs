using UnityEngine;

public class Buff_Gamble : Buff
{    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Gamble;

        _name = "도박";

        _description = "도박.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _statBuff = false;

        _dispellable = false;

        _caster = caster;

        _owner = owner;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        if (6 >= Random.Range(0, 10))
        {
            caster.ChangedDamage += caster.BattleUnitTotalStat.ATK;
        }
        else
        {
            caster.ChangedDamage -= caster.BattleUnitTotalStat.ATK / 2;
        }

        return false;
    }
}