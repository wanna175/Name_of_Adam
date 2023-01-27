using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{

    [SerializeField] GameObject TilePrefabs;
    [SerializeField] GameObject UnitPrefabs;
    private const int MaxFieldX = 8;
    private const int MaxFieldY = 3;

    BattleManager _BattleMNG;
    BattleDataManager _BattleDataMNG;
    UIManager _UIMNG;

    private Dictionary<Vector2, Tile> _tileDict = new Dictionary<Vector2, Tile>();
    public Dictionary<Vector2, Tile> TileDict => _tileDict;
    public Vector2 FindCoordByTile(Tile tile)
    {
        foreach (KeyValuePair<Vector2, Tile> items in TileDict)
            if (items.Value == tile)
                return items.Key;
        return default;
    }

    // 필드의 생성을 위한 필드의 위치
    public Vector3 FieldPosition => new Vector3(0, -1.4f, 0);
    public Vector3 FieldRotation => new Vector3(16, 0, 0);

    private void Awake()
    {
        for (int i = 0; i < MaxFieldY; i++)
            for (int j = 0; j < MaxFieldX; j++)
                _tileDict.Add(new Vector2(j, i), CreateTile(j, i));

        transform.position = FieldPosition;
        transform.eulerAngles = FieldRotation;
    }

    private void Start()
    {
        _BattleMNG = GameManager.Instance.BattleMNG;
        _BattleDataMNG = _BattleMNG.BattleDataMNG;
        _UIMNG = GameManager.Instance.UIMNG;
    }

    public BattleUnit GetTargetUnit(Vector2 coord)
    {
        if (IsInRange(coord))
        {
            TileDict[coord].SetColor(Color.red);
            return TileDict[coord].UnitOnTile;
        }

        return null;
    }

    private Tile CreateTile(int x, int y)
    {
        x -= MaxFieldX / 2;
        y -= MaxFieldY / 2;

        float disX = transform.localScale.x / MaxFieldX;
        float disY = transform.localScale.y / MaxFieldY;

        float locX = (disX * x) + (disX * 0.5f);
        float locY = disY * y;

        GameObject tile = GameObject.Instantiate(TilePrefabs, transform);
        tile.transform.position = new Vector3(locX, transform.position.y + locY);

        return tile.GetComponent<Tile>();
    }

    private bool IsInRange(Vector2 coord)
    {
        if (TileDict.ContainsKey(coord))
            return true;
        return false;
    }

    // 지정한 위치에 있는 타일의 좌표를 반환
    public Vector3 GetTilePosition(Vector2 coord)
    {
        Vector3 position = TileDict[coord].transform.position;

        float sizeX = TileDict[coord].transform.localScale.x * 0.5f;
        float sizeY = TileDict[coord].transform.localScale.y * 0.5f;
        sizeY += TileDict[coord].transform.localScale.y * 0.5f; // 스프라이트 변경으로 인한 임시조치

        position.x += sizeX;
        position.y += sizeY;

        return position;
    }

    public void ClearAllColor()
    {
        foreach(KeyValuePair<Vector2, Tile> items in TileDict)
        {
            items.Value.SetCanSelect(false);
            items.Value.SetColor(Color.white);
        }
    } 

    public void EnterTile(BattleUnit unit, Vector2 coord)
    {
        TileDict[coord].EnterTile(unit);
    }

    public void ExitTile(Vector2 coord)
    {
        TileDict[coord].ExitTile();
    }

    public bool GetIsOnTile(Vector2 coord) => TileDict[coord].IsOnTile;

    public void TileClick(Tile tile)
    {
        int tileX = 0,
            tileY = 0;
        Vector2 coord = FindCoordByTile(tile);

        // 현재 클릭 상태가 어떤 상태인지, 클릭 가능한지 체크하는 클래스 생성 필요

        // 유닛이 공격할 타겟을 선택중이라면
        if (tile.CanSelect)
        {
            ClearAllColor();
            _BattleMNG.GetNowUnit().TileSelected(tileX, tileY);
            return;
        }
        // 클릭한 타일에 유닛이 있을 시
        else if (tile.UnitOnTile != null)
        {
            //BattleUnit SelectUnit = tile.TileUnit;
            //FieldClear();

            //// 그 유닛이 아군이라면
            //if (tile.TileUnit.BattleUnitSO.MyTeam)
            //{
            //    // 유닛이 보유한 스킬이 타겟팅 형식인지 확인한다.
            //    List<Vector2> vecList = SelectUnit.BattleUnitSO.GetTargetingRange();
            //    SetCanSelect(vecList, SelectUnit);
            //}
        }
        // 클릭한 타일에 유닛이 없을 시
        else
        {
            //핸드를 누르고 타일을 누를 때
            if (_UIMNG.Hands.ClickedHand != 0)
            {
                //범위 외
                if (tileX > 3 && tileY > 2)
                {
                    Debug.Log("out of range");
                }
                else
                {
                    if (_BattleDataMNG.CanUseMana(_UIMNG.Hands.ClickedUnit.GetUnitSO().ManaCost)) //조건문이 참이라면 이미 마나가 소모된 후
                    {
                        _BattleDataMNG.ChangeMana(-1 * _UIMNG.Hands.ClickedUnit.GetUnitSO().ManaCost);
                        GameObject BattleUnitPrefab = Instantiate(UnitPrefabs);

                        _BattleDataMNG.CreatBattleUnit(BattleUnitPrefab, tileX, tileY);

                        _UIMNG.Hands.RemoveHand(_UIMNG.Hands.ClickedHand);
                        _UIMNG.Hands.ClearHand();
                    }
                    else
                    {
                        //마나 부족
                        Debug.Log("not enough mana");
                    }
                }
            }
        }
    }

    public void SetCanSelect(List<Vector2> vecList, BattleUnit SelectUnit)
    {
        if (vecList != null)
        {
            // 타겟팅이 맞다면 범위 표시
            for (int i = 0; i < vecList.Count; i++)
            {
                int x = SelectUnit.LocX - (int)vecList[i].x;
                int y = SelectUnit.LocY - (int)vecList[i].y;

                if (0 <= x && x < 8)
                {
                    if (0 <= y && y < 3)
                        TileDict[new Vector2(y, x)].SetCanSelect(true);
                }
            }
        }
    }
}