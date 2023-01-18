using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitMove : MonoBehaviour
{
    BattleUnit _BattleUnit;
    BattleDataManager _BattleDataMNG;
    FieldManager _FieldMNG;

    #region Loc X, Y
    [SerializeField] int _LocX, _LocY;
    public int LocX => _LocX;
    public int LocY => _LocY;
    #endregion


    private void Awake()
    {
        _BattleUnit = GetComponent<BattleUnit>();
        _FieldMNG = GameManager.Instance.FieldMNG;
    }

    private void Start()
    {
        _BattleDataMNG = GameManager.Instance.BattleMNG.BattleDataMNG;
        MoveOnTile();
    }


    //오브젝트 생성 이전, 최초 위치 설정
    public void setLocate(int x, int y)
    {
        _LocX = x;
        _LocY = y;
    }


    // 이동 경로를 받아와 이동시킨다
    public void MoveLotate(int x, int y)
    {
        _FieldMNG.ExitTile(LocX, LocY);

        int dumpX = _LocX;
        int dumpY = _LocY;

        // 타일 범위를 벗어난 이동이면 이동하지 않음
        if (0 <= _LocX + x && _LocX + x < 8)
            dumpX += x;
        if (0 <= _LocY + y && _LocY + y < 3)
            dumpY += y;

        // x와 y가 -1 이라면(유닛이 사망한 상태라면)
        if(x == -1 && y == -1)
        {
            _LocX = _LocY = -1;
        }
        // 이동할 곳이 비어있지 않다면 이동하지 않음
        else if (!_FieldMNG.GetIsOnTile(dumpX, dumpY))
        {
            _LocX = dumpX;
            _LocY = dumpY;
        }

        MoveOnTile();
    }

    // 타일 위로 이동
    public void MoveOnTile()
    {
        Vector3 vec = _FieldMNG.GetTileLocate(LocX, LocY);
        
        if (vec.x == -1 && vec.y == -1)
        {
            _BattleUnit.UnitRenderer.SetUnitLayer(-10);
        }
        else
        {
            // 현재 타일에 내가 들어왔다고 알려줌 
            transform.position = vec;
            _FieldMNG.EnterTile(_BattleUnit, LocX, LocY);
        }
    }
}
