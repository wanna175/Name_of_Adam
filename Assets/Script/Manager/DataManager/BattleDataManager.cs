using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDataManager
{
    private FieldDataManager _FieldDataMNG;
    public FieldDataManager FieldDataMNG => _FieldDataMNG;

    public void Init()
    {
        _FieldDataMNG = new FieldDataManager();
    }
}
