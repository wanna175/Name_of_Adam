using UnityEngine;

public class Buff_Sadism : Buff
{
    private int attackUp = 3;
    private int totalUp = 0;
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sadism;

        _name = "가학";

        _description = "공격력이 3 증가합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _statBuff = true;

        _dispellable = true;

        _caster = caster;

        _owner = owner;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        return false;
    }

    public override void Stack()
    {
        totalUp += attackUp;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK += totalUp;

        return stat;
    }

    public override void SetValue(int num)
    {
        attackUp = num;
    }
}