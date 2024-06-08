using UnityEngine;
using System.Collections.Generic;

public class UnitAction_Tubalcain : UnitAction
{
    private List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    private bool _isMove = false;

    public override void AISkillUse(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
        {
            BattleManager.Instance.DirectAttack(attackUnit);
            return;
        }

        List<Vector2> MinHPUnit = MinHPSearch(ChargeAttackSearch(attackUnit));

        if (MinHPUnit.Count > 0)
        {
            Attack(attackUnit, MinHPUnit[Random.Range(0, MinHPUnit.Count)]);
        }
        else
            BattleManager.Instance.EndUnitAction();
    }

    private Dictionary<Vector2, int> ChargeAttackSearch(BattleUnit caster)
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

                if (unit != null && attackRange.Contains(direction * i))
                {
                    if (unit.Team != caster.Team)
                    {
                        AttackableFour.Add(tempPosition, unit.GetHP());
                    }
                    
                    break;
                }
            }
        }

        return AttackableFour;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.BEFORE_ATTACK) == ActiveTiming.BEFORE_ATTACK)
        {
            if (_isMove)
                receiver.ChangeFall(1, FallAnimMode.On, 0.75f);
        }
        else if ((activeTiming & ActiveTiming.DAMAGE_CONFIRM) == ActiveTiming.DAMAGE_CONFIRM)
        {
            if (_isMove)
                caster.ChangedDamage *= 2;
        }
        else if ((activeTiming & ActiveTiming.AFTER_ATTACK) == ActiveTiming.AFTER_ATTACK)
        {
            caster.AnimatorSetBool("isAttackStart", false);
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END)
        {
            if (_isMove)
            {
                caster.SetBuff(new Buff_Stun());

                _isMove = false;
            }
        }

        return false;
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        BattleUnit receiver;

        if (hits.Count == 0)
        {
            return false;
        }
        else if (hits.Count == 1)
        {
            receiver = hits[0];
        }
        else
        {
            receiver = BattleManager.Field.GetUnit(coord);
        }

        if (BattleManager.Field.GetArroundUnits(attackUnit.Location).Contains(receiver))
        {
            BattleManager.Instance.AttackStart(attackUnit, hits);
            _isMove = false;
            return true;
        }

        if (!ChargeAttackSearch(attackUnit).ContainsKey(coord))
            return false;

        Vector2 hookDir = (attackUnit.Location - receiver.Location).normalized;
        Vector2 moveVector = receiver.Location + hookDir;

        _isMove = true;

        attackUnit.AnimatorSetBool("isAttackStart", true);
        BattleManager.Instance.MoveUnit(attackUnit, moveVector, 5f);
        BattleManager.Instance.AttackStart(attackUnit, hits);

        return true;
    }
}