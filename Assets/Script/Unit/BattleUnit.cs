using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleUnitState
{
    Idle,
    Move,
    AttackWait,
    Attack,
    HitWait
}


public class BattleUnit : MonoBehaviour
{
    [SerializeField] public BattleUnitSO BattleUnitSO;

    BattleManager _BattleMNG;
    BattleDataManager _BattleDataMNG;
    CutSceneManager _CutSceneMNG;

    SpriteRenderer _SR;
    Animator _Animator;

    BattleUnitState state;

    #region Loc X, Y
    [SerializeField] int _LocX, _LocY;
    public int LocX => _LocX;
    public int LocY => _LocY;
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

    public event Action<List<Vector2>, BattleUnit, Color> SetTIleColor;

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

        SetState(BattleUnitState.Idle);

        // 적군일 경우 x축 뒤집기
        _SR.flipX = (!BattleUnitSO.MyTeam) ? true : false;
        setLocate(LocX, LocY);
    }

    // 상태 바꾸는 애를 하나로 통합, 애니메이션 바꾸는애도 동일하게
    // switch로 state를 받아서 변환을 시키는 식

    
    public void SetState(BattleUnitState _st)
    {
        state = _st;

        switch (state)
        {
            case BattleUnitState.Idle:
                _Animator.SetBool("isAttack", false);
                _Animator.SetBool("isHit", false);

                break;

            case BattleUnitState.Move:
                GetCanMoveRange();

                break;

            case BattleUnitState.AttackWait:
                //_field.SetTileColor(BattleUnitSO.GetRange(), this, Color.yellow);

                break;

            case BattleUnitState.Attack:
                _Animator.SetBool("isAttack", true);

                break;

            case BattleUnitState.HitWait:
                _Animator.SetBool("isHit", true);

                break;
        }
    }

    #region MoveState

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

    #endregion

    #region AttackState

    public void Attack_OnAttack(List<BattleUnit> _HitUnits)
    {
        _CutSceneMNG.BattleCutScene(this, _HitUnits);
    }
    
    #endregion

    #region HitState
    
    public void Hit_GetDamage(float DMG)
    {
        CurHP -= DMG;

        Debug.Log("DMG : " + DMG + ", CurHP ; " + CurHP);

        if (MaxHP <= CurHP)  // 현재 체력이 최대 체력을 넘겼을 시
            CurHP = MaxHP;
        else if (CurHP <= 0) // 유닛 사망 시
            UnitDestroy();
    }

    #endregion



    #region SetMove

    //오브젝트 생성 시, 최초 위치 설정
    public void setLocate(int x, int y)
    {
        _BattleMNG.SetUnit(this, x, y);
    }

    #endregion

    #region Hit & Destroy

    void UnitDestroy()
    {
        _BattleDataMNG.BattleUnitExit(this);
        _BattleMNG.BattleOrderRemove(this);
        Destroy(gameObject);
    }

    #endregion

    #region Fall

    // 타락 게이지가 늘어나거나 줄어들 때
    public void SetFallGauge(int value)
    {
        int gauge = BattleUnitSO.FallGauge;
        int maxGauge = 3;

        gauge += value;
        if (gauge < 0) gauge = 0;
        else if (gauge >= maxGauge)
        {
            Fall();
            gauge = 0;
        }

        BattleUnitSO.FallGauge = gauge;
    }

    void Fall()
    {
        BattleUnitSO.Fall = true;
        if (!BattleUnitSO.MyTeam) // 적이라면
            BattleUnitSO.MyTeam = true; // 아군으로
        else
            BattleUnitSO.MyTeam = false; // 아군이면 적으로
    }

    #endregion

    public Stat GetStat(bool buff = true)
    {
        Stat stat = BattleUnitSO.stat;

        if (buff == false)
            return stat;

        return stat;
        //return _stigma.Use(stat);
    }

    public void SetPosition(Vector3 dest)
    {
        transform.position = dest;
    }

    public void TileSelected(int x, int y)
    {
        switch (state)
        {
            case BattleUnitState.Move:
                x -= LocX;
                y -= LocY;
            
                //_BattleMNG.MoveLotate(this, x, y);
                SetState(BattleUnitState.AttackWait);

                break;

            case BattleUnitState.AttackWait:
                _SelectTile = new Vector2(x, y);

                if (_SelectTile == new Vector2(LocX, LocY))
                    _BattleMNG.UseNextUnit();
                else
                    BattleUnitSO.use(this);

                break;
        }
    }

    public int GetSpeed() => BattleUnitSO.stat.SPD;

    public bool GetFlipX() => _SR.flipX;
}