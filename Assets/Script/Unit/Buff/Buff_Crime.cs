using UnityEngine;

public class Buff_Crime : Buff
{
    public override void Init()
    {
        _buffEnum = BuffEnum.Crime;

        _name = "죄악";

        _description = "공격 시 타락을 1 부여합니다.";

        _count = 2;

        _countDownTiming = ActiveTiming.AFTER_ATTACK;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _passiveBuff = false;

        _dispellable = true;
    }

    public override void Active(BattleUnit caster, BattleUnit receiver)
    {
        receiver.ChangeFall(1);
    }

    public override void Stack()
    {
        _count += 2;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        return stat;
    }
}