using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCutSceneData
{
    public BattleUnit AttackUnit;
    public List<BattleUnit> HitUnits;

    // 각 유닛의 위치
    public Vector3 AttackPosition;
    public List<Vector3> HitPosition;
    // 각 유닛의 타일 위치
    public Vector2 AttackLocation;
    public List<Vector2> HitLocation;

    // 공격 타입
    public CutSceneMoveType MoveType;
    // 얼마나 줌할 것인지
    public float ZoomSize;

    // 확대 할 위치
    public Vector3 ZoomLocation;
    // 움직일 위치
    public Vector3 MovePosition;
    // 공격 유닛이 바라보는 방향
    public bool AttackUnitFlipX;

    public BattleCutSceneData(BattleUnit attackUnit, List<BattleUnit> hitUnits)
    {
        AttackUnit = attackUnit;
        HitUnits = hitUnits;

        AttackLocation = AttackUnit.Location;
        AttackPosition = BattleManager.Field.GetTilePosition(AttackLocation);
        HitLocation = new();
        HitPosition = new();
        foreach (BattleUnit unit in HitUnits)
        {
            HitLocation.Add(unit.Location);
            HitPosition.Add(BattleManager.Field.GetTilePosition(unit.Location));
        }

        MoveType = AttackUnit.Data.AnimType.MoveType;
        ZoomSize = AttackUnit.Data.AnimType.ZoomSize;

        MovePosition = BattleManager.Field.GetTilePosition(GetMoveLocation(AttackUnit));
        
        Vector3 vec = Vector3.Lerp(MovePosition, HitPosition[0], 0.5f);
        ZoomLocation = new Vector3(vec.x, vec.y, 0);
    }

    Vector2 GetMoveLocation(BattleUnit unit)
    {
        if (AttackLocation.x != HitLocation[0].x)
        {
            AttackUnitFlipX = AttackLocation.x < HitLocation[0].x;
        }
        else
        {
            AttackUnitFlipX = (HitLocation[0].x > 3) ^ ( HitLocation[0].y > AttackLocation.y);/* MaxFieldX / 2 */
        }

        if (unit.Data.AnimType.MoveType == CutSceneMoveType.stand)
            return unit.Location;

        Vector2 moveTile = HitLocation[0];

        if (AttackUnitFlipX)
        {
            moveTile.x = HitLocation[0].x - 1;

            if (!BattleManager.Field.IsInRange(moveTile))
            {
                moveTile.x = HitLocation[0].x + 1;
                AttackUnitFlipX = false;
            }
        }
        else
        {
            moveTile.x = HitLocation[0].x + 1;

            if (!BattleManager.Field.IsInRange(moveTile))
            {
                moveTile.x = HitLocation[0].x - 1;
                AttackUnitFlipX = true;
            }
        }

        return moveTile;
    }
}