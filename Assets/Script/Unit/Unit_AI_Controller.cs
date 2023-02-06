using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AI_Controller : MonoBehaviour
{
    protected BattleDataManager _BattleDataMNG;

    protected Field _field;

    protected List<Vector2> FindTileList = new List<Vector2>();
    protected List<Vector2> RangedVectorList = new List<Vector2>();
    protected List<Vector2> AttackRangeList = new List<Vector2>();
    protected SortedSet<Vector3> AttackTileSet = new SortedSet<Vector3>();

    protected BattleUnit caster = new BattleUnit();


    void Awake()
    {
        _BattleDataMNG = GameManager.Battle.Data;
        _field = GameManager.Battle.Field;
    }

    public virtual void AI_Action()
    {
        
    }

    public void SetCaster(BattleUnit unit)
    {
        caster = unit;
    }
    #region AISet
    protected void SetFindTileList()
    {
        //현재 위치에서 공격범위 내의 유닛을 찾는다.

        FindTileList.Clear();
        RangedVectorList.Clear();


        foreach (Vector2 arl in caster.GetRange())
        {
            Vector2 vector = caster.Location;

            if (_field.IsInRange(vector))
            {
                Vector2 vec = vector;
                if (_field.TileDict[vec].IsOnTile)
                {
                    FindTileList.Add(vec);
                    // 만약 if 한번 더 넣어도 되면 여기서 원거리 판별
                }
            }
        }

        foreach (Vector2 ftl in FindTileList)
        {
            if (_field.TileDict[ftl].Unit.Data.BehaviorType == BehaviorType.원거리)
            {
                RangedVectorList.Add(ftl);
            }
        }

        // 유닛 내용은 FindTileList에 저장 및 원거리 리스트 저장
    }

    protected bool IsUnitExist()
    {
        // 유닛이 범위내에 있는지 확인
        return FindTileList.Count > 0;
    }

    protected bool IsRangedUnitExist()
    {
        return RangedVectorList.Count > 0;
    }

    protected void SetDistance()
    {
        //모든 공격 타일을 AttackTileSet에 저장한다. X, Y는 좌표, Z는 원거리/근거리 유무
        foreach (BattleUnit unit in _BattleDataMNG.BattleUnitList)
        {
            if (unit.Team == Team.Player)
            {
                foreach (Vector2 arl in caster.GetRange())
                {
                    Vector3 vector = unit.Location - arl;
                    if (unit.Data.BehaviorType == BehaviorType.원거리)
                        vector.z = 0f;//원거리면 0
                    else
                        vector.z = 0.1f;//근거리면 0.1


                    AttackTileSet.Add(vector);
                }
            }
        }
    }

    protected void SearchAttackableTile()
    {
        //유닛을 때릴 수 있는 타일이 이동 범위 내에 있는 지 확인한다.
        //단 위, 아래, 왼, 오른쪽만 이동 가능하다고 가정

        FindTileList.Clear();
        RangedVectorList.Clear();

        for (int i = -1; i <= 1; i += 2)
        {
            for (float j = 0; j <= 0.1f; j += 0.1f)
            {
                Vector3 vec1 = new Vector3(caster.Location.x + i, caster.Location.y, j);
                if (AttackTileSet.Contains(vec1))
                {
                    FindTileList.Add(vec1);
                }

                Vector3 vec2 = new Vector3(caster.Location.x, caster.Location.y + i, j);
                if (AttackTileSet.Contains(vec2))
                {
                    FindTileList.Add(vec2);
                }
            }
        }

        foreach (Vector3 v in FindTileList)
        {
            if (v.z == 0)
            {
                RangedVectorList.Add(new Vector2(v.x, v.y));
            }
        }

    }

    protected void OrderbyDistance()
    {
        Vector3 MyPosition = caster.Location;

        float dis = 100f;
        Vector3 minVec = new Vector3();

        foreach (Vector3 v in RangedVectorList)
        {
            if (dis > Mathf.Abs(v.x - MyPosition.x) + Mathf.Abs(v.y - MyPosition.y))
            {
                dis = Mathf.Abs(v.x - MyPosition.x) + Mathf.Abs(v.y - MyPosition.y);
                minVec = v;
            }
        }
        //가장 가까운 타일 = minVec으로 이동

        dis = 100f;//재활용
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
    }
    #endregion
}

public class Common_Unit_AI_Controller : Unit_AI_Controller
{
    public override void AI_Action()
    {
        //전달받은 범위에서 유닛을 찾는다.
        SetFindTileList();

        //찾은 유닛이 있는지 확인하고, 있다면 원거리인지, 근거리인지 확인한다.
        if (IsUnitExist())
        {
            if (IsRangedUnitExist())
            {
                //원거리 유닛이 있을 경우
                //Random.Range(0, RangeList.Count);
            }
            else
            {
                //근거리 유닛만 있을 경우
                //Random.Range(0, findUnitList.Count);
            }
        }
        else
        {
            //공격 범위 내에서 찾은 유닛이 없으면 이동하고 공격한다
            SetDistance();
            SearchAttackableTile();
            if (IsUnitExist())
            {
                if (IsRangedUnitExist())
                {
                    //원거리가 있음
                    //Random.Range(0, RangedVectorList.Count);
                }
                else
                {
                    //근거리만 있음
                    //Random.Range(0, FindTileList.Count);
                }
            }
            else
            {
                OrderbyDistance();
                //moveVec으로 이동
            }
        }
    }
}