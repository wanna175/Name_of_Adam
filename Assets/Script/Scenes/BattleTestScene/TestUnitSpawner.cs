using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitSpawner : UnitSpawner
{
    private Queue<GameObject> UnitQueue;
    BattleTestDataContainer data;
    Transform root;


    private void Start()
    {
        data = GameObject.Find("@BattleManager").GetComponent<BattleTestManager>().dataContainer;
        UnitQueue = new Queue<GameObject>();

        root = SetParent();

        for (int i = 0; i < 9; i++)
            CreateUnit();

        BattleManager.Instance.PlayAfterCoroutine(() => {
            foreach (SpawnData data in data.SpawnUnits)
                InitSpawn(data);
        }, 1);
    }

    private void InitSpawn(SpawnData spawndata)
    {
        if (BattleManager.Field.TileDict[spawndata.location].UnitExist)
        {
            Debug.Log("해당 타일에 유닛이 존재합니다.");
        }
        else
        {
            GameObject go = GetUnit();
            BattleUnit unit = go.GetComponent<BattleTestUnit>();
            unit.DeckUnit.Data = spawndata.unitData;

            unit.Init();
            unit.UnitSetting(spawndata.location, spawndata.team);
            GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/VisualEffect/UnitSpawnEffect", unit.transform.position);
        }
    }

    private void CreateUnit()
    {
        GameObject go = GameManager.Resource.Instantiate("BattleUnits/BattleTestUnit", root);
        go.SetActive(false);

        UnitQueue.Enqueue(go);
    }

    private GameObject GetUnit()
    {
        GameObject go;

        if (!UnitQueue.TryDequeue(out go))
        {
            CreateUnit();
            go = UnitQueue.Dequeue();
        }

        go.SetActive(true);
        return go;
    }

    public void RrestoreUnit(GameObject go)
    {
        UnitQueue.Enqueue(go);
        go.SetActive(false);
    }

    private Transform SetParent()
    {
        GameObject go = GameObject.Find("Units");
        if (go == null)
            go = new GameObject("Units");
        return go.transform;
    }
}
