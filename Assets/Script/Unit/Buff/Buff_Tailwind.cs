using UnityEngine;

public class Buff_Tailwind : Buff
{
    private int speedUp;
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Raise;

        _name = "순풍";

        _description = "속도가 30% 증가합니다.";

        _count = 1;

        _countDownTiming = ActiveTiming.AFTER_ATTACK;

        _buffActiveTiming = ActiveTiming.NONE;

        _caster = caster;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;

        speedUp = owner.DeckUnit.DeckUnitTotalStat.SPD / 2;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();  
        stat.SPD += speedUp;

        return stat;
    }
}