using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckUnit : Unit
{
    private UnitData _unitSO = null;

    public UnitData GetUnitSO()
    {
        return _unitSO;
    }

    public void SetUnitSO(UnitData so)
    {
        _unitSO = so;
    }
}
