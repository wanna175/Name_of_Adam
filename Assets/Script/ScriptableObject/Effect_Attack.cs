using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    none,

    targeting,
    rangeAttack
}

public enum RangeType
{
    none,

    noneMove,
    tracking,
    center
}

[CreateAssetMenu(fileName = "Effect_Attack", menuName = "Scriptable Object/Effect_Attack", order = 3)]
public class Effect_Attack : EffectSO
{
    [SerializeField] public AttackType attackType;
    [SerializeField] public RangeType rangeType;
    [SerializeField] RangeSO range;    // 공격 범위


    // 공격 실행
    public override void Effect(BattleUnit caster)
    {
        FieldManager _FieldMNG = GameManager.Instance.BattleMNG.BattleDataMNG.FieldMNG;
        List<Vector2> RangeList = GetRange();
        List<BattleUnit> _BattleUnits = new List<BattleUnit>();

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
                    _BattleUnits.Add(_unit);
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
                _BattleUnits.Add(_unit);
        }
        caster.UnitAction.OnAttack(_BattleUnits);
    }

    public List<Vector2> GetRange() => range.GetRange();
}
