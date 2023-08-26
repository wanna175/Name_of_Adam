using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Field : MonoBehaviour
{
    private Dictionary<Vector2, Tile> _tileDict = new();
    public Dictionary<Vector2, Tile> TileDict => _tileDict;

    public FieldColorType FieldType => _fieldType;
    private FieldColorType _fieldType = FieldColorType.none;

    private List<Vector2> _coloredTile = new();
    public List<Vector2> ColoredTile => _coloredTile;

    // 필드의 생성을 위한 필드의 위치
    private Vector3 FieldPosition => new Vector3(0, -2f, 2.5f);
    private Vector3 FieldRotation => new Vector3(24f, 0, 0);
    private Vector3 FieldScale => new Vector3(20.46f, 7.57f, 1f);

    private const int MaxFieldX = 6;
    private const int MaxFieldY = 3;

    private Color ColorList(FieldColorType color)
    {
        return color switch
        {
            FieldColorType.Move => new Color32(23, 114, 102, 40),
            FieldColorType.Attack => new Color32(220, 20, 60, 40),
            FieldColorType.none => new Color32(0, 0, 0, 0),
            _ => new Color32(23, 114, 102, 40),
        };
    }

    private void Awake()
    {
        for (int i = 0; i < MaxFieldY; i++)
            for (int j = 0; j < MaxFieldX; j++)
                _tileDict.Add(new Vector2(j, i), CreateTile(j, i));

        transform.position = FieldPosition;
        transform.eulerAngles = FieldRotation;
        transform.localScale = FieldScale;
    }

    private Tile CreateTile(int x, int y)
    {
        x -= MaxFieldX / 2;
        y -= MaxFieldY / 2;

        float disX = transform.localScale.x / MaxFieldX;
        float disY = transform.localScale.y / MaxFieldY;

        float locX = (disX * x) + (disX * 0.5f);
        float locY = disY * y + 1.5f;

        Vector3 tilePos = new(locX, transform.position.y + locY);
        GameObject tileObject = GameManager.Resource.Instantiate("Tile", transform);

        return tileObject.GetComponent<Tile>().Init(tilePos);
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

    public List<BattleUnit> GetArroundUnits(Vector2 unitCoord)
    {
        List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
        List<BattleUnit> units = new();

        foreach (Vector2 udlr in UDLR)
        {
            BattleUnit targetUnit = GetUnit(unitCoord + udlr);
            if (targetUnit == null)
                continue;
            units.Add(targetUnit);
        }

        return units;
    }

    //십자가 범위 유닛
    public List<Vector2> GetCrossCoord(Vector2 unitCoord)
    {
        List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
        List<Vector2> Coords = new();
        Coords.Add(unitCoord);

        for(int i=1; i < MaxFieldX; i++)
        {
            foreach(Vector2 udlr in UDLR)
            {
                Vector2 vec = unitCoord + udlr * i;
                if (IsInRange(vec))
                    Coords.Add(vec);
            }
        }

        return Coords;
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
        if (IsInRange(dest) == false || current == dest)
            return;

        BattleUnit currentUnit = TileDict[current].Unit;
        BattleUnit destUnit = TileDict[dest].Unit;
        
        if (TileDict[dest].UnitExist)
        {
            if (currentUnit.Team == destUnit.Team && GetArroundUnits(current).Contains(destUnit))
            {
                ExitTile(current);
                ExitTile(dest);

                EnterTile(currentUnit, dest, true);
                EnterTile(destUnit, current, true);
                return;
            }
        }
        else
        {
            ExitTile(current);
            EnterTile(currentUnit, dest, true);
        }
    }

    // 지정한 위치에 있는 타일의 좌표를 반환
    public Vector3 GetTilePosition(Vector2 coord)
    {
        Vector3 position = TileDict[coord].transform.position;
        
        float sizeY = TileDict[coord].transform.lossyScale.y * 0.2f;
        position.y -= sizeY;

        return position;
    }

    public void SetNextActionTileColor(BattleUnit unit, FieldColorType fieldType)
    {
        List<Vector2> rangeList = new();

        if (fieldType == FieldColorType.Move)
            rangeList = unit.GetMoveRange();
        else if (fieldType == FieldColorType.Attack)
            rangeList = unit.GetAttackRange();

        foreach (Vector2 vec in rangeList)
        {
            Vector2 range = unit.Location + vec;
            if (IsInRange(range))
            {
                TileDict[range].SetColor(ColorList(fieldType));
                _coloredTile.Add(range);
            }
        }
    }

    public void SetSpawnTileColor(FieldColorType fieldType)
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
        {
            if (!items.Value.UnitExist && IsPlayerRange(items.Key))
            {
                items.Value.SetColor(ColorList(fieldType));
                _coloredTile.Add(items.Key);
            }    
        }

        _fieldType = fieldType;
    }

    public void SetUnitTileColor(FieldColorType fieldType)
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
        {
            if (items.Value.UnitExist)
            {
                items.Value.SetColor(ColorList(fieldType));
                _coloredTile.Add(items.Key);
            }
        }

        _fieldType = fieldType;
    }

    public void SetEnemyUnitTileColor(FieldColorType fieldType)
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
        {
            if (items.Value.UnitExist && items.Value.Unit.Team == Team.Enemy)
            {
                items.Value.SetColor(ColorList(fieldType));
                _coloredTile.Add(items.Key);
            }
        }

        _fieldType = fieldType;
    }
    
    public void SetFriendlyUnitTileColor(FieldColorType fieldType)
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
        {
            if (items.Value.UnitExist && items.Value.Unit.Team == Team.Player)
            {
                items.Value.SetColor(ColorList(fieldType));
                _coloredTile.Add(items.Key);
            }
        }

        _fieldType = fieldType;
    }

    public void ClearAllColor()
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
        {
            items.Value.SetColor(Color.white);
        }
        _coloredTile.Clear();
    }

    public void EnterTile(BattleUnit unit, Vector2 coord, bool move = false)
    {
        TileDict[coord].EnterTile(unit);

        unit.SetLocate(coord, move);
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

        return false;
    }

    private UI_Info _hoverInfo;

    public void MouseEnterTile(Tile tile)
    {
        FieldShowInfo(tile);
    }

    public void MouseExitTile(Tile tile)
    {
        FieldCloseInfo(tile);
    }

    public void FieldShowInfo(Tile tile)
    {
        if (tile.UnitExist)
        {
            BattleUnit unit = tile.Unit;
            _hoverInfo = BattleManager.BattleUI.ShowInfo();
            _hoverInfo.SetInfo(unit);
        }
    }

    public void FieldCloseInfo(Tile tile)
    {
        if (tile.UnitExist && _hoverInfo != null)
        {
            BattleManager.BattleUI.CloseInfo(_hoverInfo);
        }
    }
}