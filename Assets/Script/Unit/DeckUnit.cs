using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckUnit
{
    private BattleUnitSO _unitSO = null;

    public BattleUnitSO GetUnitSO()
    {
        return _unitSO;
    }

    public void SetUnitSO(BattleUnitSO so)
    {
        _unitSO = so;
    }
}
