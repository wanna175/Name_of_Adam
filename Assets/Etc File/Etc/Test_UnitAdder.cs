using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_UnitAdder : MonoBehaviour
{
    [SerializeField] List<Unit> unit;

    void OnMouseDown()
    {
        foreach (Unit b in unit)
        {
            Debug.Log("ADD");
            Unit d1 = new Unit();
            //d1.SetUnitSO(unit[0].Data); 
            GameManager.Battle.Data.AddUnit(d1);
        }
    }
}