using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackType
{
    targeting,
    rangeAttack
}

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/Skill", order = 1)]
public class SkillSO : ScriptableObject
{
    [SerializeField] public AttackType attackType;
    [SerializeField] public RangeType rangeType;
    [SerializeField] RangeSO range;    // 공격 범위

    [SerializeField] public List<EffectSO> EffectList;

    // 이펙트 리스트 안의 모든 이펙트가 하나의 이펙트로 묶여서 사용된다.
    public void use(BattleUnit ch)
    {
        for (int i = 0; i < EffectList.Count; i++)
        {
            EffectList[i].Effect(ch);
        }
    }

    void RangeCheck(BattleUnit caster)
    {
        FieldManager _FieldMNG = GameManager.Instance.FieldMNG;
        List<Vector2> RangeList = GetRange();
        List<BattleUnit> _HitUnits = new List<BattleUnit>();

        if (attackType == AttackType.rangeAttack)
        {
            // 공격범위 안에 있는 모든 대상을 리스트에 넣는다.
            for (int i = 0; i < RangeList.Count; i++)
            {
                BattleUnit _unit = null;
                int x = caster.UnitMove.LocX - (int)RangeList[i].x;
                int y = caster.UnitMove.LocY - (int)RangeList[i].y;

                // 공격 범위가 필드를 벗어나지 않았다면 범위 위의 적 유닛을 가져온다
                _unit = _FieldMNG.RangeCheck(caster, x, y);

                if (_unit != null)
                    _HitUnits.Add(_unit);
            }
        }
        else if (attackType == AttackType.targeting)
        {

            int x = (int)caster.SelectTile.x;
            int y = (int)caster.SelectTile.y;

            if (x == -1 && y == -1)
            {
                x = caster.UnitMove.LocX;
                y = caster.UnitMove.LocY;
            }

            BattleUnit _unit = null;

            _unit = _FieldMNG.RangeCheck(caster, x, y);

            if (_unit != null)
                _HitUnits.Add(_unit);
        }
        caster.UnitAction.OnAttack(_HitUnits);
    }

    public List<Vector2> GetRange() => range.GetRange();
}