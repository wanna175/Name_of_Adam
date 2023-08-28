using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitSpawner : UnitSpawner
{
    BattleTestDataContainer data;
    Transform root;

    private void Awake()
    {
        data = GameObject.Find("@BattleManager").GetComponent<BattleTestManager>().dataContainer;
    }

    private void Start()
    {
        root = SetParent();

        foreach (SpawnData data in data.SpawnUnits)
            InitSpawn(data);
    }

    private void InitSpawn(SpawnData spawndata)
    {
        if (BattleManager.Field.TileDict[spawndata.location].UnitExist)
        {
            Debug.Log("해당 타일에 유닛이 존재합니다.");
        }
        else
        {
            GameObject go = GameManager.Resource.Instantiate("BattleUnits/BattleTestUnit", root);
            BattleUnit unit = go.GetComponent<BattleTestUnit>();
            unit.DeckUnit.Data = spawndata.unitData;

            unit.Init();
            unit.UnitSetting(spawndata.location, spawndata.team);
        }
    }

    private Transform SetParent()
    {
        GameObject go = GameObject.Find("Units");
        if (go == null)
            go = new GameObject("Units");
        return go.transform;
    }
}
