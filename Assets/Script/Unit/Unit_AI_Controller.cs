using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AI_Controller : MonoBehaviour
{
    protected BattleDataManager _Data;

    protected Field _field;

    protected BattleUnit caster;


    void Awake()
    {
        _Data = GameManager.Battle.Data;
        _field = GameManager.Battle.Field;
    }

    public void SetCaster(BattleUnit unit)
    {
        caster = unit;
    }

    protected List<Vector2> Unit_in_Attack_Range_TileList = new List<Vector2>();
    protected List<Vector2> Ranged_Unit_in_Attack_Range_TileList = new List<Vector2>();

    protected void Find_Unit_in_Attack_Range()
    {
        //현재 위치에서 공격범위 내의 유닛을 찾는다.
        Unit_in_Attack_Range_TileList.Clear();
        Ranged_Unit_in_Attack_Range_TileList.Clear();

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

    protected bool Is_Unit_Exist()
    {
        //유닛이 있는 타일이 공격 범위 내에 있는지 확인
        return Unit_in_Attack_Range_TileList.Count > 0;
    }

    protected bool Is_Ranged_Unit_Exist()
    {
        //찾은 유닛이 있는 타일 중 원거리 유무 확인 
        return Ranged_Unit_in_Attack_Range_TileList.Count > 0;
    }

    protected Vector2 Unit_Coord()
    {
        return Unit_in_Attack_Range_TileList[Random.Range(0, Unit_in_Attack_Range_TileList.Count)];
    }

    protected Vector2 Ranged_Unit_Coord()
    {
        return Ranged_Unit_in_Attack_Range_TileList[Random.Range(0, Ranged_Unit_in_Attack_Range_TileList.Count)];
    }


    [SerializeField] protected List<Vector3> Attackable_Tile_List = new List<Vector3>();

    protected void Set_Attackable_Tile()
    {
        //모든 공격 타일을 AttackTileList에 저장한다. X, Y는 좌표, Z는 원거리/근거리 유무
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

    protected List<Vector3> Unit_in_Attackable_Range_TileList = new List<Vector3>();
    protected List<Vector3> Ranged_Unit_in_Attackable_Range_TileList = new List<Vector3>();

    protected void Search_Attackable_Tile()
    {
        //유닛을 때릴 수 있는 타일이 이동 범위 내에 있는 지 확인한다.
        //단 위, 아래, 왼, 오른쪽만 이동 가능하다고 가정

        Unit_in_Attackable_Range_TileList.Clear();
        Ranged_Unit_in_Attackable_Range_TileList.Clear();

        for (int i = -1; i <= 1; i += 2)
        {
            for (float j = 0; j <= 0.1f; j += 0.1f)
            {
                Vector3 vec1 = new Vector3(caster.Location.x + i, caster.Location.y, j);
                if (Attackable_Tile_List.Contains(vec1))
                {
                    Unit_in_Attackable_Range_TileList.Add(vec1);
                }

                Vector3 vec2 = new Vector3(caster.Location.x, caster.Location.y + i, j);
                if (Attackable_Tile_List.Contains(vec2))
                {
                    Unit_in_Attackable_Range_TileList.Add(vec2);
                }
            }
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

    protected Vector3 Search_Near_Enemy()
    {
        Vector3 MyPosition = caster.Location;

        float dis = 100f;

        List<Vector3> list_minVec = new List<Vector3>();

        foreach (Vector3 v in Attackable_Tile_List)
        {
            float abs = Mathf.Abs(v.x - MyPosition.x) + Mathf.Abs(v.y - MyPosition.y);
            Debug.Log(abs);
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

    public Vector3 Move(Vector3 minVec)
    {
        Vector3 MyPosition = caster.Location;
        float dis = 100f;
        //가장 가까운 타일 = minVec으로 이동
        Vector3 moveVec = new Vector3();
        for (int i = -1; i <= 1; i += 2)
        {
            Vector3 vec1 = new Vector3(MyPosition.x + i, MyPosition.y, 0);
            if (dis > (vec1 - minVec).sqrMagnitude)
            {
                dis = (vec1 - minVec).sqrMagnitude;
                moveVec = vec1;
            }
            Vector3 vec2 = new Vector3(MyPosition.x, MyPosition.y + i, 0);
            if (dis > (vec2 - minVec).sqrMagnitude)
            {
                dis = (vec2 - minVec).sqrMagnitude;
                moveVec = vec2;
            }
        }
        return moveVec;
    }

    protected Vector2 Unit_Coord_2()
    {
        return Unit_in_Attackable_Range_TileList[Random.Range(0, Unit_in_Attackable_Range_TileList.Count)];
    }

    protected Vector2 Ranged_Unit_Coord_2()
    {
        return Ranged_Unit_in_Attackable_Range_TileList[Random.Range(0, Ranged_Unit_in_Attackable_Range_TileList.Count)];
    }

    protected void Attack_Unit_in_Range()
    {
        if (Is_Ranged_Unit_Exist())
        {
            //원거리 유닛이 있을 경우
            Debug.Log("범위 내 원거리");
            caster.AttackTileClick(Ranged_Unit_Coord());
        }
        else
        {
            //근거리 유닛만 있을 경우
            Debug.Log("범위 내 근거리");
            caster.AttackTileClick(Unit_Coord());
        }
    }

    public virtual void AI_Action()
    {
        //전달받은 범위에서 유닛을 찾는다.
        Find_Unit_in_Attack_Range();

        //찾은 유닛이 있는지 확인하고, 있다면 원거리인지, 근거리인지 확인한다.
        if (Is_Unit_Exist())
        {
            Attack_Unit_in_Range();
        }
        else
        {
            //공격 범위 내에서 찾은 유닛이 없으면 이동하고 공격한다
            Set_Attackable_Tile();

            Search_Attackable_Tile();

            //if (Is_Unit_Exist())
            if (Unit_in_Attackable_Range_TileList.Count > 0)
            {
                //if (Is_Ranged_Unit_Exist())
                if (Ranged_Unit_in_Attackable_Range_TileList.Count > 0)
                {
                    //원거리가 있음
                    //Random.Range(0, Ranged_Unit_in_Attackable_Range_TileList.Count);
                    
                    GameManager.Battle.Field.MoveUnit(caster.Location, Ranged_Unit_Coord_2());
                    Find_Unit_in_Attack_Range();
                    Attack_Unit_in_Range();
                }
                else
                {
                    //근거리만 있음
                    //Random.Range(0, Unit_in_Attackable_Range_TileList.Count);
                    GameManager.Battle.Field.MoveUnit(caster.Location, Unit_Coord_2());
                    Find_Unit_in_Attack_Range();
                    Attack_Unit_in_Range();
                }
            }
            else
            {
                GameManager.Battle.Field.MoveUnit(caster.Location, Move(Search_Near_Enemy()));
                GameManager.Battle.UseNextUnit();
            }
        }
    }
}

public class Common_Unit_AI_Controller : Unit_AI_Controller
{
    public override void AI_Action()
    {
        base.AI_Action();
    }
}