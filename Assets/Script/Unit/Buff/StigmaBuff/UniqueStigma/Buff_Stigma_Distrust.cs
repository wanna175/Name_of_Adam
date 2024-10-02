using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Distrust : Buff
{
    private Stat _unitStat = new();

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Distrust;

        _name = "Distrust";

        _buffActiveTiming = ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;

        _statBuff = true;

        SetStat();
    }

    public override Stat GetBuffedStat()
    {
        return _unitStat;
    }

    private void SetStat()
    {
        int statTotal = 0;

        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Team == _owner.Team && unit != _owner &&
                unit.DeckUnit.DeckUnitTotalStat.ATK + unit.DeckUnit.DeckUnitTotalStat.MaxHP > statTotal)
            {
                _unitStat = unit.DeckUnit.DeckUnitTotalStat;
                _unitStat.CurrentHP = _unitStat.MaxHP;
                statTotal = unit.DeckUnit.DeckUnitTotalStat.ATK + unit.DeckUnit.DeckUnitTotalStat.MaxHP;
            }
        }
        _unitStat.ManaCost = 0;
    }
}