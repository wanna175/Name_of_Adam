
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitAction_Libiel : UnitAction
{
    private Buff_Libiel _libielBuff;
    private int _libielCount = 1;

    bool[] _normalRange = new bool[] {
            false, false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, false, true, false, false, false, false, false,
            false, false, false, false, true, true, true, false, false, false, false,
            false, false, false, false, false, true, false, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false, false
    };

    bool[] _rageRange = new bool[] {
            false, false, false, true, true, true, true, true, false, false, false,
            false, false, false, true, true, true, true, true, false, false, false,
            false, false, false, true, true, true, true, true, false, false, false,
            false, false, false, true, true, true, true, true, false, false, false,
            false, false, false, true, true, true, true, true, false, false, false
    };

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            _libielBuff = new Buff_Libiel();
            caster.SetBuff(_libielBuff);
        }
        else if ((activeTiming & ActiveTiming.AFTER_ATTACKED) == ActiveTiming.AFTER_ATTACKED)
        {
            _libielCount = _libielCount % 3 + 1;
            _libielBuff.SetValue(_libielCount);
            caster.SetAttackRange(_libielCount == 3 ? _rageRange : _normalRange);
        }
        else if ((activeTiming & ActiveTiming.BEFORE_ATTACKED) == ActiveTiming.BEFORE_ATTACKED)
        {
            if (_libielCount == 1)
                caster.ChangedDamage /= 2;
        }
        else if ((activeTiming & ActiveTiming.DAMAGE_CONFIRM) == ActiveTiming.DAMAGE_CONFIRM)
        {
            if (_libielCount == 2 || _libielCount == 3)
                caster.ChangedDamage *= 2;
        }
        else if ((activeTiming & ActiveTiming.BEFORE_ATTACK) == ActiveTiming.BEFORE_ATTACK)
        {
            if (_libielCount == 3 && receiver != null)
            {
                receiver.ChangeFall(1, caster, FallAnimMode.On);
            }
        }

        return false;
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count != 1)
            return false;

        foreach (Vector2 vec in attackUnit.GetAttackRange())
        {
            if ((coord.x - attackUnit.Location.x > 0) == (vec.x > 0))
            {
                BattleUnit unit = BattleManager.Field.GetUnit(vec + attackUnit.Location);

                if (unit != null && unit.Team != attackUnit.Team)
                {
                    hits.Add(unit);
                }
            }
            else if (vec.x * (coord.x - attackUnit.Location.x) == 0)
            { 
            
            }
        }

        BattleManager.Instance.AttackStart(attackUnit, hits.Distinct().ToList());
        return true;
    }

    public override List<Vector2> GetSplashRangeForField(BattleUnit unit, Tile targetTile, Vector2 caster)
    {
        List<Vector2> splashRangeList = new();
        Vector2 target = BattleManager.Field.GetCoordByTile(targetTile);

        foreach (Vector2 vec in unit.GetAttackRange())
        {
            if ((target.x - unit.Location.x > 0) == (vec.x > 0) && BattleManager.Field.IsInRange(vec + caster))
            {
                splashRangeList.Add(vec + caster);
            }
        }

        return splashRangeList;
    }
}