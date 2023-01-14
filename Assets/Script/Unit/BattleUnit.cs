using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 필드 위에 올려진 캐릭터의 스크립트
// 스킬 사용, 이동, 데미지와 사망판정을 처리

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

        _BattleDataMNG.BattleUnitMNG.BattleUnitEnter(this);
        _UnitAction.GetMaxHP(GetStat().HP);
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
}
