using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    targeting,
    rangeAttack,

    none
}

[CreateAssetMenu(fileName = "Effect_Attack", menuName = "Scriptable Object/Effect_Attack", order = 3)]
public class Effect_Attack : EffectSO
{
    [SerializeField] public AttackType attackType;
    [SerializeField] RangeSO range;    // 공격 범위
    [SerializeField] float DMG;        // 데미지 배율



    // 공격 실행
    public override void Effect(BattleUnit caster)
    {
        List<List<Tile>> Tiles = GameManager.Instance.BattleMNG.BattleDataMNG.FieldMNG.TileArray;
        List<Vector2> RangeList = GetRange();

        float CharATK = caster.BattleUnitSO.stat.ATK;

        if (attackType == AttackType.rangeAttack)
        {
            List<BattleUnit> _BattleUnits = new List<BattleUnit>();

            // 공격 범위를 향해 공격
            for (int i = 0; i < RangeList.Count; i++)
            {
                int x = caster.UnitMove.LocX - (int)RangeList[i].x;
                int y = caster.UnitMove.LocY - (int)RangeList[i].y;

                if (0 <= x && x < 8)
                {
                    if (0 <= y && y < 3)
                    {
                        Tiles[y][x].SetColor(Color.red);

                        if (Tiles[y][x].isOnTile)
                        {
                            if (caster.BattleUnitSO.team != Tiles[y][x].TileUnit.BattleUnitSO.team)
                            {
                                _BattleUnits.Add(Tiles[y][x].TileUnit);
                            }
                        }
                    }
                }
            }
            caster.UnitAction.OnAttackRange(_BattleUnits);
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

            BattleUnit unit = null;

            // 공격 범위가 필드를 벗어나지 않은 경우 공격
            if (0 <= x && x < 8)
            {
                if (0 <= y && y < 3)
                {
                    Tiles[y][x].SetColor(Color.red);

                    if (Tiles[y][x].isOnTile)
                    {
                        if (caster.BattleUnitSO.team != Tiles[y][x].TileUnit.BattleUnitSO.team)
                            unit = Tiles[y][x].TileUnit;
                    }

                }
            }
            caster.UnitAction.OnAttackTarget(unit);
        }
    }

    public List<Vector2> GetRange() => range.GetRange();
}
