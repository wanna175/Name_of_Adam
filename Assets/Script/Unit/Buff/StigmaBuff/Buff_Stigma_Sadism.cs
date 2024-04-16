using UnityEngine;

public class Buff_Stigma_Sadism : Buff
{
    private int attackUp = 0;
    private int totalUp = 0;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sadism;

        _name = "가학";

        _description = "공격 후 공격력이 3 증가합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        totalUp += _owner.AttackUnitNum * attackUp;

        return false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK += totalUp;

        return stat;
    }

    public override void SetValue(int num)
    {
        attackUp += num;
    }
}