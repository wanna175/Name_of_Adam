using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleUnit : Unit
{
    [SerializeField] public bool MyTeam;
    [SerializeField] public RangeType RType;
    [SerializeField] public int FallGauge;
    [SerializeField] public BattleUnitSO BattleUnitSO;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Stat stat;
    [SerializeField] public int MoveDistance;
    [SerializeField] public int ManaCost;
    [SerializeField] public bool Fall;
    [SerializeField] SkillSO skill;

    BattleManager _BattleMNG;
    BattleDataManager _BattleDataMNG;
    CutSceneManager _CutSceneMNG;

    SpriteRenderer _SR;
    Animator _Animator;
    

    #region Location
    [SerializeField] Vector2 _location;
    public Vector2 Location => _location;
    #endregion
    #region HP
    [SerializeField] float _MaxHP, _CurHP;
    public float MaxHP => _MaxHP;
    public float CurHP
    {
        get { return _CurHP; }
        set
        {
            _CurHP = value;

            if (MaxHP < _CurHP)
                _CurHP = MaxHP;
            else if (_CurHP < 0)
                _CurHP = 0;
        }
    }
    #endregion

    public Vector2 _SelectTile = new Vector2(-1, -1);
    public Vector2 SelectTile => _SelectTile;

    // Move인지 Attack인지 확인하기 위한 임시클래스
    bool isMove = true;
    public bool IsMove => isMove;
    private void Awake()
    {
        _BattleMNG = GameManager.BattleMNG;
        _BattleDataMNG = GameManager.BattleMNG.BattleDataMNG;
        _CutSceneMNG = GameManager.CutSceneMNG;

        _SR = GetComponent<SpriteRenderer>();
        _Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _BattleDataMNG.BattleUnitEnter(this);
        //ChangeState(BattleUnitState.Idle);
        //UpdateState();

        // 적군일 경우 x축 뒤집기
        _SR.flipX = (!BattleUnitSO.MyTeam) ? true : false;
        setLocate(Location);
    }

    public void Init()
    {
        _BattleDataMNG.BattleUnitEnter(this);

        //ChangeState(BattleUnitState.Idle);
        //UpdateState();
        // 적군일 경우 x축 뒤집기
        _SR.flipX = (!BattleUnitSO.MyTeam) ? true : false;
        setLocate(Location);
    }

    // 이동가능한 범위를 가져온다.
    public List<Vector2> GetCanMoveRange()
    {
        List<Vector2> vecList = new List<Vector2>();
        vecList.Add(new Vector2(0, 0));
        
        for (int i = 1; i <= BattleUnitSO.MoveDistance; i++)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                vecList.Add(new Vector2(i * j, 0));
                vecList.Add(new Vector2(0, i * j));
            }
        }

        return vecList;
    }


    public void Attack_OnAttack(List<BattleUnit> _HitUnits)
    {
        _CutSceneMNG.BattleCutScene(this, _HitUnits);
    }
    
    public void Hit_GetDamage(float DMG)
    {
        CurHP -= DMG;

        Debug.Log("DMG : " + DMG + ", CurHP ; " + CurHP);

        if (MaxHP <= CurHP)  // 현재 체력이 최대 체력을 넘겼을 시
            CurHP = MaxHP;
        else if (CurHP <= 0) // 유닛 사망 시
            UnitDestroy();
    }
    

    //오브젝트 생성 시, 최초 위치 설정
    public void setLocate(Vector2 coord)
    {
        _location = coord;
        _BattleMNG.SetUnit(this, coord);
    }
    
    void UnitDestroy()
    {
        _BattleDataMNG.BattleUnitExit(this);
        _BattleMNG.BattleOrderRemove(this);
        Destroy(gameObject);
    }

    public void SetPosition(Vector3 dest)
    {
        transform.position = dest;
    }


    public void TileSelected(Vector2 coord)
    {
        if (isMove)
            MoveTileClick(coord);
        else
            AttackTileClick(coord);
    }

    void MoveTileClick(Vector2 coord)
    {
        coord -= Location;

        // 이동범위 밖을 선택했다면 다시 선택하기
        if (!GetCanMoveRange().Contains(coord))
        {
            _BattleMNG.SetTileColor(Color.yellow);
            return;
        }

        _BattleMNG.MoveLotate(this, coord);
        //ChangeState(BattleUnitState.AttackWait);
        //UpdateState();

        isMove = false;
        return;
    }

    void AttackTileClick(Vector2 coord)
    {
        Vector2 dump = coord - Location;

        // 공격범위 밖을 선택했으면 다시 선택하기
        if (!BattleUnitSO.GetRange().Contains(dump))
        {
            _BattleMNG.SetTileColor(Color.yellow);
            return;
        }

        _SelectTile = coord;

        if (_SelectTile == Location)
            _BattleMNG.UseNextUnit();
        else
            BattleUnitSO.use(this);

        isMove = true;
        return;
    }

    public Stat GetStat(bool buff = true)
    {
        Stat stat = BattleUnitSO.stat;

        if (buff == false)
            return stat;

        return stat;
        //return _stigma.Use(stat);
    }

    public bool GetFlipX() => _SR.flipX;
}