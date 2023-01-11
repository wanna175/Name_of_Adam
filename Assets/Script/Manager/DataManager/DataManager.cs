using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    #region DeckUnitList
    List<DeckUnit> _DeckUnitList = new List<DeckUnit>();
    public List<DeckUnit> DeckUnitList => _DeckUnitList;    
    #endregion
}