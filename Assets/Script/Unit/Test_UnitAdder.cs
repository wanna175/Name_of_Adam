using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_UnitAdder : MonoBehaviour
{
    [SerializeField] List<BattleUnitSO> unit;

    void OnMouseDown()
    {
        foreach (BattleUnitSO b in unit)
        {
            DeckUnit d1 = new DeckUnit();
             d1.SetUnitSO(b);
             GameManager.BattleMNG.BattleDataMNG.AddDeckUnit(d1);
        }

        GameManager.UIMNG.Hands.TEST();
    }
}