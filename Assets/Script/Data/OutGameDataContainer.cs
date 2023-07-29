using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class OutGameDatas
{
    public List<ProgressPoint> ProgressPoint;
    public List<HallUnit> HallUnit;
}

[Serializable]
public class ProgressPoint
{
    public int ID;
    public string Name;
    public int Cost;
    public List<int> Conditions;
    public bool isLock;
}

[Serializable]
public class HallUnit
{
    public int UnitID;
    public string[] Stigmata;
}


public class OutGameDataContainer : MonoBehaviour
{
    [SerializeField] OutGameDatas data;

    public void Init()
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>("Data/OutGameData");
        data = JsonUtility.FromJson<OutGameDatas>(textAsset.text);
    }
}