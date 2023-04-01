using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field : MonoBehaviour
{
    private Dictionary<Vector2, Tile> _tileDict = new Dictionary<Vector2, Tile>();
    public Dictionary<Vector2, Tile> TileDict => _tileDict;

    public List<Vector2> _coloredTile = new List<Vector2>();

    // 필드의 생성을 위한 필드의 위치
    private Vector3 FieldPosition => new Vector3(0, -1.4f, 0);
    private Vector3 FieldRotation => new Vector3(16, 0, 0);

    private const int MaxFieldX = 6;
    private const int MaxFieldY = 3;

    private Color ColorList(ClickType type)
    {
        switch (type)
        {
            case ClickType.Move:
                return Color.yellow;
            case ClickType.Attack:
                return Color.red;
            default:
                return default;
        }
    }

    private void Awake()
    {
        for (int i = 0; i < MaxFieldY; i++)
            for (int j = 0; j < MaxFieldX; j++)
                _tileDict.Add(new Vector2(j, i), CreateTile(j, i));

        transform.position = FieldPosition;
        transform.eulerAngles = FieldRotation;
    }

    // 타일의 좌표값을 리턴한다.
    public Vector2 FindCoordByTile(Tile tile)
    {
        // 타일이 없으면 -1, -1을 반환
        if(tile == null)
        {
            Debug.Log("Tile Parameter is Null");
            return new Vector2(-1, -1);
        }

        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
            if (items.Value == tile)
                return items.Key;

        Debug.Log("Can't find target tile");
        return new Vector2(-1, -1);
    }

    public BattleUnit GetUnit(Vector2 coord)
    {
        if (IsInRange(coord))
            return TileDict[coord].Unit;

        return null;
    }

    private Tile CreateTile(int x, int y)
    {
        x -= MaxFieldX / 2;
        y -= MaxFieldY / 2;

        float disX = transform.localScale.x / MaxFieldX;
        float disY = transform.localScale.y / MaxFieldY;

        float locX = (disX * x) + (disX * 0.5f);
        float locY = disY * y + 1.5f;

        Vector3 tilePos = new Vector3(locX, transform.position.y + locY);
        GameObject tileObject = GameManager.Resource.Instantiate("Tile", transform);
        
        return tileObject.GetComponent<Tile>().Init(tilePos);
    }

    // 타일이 최대 범위를 벗어났는지 확인
    public bool IsInRange(Vector2 coord)
    {
        if (TileDict.ContainsKey(coord))
            return true;
        return false;
    }

    public void MoveUnit(Vector2 current, Vector2 dest)
    {
        if (IsInRange(dest) == false | current == dest)
            return;

        BattleUnit currentUnit = TileDict[current].Unit;
        BattleUnit destUnit = TileDict[dest].Unit;
        
        if (TileDict[dest].UnitExist)
        {
            if (currentUnit.Team == destUnit.Team)
            {
                ExitTile(current);
                ExitTile(dest);

                EnterTile(currentUnit, dest);
                EnterTile(destUnit, current);
                return;
            }
        }
        else
        {
            ExitTile(current);
            EnterTile(currentUnit, dest);
        }
    }

    // 지정한 위치에 있는 타일의 좌표를 반환
    private Vector3 GetTilePosition(Vector2 coord)
    {
        Vector3 position = TileDict[coord].transform.position;

        float sizeX = TileDict[coord].transform.localScale.x * 0.5f;
        float sizeY = TileDict[coord].transform.localScale.y * 0.5f;
        sizeY += TileDict[coord].transform.localScale.y * 0.5f; // 스프라이트 변경으로 인한 임시조치

        position.x += sizeX;
        position.y += sizeY;

        return position;
    }

    // *****
    // 메서드 이름 바꾸기
    public List<Vector2> Get_Abs_Pos(BattleUnit _unit, ClickType _clickType)
    {
        List<Vector2> ResultVector = new List<Vector2>();

        List<Vector2> RangeList = new List<Vector2>();

        if (_clickType == ClickType.Move)
            RangeList = _unit.GetMoveRange();
        else if(_clickType == ClickType.Attack)
            RangeList = _unit.GetAttackRange();

        foreach (Vector2 vec in RangeList)
        {
            Vector2 dump = _unit.Location + vec;
            if(IsInRange(dump))
            {
                ResultVector.Add(dump);
            }
        }

        return ResultVector;
    }


    public void SetTileColor(BattleUnit unit, ClickType clickType)
    {
        List<Vector2> vector = Get_Abs_Pos(unit, clickType);

        foreach (Vector2 vec in vector)
        {
            TileDict[vec].SetColor(ColorList(clickType));
            _coloredTile.Add(vec);
        }
    }

    public void SetTileColor()
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
        {
            if (items.Value.UnitExist == false && IsPlayerRange(items.Key))
            {
                items.Value.SetColor(Color.blue);
                _coloredTile.Add(items.Key);
            }    
        }  
    }

    public void ClearAllColor()
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
        {
            items.Value.SetColor(Color.white);
        }
        _coloredTile.Clear();
    }


    public void EnterTile(BattleUnit unit, Vector2 coord)
    {
        TileDict[coord].EnterTile(unit);

        unit.SetLocate(coord);
        unit.SetPosition(GetTilePosition(coord));
    }

    private void ExitTile(Vector2 coord)
    {
        TileDict[coord].ExitTile();
    }

    // 배치 가능 범위 확인
    public bool IsPlayerRange(Vector2 coord)
    {
        if ((int)coord.x < MaxFieldX / 2)
            return true;

        Debug.Log("out of range");
        return false;
    }

    //리팩토링 무저건 하기.
    public void MouseEnterTile(Tile tile)
    {
        Vector2 coord = FindCoordByTile(tile);
        if (_coloredTile.Contains(coord))
        {
            if (GameManager.Battle.Phase.Current == GameManager.Battle.Phase.Action)
            {
                List<Vector2> range = GameManager.Battle.Data.GetNowUnit().GetSplashRange(coord, GameManager.Battle.Data.GetNowUnit().Location);
                foreach (Vector2 vec in range)
                {
                    TileDict[coord + vec].SetColor(Color.green);
                }
            }
        }
    }

    public void MouseExitTile(Tile tile)
    {
        if (GameManager.Battle.Phase.Current == GameManager.Battle.Phase.Action)
        {
            SetTileColor(GameManager.Battle.Data.GetNowUnit(), ClickType.Attack);
        }
    }
}