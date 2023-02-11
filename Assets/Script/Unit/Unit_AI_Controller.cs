using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AI_Controller : MonoBehaviour
{
    protected BattleDataManager _Data;
    protected Field _field;

    protected BattleUnit caster;

    protected List<Vector2> Unit_in_Attack_Range_TileList = new List<Vector2>();
    protected List<Vector2> Ranged_Unit_in_Attack_Range_TileList = new List<Vector2>();

    protected List<Vector3> Attackable_Tile_List = new List<Vector3>();
    
    protected List<Vector3> Unit_in_Attackable_Range_TileList = new List<Vector3>();
    protected List<Vector3> Ranged_Unit_in_Attackable_Range_TileList = new List<Vector3>();

    protected List<Vector2> FourWay = new List<Vector2>{new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1)};

    void Awake()
    {
        _Data = GameManager.Battle.Data;
        _field = GameManager.Battle.Field;
    }

    public void SetCaster(BattleUnit unit)
    {
        caster = unit;
    }

    protected void Find_Unit_in_Attack_Range()
    {
        //현재 위치에서 공격범위 내의 유닛을 찾는다.
        foreach (Vector2 range in caster.Data.GetRange())
        {
            Vector2 AttackRange = caster.Location + range;

            if (_field.IsInRange(AttackRange))
            {
                if (_field.TileDict[AttackRange].IsOnTile && _field.TileDict[AttackRange].Unit.Team == Team.Player)
                {
                    Unit_in_Attack_Range_TileList.Add(AttackRange);
                }
            }
        }

        foreach (Vector2 unitTile in Unit_in_Attack_Range_TileList)
        {
            if (_field.TileDict[unitTile].Unit.Data.BehaviorType == BehaviorType.원거리)
            {
                Ranged_Unit_in_Attack_Range_TileList.Add(unitTile);
            }
        }
        //Unit_in_Attack_Range_TileList에 유닛이 있는 타일을 모두 저장
        //Ranged_Unit_in_Attack_Range_TileList엔 원거리 유닛이 있는 타일만 저장
    }

    protected void Set_Attackable_Tile()
    {
        //모든 공격 타일을 Attackable_Tile_List에 저장한다. X, Y는 좌표, Z는 원거리/근거리 유무
        foreach (BattleUnit unit in _Data.BattleUnitList)
        {
            if (unit.Team == Team.Player)
            {
                foreach (Vector2 arl in caster.Data.GetRange())
                {
                    Vector3 vector = unit.Location - arl;
                    if (unit.Data.BehaviorType == BehaviorType.원거리)
                        vector.z = 0f;//원거리면 0
                    else
                        vector.z = 0.1f;//근거리면 0.1

                    if (!Attackable_Tile_List.Contains(vector))
                        Attackable_Tile_List.Add(vector);
                }
            }
        }
    }

    protected void Search_Attackable_Tile()
    {
        //유닛을 때릴 수 있는 타일이 이동 범위 내에 있는 지 확인한다.
        //단 위, 아래, 왼, 오른쪽만 이동 가능하다고 가정
        List<Vector3> swapList = new List<Vector3>();

        foreach(Vector2 vec in caster.Data.GetMoveRange())
        {
            for (float i = 0; i <= 0.1f; i += 0.1f)
            {
                Vector3 vec1 = new Vector3(caster.Location.x + vec.x, caster.Location.y + vec.y, i);
                if (Attackable_Tile_List.Contains(vec1))
                {
                    if (_field.TileDict[vec1].IsOnTile)
                        swapList.Add(vec1);
                    else
                        Unit_in_Attackable_Range_TileList.Add(vec1);
                }
            }
        }

        //스왑이 필요한지 확인
        if (Unit_in_Attackable_Range_TileList.Count == 0)
        {
            foreach (Vector3 vec in swapList)
                Unit_in_Attackable_Range_TileList.Add(vec);
        }

        foreach (Vector3 v in Unit_in_Attackable_Range_TileList)
        {
            //원거리인지 확인
            if (v.z == 0)
            {
                Ranged_Unit_in_Attackable_Range_TileList.Add(new Vector3(v.x, v.y, 0));
            }
        }
    }

    protected Vector2 Search_Near_Enemy()
    {
        Vector2 MyPosition = caster.Location;

        float dis = 100f;

        List<Vector3> list_minVec = new List<Vector3>();

        foreach (Vector3 v in Attackable_Tile_List)
        {
            float abs = Mathf.Abs(v.x - MyPosition.x) + Mathf.Abs(v.y - MyPosition.y);
            if (dis > abs)
            {
                dis = abs;
                list_minVec.Clear();
                list_minVec.Add(v);
            }
            else if (dis == abs)
            {
                list_minVec.Add(v);
            }
        }
        Vector3 minVec = list_minVec[Random.Range(0, list_minVec.Count)];

        return minVec;
    }

    public Vector2 Move_Direction(Vector2 minVec)
    {
        //가야하는 위치 minVec을 받아 상하좌우 중 어디로 갈지를 정해 moveVec으로 리턴한다
        Vector2 MyPosition = caster.Location;
        float currntMin = 100f;

        List<Vector2> moveVectorList = new List<Vector2>();

        foreach (Vector2 direction in FourWay)
        {
            Vector2 Vec = new Vector2(MyPosition.x + direction.x, MyPosition.y + direction.y);
            float sqr = (Vec - minVec).sqrMagnitude;

            if (currntMin > sqr)
            {
                currntMin = sqr;
                moveVectorList.Clear();
                if (!_field.TileDict[Vec].IsOnTile)
                {
                    moveVectorList.Add(Vec);
                }
            }
            else if (currntMin == sqr)
            {
                if (!_field.TileDict[Vec].IsOnTile)
                {
                    moveVectorList.Add(Vec);
                }
            }
        }

        if (moveVectorList.Count == 0)
        {
            return MyPosition;
        }
        else
        {
            return moveVectorList[Random.Range(0, moveVectorList.Count)];
        }
    }

    protected Vector2 Unit_Coord(List<Vector2> UnitList)
    {
        return UnitList[Random.Range(0, UnitList.Count)];
    }

    protected Vector2 Unit_Coord(List<Vector3> UnitList)
    {
        return UnitList[Random.Range(0, UnitList.Count)];
    }

    protected void Attack_Unit_in_Range()
    {
        if (Ranged_Unit_in_Attack_Range_TileList.Count > 0)
        {
            //원거리 유닛이 있을 경우
            Debug.Log("범위 내 원거리");
            caster.AttackTileClick(Unit_Coord(Ranged_Unit_in_Attack_Range_TileList));
        }
        else
        {
            //근거리 유닛만 있을 경우
            Debug.Log("범위 내 근거리");
            caster.AttackTileClick(Unit_Coord(Unit_in_Attack_Range_TileList));
        }
    }

    protected void List_Clear()
    {
        Unit_in_Attack_Range_TileList.Clear();
        Ranged_Unit_in_Attack_Range_TileList.Clear();

        Attackable_Tile_List.Clear();
        
        Unit_in_Attackable_Range_TileList.Clear();
        Ranged_Unit_in_Attackable_Range_TileList.Clear();
    }

    public virtual void AI_Action()
    {
        //전달받은 범위에서 유닛을 찾는다.
        Find_Unit_in_Attack_Range();

        //찾은 유닛이 있는지 확인하고, 있다면 원거리인지, 근거리인지 확인한다.
        if (Unit_in_Attack_Range_TileList.Count > 0)
        {
            Attack_Unit_in_Range();
        }
        else
        {
            //공격 범위 내에서 찾은 유닛이 없으면 이동하고 공격한다
            Set_Attackable_Tile();

            Search_Attackable_Tile();

            if (Unit_in_Attackable_Range_TileList.Count > 0)
            {
                if (Ranged_Unit_in_Attackable_Range_TileList.Count > 0)
                {
                    //원거리가 있음
                    //Random.Range(0, Ranged_Unit_in_Attackable_Range_TileList.Count);
                    
                    _field.MoveUnit(caster.Location, Unit_Coord(Ranged_Unit_in_Attackable_Range_TileList));
                    Find_Unit_in_Attack_Range();
                    Attack_Unit_in_Range();
                }
                else
                {
                    //근거리만 있음
                    //Random.Range(0, Unit_in_Attackable_Range_TileList.Count);
                    _field.MoveUnit(caster.Location, Unit_Coord(Unit_in_Attackable_Range_TileList));
                    Find_Unit_in_Attack_Range();
                    Attack_Unit_in_Range();
                }
            }
            else
            {
                _field.MoveUnit(caster.Location, Move_Direction(Search_Near_Enemy()));
                GameManager.Battle.UseNextUnit();
            }
        }
        List_Clear();
    }
}

public class Common_Unit_AI_Controller : Unit_AI_Controller
{
    public override void AI_Action()
    {
        base.AI_Action();
    }
}