using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDataManager
{
    #region FieldMNG
    private FieldManager _FieldMNG;
    public FieldManager FieldMNG => _FieldMNG;
    #endregion
    #region BattleUnitMNG
    private BattleUnitManager _BattleUnitManager;
    public BattleUnitManager BattleUnitMNG => _BattleUnitManager;
    #endregion
    #region ManaMNG
    private ManaManager _ManaMNG;
    public ManaManager ManaMNG => _ManaMNG;
    #endregion

    // 필드와 배틀유닛, 마나의 정보 및 관리는 각 매니저로 분할

    public BattleDataManager()
    {
        _FieldMNG = new FieldManager();
        _BattleUnitManager = new BattleUnitManager();
        _ManaMNG = new ManaManager();
    }
}
