using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIDManager :MonoBehaviour
{
    #region 유닛id를 관리하는 set
    private HashSet<int> UnitID_set = new HashSet<int>();
    private int nextID = 0;
    #endregion

    #region 함수
    public void Init(List<DeckUnit> deckUnits)
    {
        foreach(DeckUnit unit in deckUnits)
        {
            LoadID(unit);
        }
    }
    public void LoadID(DeckUnit unit)
    {
        unit.UnitID =  GetID();
    }
    public int GetID()
    {
        UnitID_set.Add(nextID++);
        return nextID - 1;
    }
    public void resetID()
    {
        UnitID_set.Clear();
        nextID = 0;
    }
    #endregion

}
