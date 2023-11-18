using UnityEngine;
using System.Collections.Generic;

public class UnitAction_Centaurus : UnitAction
{
    private List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    private bool _isMove = false;

    public override void AISkillUse(BattleUnit attackUnit)
    {
        List<Vector2> MinHPUnit = CentaurusAttackSearch(attackUnit);

        if (MinHPUnit.Count > 0)
        {
            Attack(attackUnit, MinHPUnit[Random.Range(0, MinHPUnit.Count)]);
        }
        else
            BattleManager.Instance.EndUnitAction();
    }

    private List<Vector2> CentaurusAttackSearch(BattleUnit caster)
    {
        Dictionary<Vector2, int> AttackableFour = new();
        List<Vector2> attackRange = caster.GetAttackRange();

        foreach (Vector2 direction in UDLR)
        {
            for (int i = 1; i < 6; i++)
            {
                Vector2 tempPosition = caster.Location + (direction * i);

                if (!_field.IsInRange(tempPosition))
                    break;

                BattleUnit unit = _field.GetUnit(tempPosition);

                if (unit != null)
                {
                    if (attackRange.Contains(direction * i))
                    {
                        AttackableFour.Add(tempPosition, unit.GetHP());
                    }

                    break;
                }
            }
        }
       
        return MinHPSearch(AttackableFour);
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if (activeTiming == ActiveTiming.DAMAGE_CONFIRM)
        {
            if (_isMove)
                caster.ChangedDamage *= 2;
        }
        else if (activeTiming == ActiveTiming.ATTACK_TURN_END)
        {
            if (_isMove)
            {
                Buff_Stun stun = new();
                caster.SetBuff(stun);

                _isMove = false;
            }
        }

        return false;
    }

    public override void ActionStart(BattleUnit attackUnit, List<BattleUnit> hits)
    {
        if (hits.Count != 1)
        {
            Debug.Log("Centaurus Attack Error");

            return;
        }

        BattleUnit receiver = hits[0];

        foreach (Vector2 direction in UDLR)
        {
            if (receiver.Location + direction == attackUnit.Location)
            {
                BattleManager.Instance.AttackStart(attackUnit, hits);
                _isMove = false;
                return;
            }
        }

        float currntMin = 100f;
        Vector2 moveVector = attackUnit.Location;

        foreach (Vector2 direction in UDLR)
        {
            Vector2 vec = receiver.Location + direction;
            float sqr = (vec - attackUnit.Location).sqrMagnitude;

            if (currntMin > sqr)
            {
                currntMin = sqr;
                if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
                {
                    moveVector = vec;
                }
            }
            else if (currntMin == sqr)
            {
                if (direction.x != 0 && BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
                {
                    moveVector = vec;
                }
            }
        }
        _isMove = true;

        BattleManager.Instance.MoveUnit(attackUnit, moveVector);
        BattleManager.Instance.AttackStart(attackUnit, hits);
    }


    public override void Action(BattleUnit attackUnit, BattleUnit receiver) => attackUnit.Attack(receiver, attackUnit.BattleUnitTotalStat.ATK);
}