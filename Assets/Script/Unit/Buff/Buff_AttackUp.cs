using UnityEngine;

public class Buff_AttackUp : Buff
{
    public override void Init()
    {
        _name = "Attack Up";

        _description = "공격력이 10 증가합니다.";

        _count = 3;

        _countDownTiming = ActiveTiming.TURN_END;

        _buffActiveTiming = ActiveTiming.NONE;

        _passiveBuff = true;

        _dispellable = true;
    }

    public override void Active(BattleUnit caster, BattleUnit receiver)
    {
    }

    public override void Stack()
    {
        _count = 3;
    }

    public override Stat GetBuffedStat()
    {
        Stat attackUp = new();
        attackUp.ATK = 10;

        return attackUp;
    }
}