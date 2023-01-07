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
    #region
    private ManaManager _ManaMNG;
    public ManaManager ManaMNG => _ManaMNG;
    #endregion

    public void Init()
    {
        _FieldMNG = new FieldManager();
        _BattleUnitManager = new BattleUnitManager();
        _ManaMNG = new ManaManager();
    }
}
