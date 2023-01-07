using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma : MonoBehaviour
{
    [SerializeField] List<StigmaSO> _stigmaList = new List<StigmaSO>();
    public List<StigmaSO> StigmaList => _stigmaList;
    public int Count => StigmaList.Count;

    public void Add(StigmaSO stigma)
    {
        if (Count <= 3)
            StigmaList.Add(stigma);
        else
            Debug.Log("Full Stigma");
    }

    public Stat Use(Stat stat)
    {
        foreach (StigmaSO buff in StigmaList)
            stat = buff.Use(stat);
        return stat;
    }
}
