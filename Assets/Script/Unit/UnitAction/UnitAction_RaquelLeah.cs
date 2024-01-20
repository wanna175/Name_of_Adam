using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_RaquelLeah : UnitAction
{
    private BattleUnit _owner = null;
    private bool _isChanged = false;
    const int _speedDifference = 65;
    const int _attackDifference = 10;

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count == 0)
            return false;

        if (_isChanged)
        {
            List<BattleUnit> units = BattleManager.Field.GetArroundUnits(attackUnit.Location, attackUnit.Team == Team.Player ? Team.Enemy : Team.Player);
            BattleManager.Instance.AttackStart(attackUnit, units.Distinct().ToList());
        }
        else 
        {
            BattleManager.Instance.AttackStart(attackUnit, hits);
        }

        return true;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            _owner = caster;
            _owner.SetBuff(new Buff_Raquel());
        }
        else if (
                (activeTiming & ActiveTiming.FIELD_UNIT_SUMMON) == ActiveTiming.FIELD_UNIT_SUMMON ||
                (activeTiming & ActiveTiming.TURN_START) == ActiveTiming.TURN_START
            )
        {
            InsertUnitInOrlder();
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END)
        {
            ChangeState(caster);
        }

        return false;
    }

    private void InsertUnitInOrlder()
    {
        if ( ! BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
        {
            return;
        }

        int ChangedSpeed = _owner.BattleUnitTotalStat.SPD - _speedDifference;

        int InsertIndex = 0;

        /*
        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.BattleUnitTotalStat.SPD > ChangedSpeed)
            {
                InsertIndex++;
            }
        }
        */

        BattleManager.Data.BattleOrderInsert(InsertIndex, _owner, ChangedSpeed);

    }

    private void ChangeState(BattleUnit caster)
    {
        _isChanged = !_isChanged;

        caster.AnimatorSetBool("isChanged", _isChanged);

        if (_isChanged)
        {
            _owner.DeleteBuff(BuffEnum.Rachel);
            _owner.SetBuff(new Buff_Leah());
        }
        else
        {
            _owner.DeleteBuff(BuffEnum.Leah);
            _owner.SetBuff(new Buff_Raquel());
        }
    }
}