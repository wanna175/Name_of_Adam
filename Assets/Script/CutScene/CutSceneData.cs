using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneData
{
    public BattleUnit AttackUnit;
    public List<BattleUnit> HitUnits;

    // 각 유닛의 위치
    public Vector3 AttackPosition;
    public List<Vector3> HitPosition = new List<Vector3>();
    // 각 유닛의 타일 위치
    public Vector2 AttackLocation;
    public List<Vector2> HitLocation = new List<Vector2>();

    // 공격 타입
    public CutSceneMoveType MoveType;
    // 기존의 줌 사이즈
    public float DefaultZoomSize;
    // 얼마나 줌할 것인지
    public float ZoomSize;
    // 어느정도 기울일 것인지
    public float TiltPower;

    // 확대 할 위치
    public Vector3 ZoomLocation;
    // 움직일 위치
    public Vector3 MovePosition;
    // 공격 유닛이 어느 방향에 있는지
    public int AttackUnitDirection;

    public CutSceneData(BattleUnit AttackUnit, List<BattleUnit> HitUnits)
    {
        this.AttackUnit = AttackUnit;
        this.HitUnits = HitUnits;

        AttackPosition = AttackUnit.transform.position;
        AttackLocation = AttackUnit.Location;
        HitPosition = new List<Vector3>();
        HitLocation = new List<Vector2>();
        foreach (BattleUnit unit in HitUnits)
        {
            HitPosition.Add(unit.transform.position);
            HitLocation.Add(unit.Location);
        }

        DefaultZoomSize = Camera.main.fieldOfView;
        MoveType = AttackUnit.Data.AnimType.MoveType;
        ZoomSize = AttackUnit.Data.AnimType.ZoomSize;
        TiltPower = AttackUnit.Data.AnimType.TiltPower;

        MovePosition = BattleManager.Field.GetTilePosition(GetMoveLocation(AttackUnit));
        
        Vector3 vec = Vector3.Lerp(MovePosition, HitPosition[0], 0.5f);
        ZoomLocation = new Vector3(vec.x, vec.y, 0);
    }

    Vector2 GetMoveLocation(BattleUnit unit)
    {
        if (unit.Data.AnimType.MoveType == CutSceneMoveType.stand)
            return unit.Location;

        Vector2 AttackUnitTile = AttackLocation;
        Vector2 HitUnitTile = HitLocation[0];

        Vector2 moveTile = HitUnitTile;

        if (AttackUnitTile.x < HitUnitTile.x)
        {
            moveTile.x -= 1;
            AttackUnitDirection = -1;
        }
        else if (HitUnitTile.x < AttackUnitTile.x)
        {
            moveTile.x += 1;
            AttackUnitDirection = 1;
        }
        else
        {
            moveTile = AttackUnitTile;
            AttackUnitDirection = 0;
        }

        return moveTile;
    }
}