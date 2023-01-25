using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum BattleUnitState
{
    Idle,
    Move,
    Attack
}

public class BattleUnit : MonoBehaviour
{
    #region UnitRenderer
    BattleUnitRenderer _UnitRenderer;
    public BattleUnitRenderer UnitRenderer => _UnitRenderer;
    #endregion
    #region UnitAction
    BattleUnitAction _UnitAction;
    public BattleUnitAction UnitAction => _UnitAction;
    #endregion
    #region UnitMove
    BattleUnitMove _UnitMove;
    public BattleUnitMove UnitMove => _UnitMove;
    #endregion

    // 렌더링 관련 정보 및 관리는 UnitRenderer에서
    // 유닛의 공격, 피격, 타락에 대한 관리는 UnitAction에서
    // 유닛의 위치와 이동에 대한 정보 관리는 UnitMove에서

    BattleDataManager _BattleDataMNG;

    [SerializeField] public BattleUnitSO BattleUnitSO;
    Stigma _stigma;
    
    public Vector2 _SelectTile = new Vector2(-1, -1);
    public Vector2 SelectTile => _SelectTile;

    BattleUnitState state;

    private void Awake()
    {
        _UnitRenderer = GetComponent<BattleUnitRenderer>();
        _UnitAction = GetComponent<BattleUnitAction>();
        _UnitMove = GetComponent<BattleUnitMove>();
        _stigma = GetComponent<Stigma>();
    }

    private void Start()
    {
        _BattleDataMNG = GameManager.Instance.BattleMNG.BattleDataMNG;

        _BattleDataMNG.BattleUnitEnter(this);
        _UnitAction.GetMaxHP(GetStat().HP);

        state = BattleUnitState.Idle;
    }
    
    // 스킬 사용
    public void use()
    {
        BattleUnitSO.use(this);
    }
    
    public Stat GetStat(bool buff = true)
    {
        Stat stat = BattleUnitSO.stat;

        if (buff == false)
            return stat;

        return _stigma.Use(stat);
    }

    public void TileSelected(int x, int y) => _SelectTile = new Vector2(x, y);

    public int GetSpeed() => BattleUnitSO.stat.SPD;








    
    // 대기중인 상태, 아무것도 안하고있으면 이곳에 들어옴
    #region Idle

    void Idle_Start()
    {
        state = BattleUnitState.Idle;
    }

    #endregion

    #region Move

    void Move_Start()
    {
        state = BattleUnitState.Move;
    }

    #endregion

    #region Attack

    void Attack_Start()
    {
        state = BattleUnitState.Attack;
    }

    #endregion
}