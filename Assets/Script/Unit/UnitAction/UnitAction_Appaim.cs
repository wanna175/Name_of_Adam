using UnityEngine;
using System.Collections.Generic;

public class UnitAction_Appaim : UnitAction
{
    //0 = sword, 1 = staff, 2 = book
    private int _trinityState  = 0;
    private bool _isStateUpdate = false;
    private Buff _appaimBuff = null;

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
        if (DirectAttackCheck())
        {
            BattleManager.Instance.DirectAttack(attackUnit);
            return;
        }

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

            if (hitUnits.Count > 0)
            {
                ActionStart(attackUnit, hitUnits, new());
            }
            else
            {
                BattleManager.Instance.EndUnitAction();
            }
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

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
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

            if (inRangeUnits.Count > 0)
            {
                BattleManager.Instance.AttackStart(attackUnit, inRangeUnits);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        { 
            BattleManager.Instance.AttackStart(attackUnit, hits);
            return false;
        }
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver) 
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            _appaimBuff = new Buff_Appaim();
            _appaimBuff.SetValue(0);
            caster.SetBuff(_appaimBuff);
        }
        else if ((activeTiming & ActiveTiming.TURN_START) == ActiveTiming.TURN_START)
        {
            _isStateUpdate = false;
        }
        else if (((activeTiming & ActiveTiming.AFTER_ATTACK) == ActiveTiming.AFTER_ATTACK || (activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END) && !_isStateUpdate)
        {
            _isStateUpdate = true;

            switch (UpdateTrinityState())
            {
                case 0:
                    caster.SetAttackRange(staffRange);
                    caster.AnimatorSetInteger("state", 0);
                    caster.DeleteBuff(_appaimBuff.BuffEnum);
                    _appaimBuff = new Buff_Appaim();
                    _appaimBuff.SetValue(0);
                    caster.SetBuff(_appaimBuff);
                    break;
                case 1:
                    caster.SetAttackRange(swordRange);
                    caster.AnimatorSetInteger("state", 1);
                    caster.DeleteBuff(_appaimBuff.BuffEnum);
                    _appaimBuff = new Buff_Appaim();
                    _appaimBuff.SetValue(1);
                    caster.SetBuff(_appaimBuff);
                    break;
                case 2:
                    caster.SetAttackRange(bowRange);
                    caster.AnimatorSetInteger("state", 2);
                    caster.DeleteBuff(_appaimBuff.BuffEnum);
                    _appaimBuff = new Buff_Appaim();
                    _appaimBuff.SetValue(2);
                    caster.SetBuff(_appaimBuff);
                    break;
                default:
                    break;
            }
        }
        else if ((activeTiming & ActiveTiming.MOVE_TURN_START) == ActiveTiming.MOVE_TURN_START)
        {
            bool moveSkip = false;
            if (_trinityState == 0)
                moveSkip = true;

            return moveSkip;

        }
        else if ((activeTiming & ActiveTiming.BEFORE_ATTACK) == ActiveTiming.BEFORE_ATTACK)
        {
            if (receiver != null)
                receiver.ChangeFall(1);
        }

        return false;
    }

    private int UpdateTrinityState()
    {
        _trinityState = (_trinityState + 1) % 3;
        return _trinityState;
    }
}