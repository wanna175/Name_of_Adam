using UnityEngine;

public class Buff_Stigma_BrokenSword : Buff
{
    private int _attackUp;
    private int _attackUpCount = 0;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_BrokenSword;

        _name = "Broken Sword";

        _description = "Broken Sword Info";

        _buffActiveTiming = ActiveTiming.TURN_END;

        _owner = owner;

        _stigmataBuff = true;

        _statBuff = true;

        _attackUp = owner.DeckUnit.DeckUnitTotalStat.ATK / 2;
    }

    public override bool Active(BattleUnit caster)
    {
        if (!IsActive && _attackUpCount != 0)
        {
            _attackUpCount = 0;
        }

        _attackUpCount++;

        if (_attackUpCount == 2)
        {
            for (int i = 0; i < 5; i++)
                _owner.SetBuff(new Buff_Stun());
        }

        IsActive = true;

        return false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        if (_attackUpCount < 2)
        {
            stat.ATK += _attackUp;
        }

        return stat;
    }
}