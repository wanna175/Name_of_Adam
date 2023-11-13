using UnityEngine;
using System.Collections.Generic;

public class UnitAction_Trinity : UnitAction
{
    //0 = staff, 1 = sword, 2 = bow
    private int _trinityState  = 0;

    bool[] staffRange = new bool[] {
            true, true, true, true, true, true, true, true, true, true, true,
            true, true, true, true, false, false, false, true, true, true, true,
            true, true, true, true, false, false, false, true, true, true, true,
            true, true, true, true, false, false, false, true, true, true, true,
            true, true, true, true, true, true, true, true, true, true, true
    };

    bool[] swordRange = new bool[] {
            false, false, false, false, false, false, false, false, false, false, false,
            false, false, false, false, true, true, true, false, false, false, false,
            false, false, false, false, true, true, true, false, false, false, false,
            false, false, false, false, true, true, true, false, false, false, false,
            false, false, false, false, false, false, false, false, false, false, false
    };

    bool[] bowRange = new bool[] {
            true, true, true, true, true, true, true, true, true, true, true,
            true, true, true, true, true, true, true, true, true, true, true,
            true, true, true, true, true, true, true, true, true, true, true,
            true, true, true, true, true, true, true, true, true, true, true,
            true, true, true, true, true, true, true, true, true, true, true
    };

    public override void AISkillUse(BattleUnit attackUnit)
    {
        if (_trinityState != 2)
        {
            List<BattleUnit> hitUnits = new();

            foreach (Vector2 range in attackUnit.GetAttackRange())
            {
                BattleUnit unit = BattleManager.Field.GetUnit(attackUnit.Location + range);

                if (unit != null && attackUnit.Team != unit.Team)
                {
                    hitUnits.Add(unit);
                }
            }

            ActionStart(attackUnit, hitUnits);
        }
        else
        { 
            Dictionary<Vector2, int> attackableTile = GetAttackableTile(attackUnit);
            Dictionary<Vector2, int> unitInAttackRange = GetUnitInAttackRangeList(attackUnit, attackableTile);

            if (unitInAttackRange.Count > 0)
            {
                List<Vector2> MinHPUnit = MinHPSearch(unitInAttackRange);

                Attack(attackUnit, MinHPUnit[Random.Range(0, MinHPUnit.Count)]);
            }
            else
            {
                BattleManager.Instance.EndUnitAction();
            }
        }
    }

    public override void ActionStart(BattleUnit attackUnit, List<BattleUnit> hits)
    {
        if (_trinityState != 2)
        {
            List<BattleUnit> inRangeUnits = new();

            foreach (Vector2 range in attackUnit.GetAttackRange())
            {
                BattleUnit unit = BattleManager.Field.GetUnit(attackUnit.Location + range);

                if (unit != null && attackUnit.Team != unit.Team)
                {
                    inRangeUnits.Add(unit);
                }
            }

            BattleManager.Instance.AttackStart(attackUnit, inRangeUnits);
        }
        else
            BattleManager.Instance.AttackStart(attackUnit, hits);
    }


    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver) 
    {
        if (activeTiming == ActiveTiming.ATTACK_TURN_END)
        {
            switch (UpdateTrinityState())
            {
                case 0:
                    caster.SetAttackRange(staffRange);
                    //애니메이션 변경
                    break;
                case 1:
                    caster.SetAttackRange(swordRange);
                    //애니메이션 변경
                    break;
                case 2:
                    caster.SetAttackRange(bowRange);
                    //애니메이션 변경
                    break;
                default:
                    break;
            }
        }
        else if (activeTiming == ActiveTiming.MOVE_TURN_START)
        {
            //for move skip
            return _trinityState == 0;
        }
        else if (activeTiming == ActiveTiming.BEFORE_ATTACK)
        {
            if (_trinityState == 0)
            {
                receiver.ChangeFall(1);
            }
        }

        return false;
    }

    private int UpdateTrinityState()
    {
        _trinityState = (_trinityState + 1) % 3;
        return _trinityState;
    }
}