using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stigma : MonoBehaviour
{
    [SerializeField] List<StigmaSO> _stigmaList = new List<StigmaSO>();
    public List<StigmaSO> StigmaList => _stigmaList;

    public void Add(StigmaSO stigma)
    {
        if (StigmaList.Count <= 3)
            StigmaList.Add(stigma);
        else
            Debug.Log("Full Stigma");
    }

    public void Use(Character chara)
    {
        if (StigmaList.Count <= 0)
            return;

        foreach (StigmaSO stigma in StigmaList)
            stigma.Use(chara);
    }
}
