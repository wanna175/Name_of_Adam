using UnityEngine;

public class Buff_Stigma_Sadism : Buff
{
    private int _attackUp = 0;
    private int _totalUp = 0;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Sadism;

        _name = "Sadism";

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _statBuff = true;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _totalUp += _owner.AttackUnitNum * _attackUp;

        return false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK += _totalUp;

        return stat;
    }

    public override void SetValue(int num)
    {
        _attackUp += num;
    }
}