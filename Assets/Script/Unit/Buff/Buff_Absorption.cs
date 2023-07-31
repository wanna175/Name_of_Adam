using UnityEngine;

public class Buff_Absorption : Buff
{
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _name = "흡수";

        _description = "피해량의 30퍼센트를 회복.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _statBuff = false;

        _dispellable = false;

        _caster = caster;

        _owner = owner;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        return false;
    }

    public override void Stack()
    {
    }

    public override Stat GetBuffedStat()
    {
        Stat attackUp = new();

        return attackUp;
    }

    public override void SetValue(int num)
    {

    }
}