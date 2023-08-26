using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnData
{
    public UnitDataSO unitData;
    public Vector2 location;
    public Team team;
}

// 핸드 -> 필드 위에 생성
// 데이터 -> 필드 생성
public class UnitSpawner : MonoBehaviour
{
    private Transform parent;

    // BattleScene에서 시작했을 때 생성하는 유닛들(디버그용)
    [SerializeField] List<SpawnData> TestSpawnUnit;

    private void Awake()
    {
        parent = SetParent();
    }
    private void Start()
    {
        // BattleScene에서 시작했을 때 유닛 생성(디버그용)
        foreach (SpawnData data in TestSpawnUnit)
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
            GameObject go = GameManager.Resource.Instantiate("BattleUnits/BattleUnit", parent);
            BattleUnit unit = go.GetComponent<BattleUnit>();
            unit.DeckUnit.Data = spawndata.unitData;

            unit.Init();
            unit.UnitSetting(spawndata.location, spawndata.team);
        }
    }

    public BattleUnit DeckSpawn(DeckUnit deckUnit, Vector2 location)
    {
        GameObject go = GameManager.Resource.Instantiate("BattleUnits/BattleUnit", parent);
        BattleUnit unit = go.GetComponent<BattleUnit>();
        unit.DeckUnit = deckUnit;

        unit.Init();
        unit.UnitSetting(location, Team.Player);

        return unit;
    }

    public void SpawnInitialUnit()
    {
        if (GameManager.Data.Map.MapObject == null)
            return;

        DataManager _data = GameManager.Data;
        StageData stage = GameManager.Data.Map.GetCurrentStage();
        List<StageUnitData> datas = GameManager.Data.StageDatas[stage.StageLevel].Find(x => x.ID == stage.StageID).Units;

        foreach (StageUnitData data in datas)
        {
            SpawnData sd = new SpawnData();
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/{data.Name}");
            sd.location = data.Location;
            sd.team = Team.Enemy;

            //sd.deckUnit.Data = sd.prefab.GetComponent<BattleUnit>()
            InitSpawn(sd);

            //Spawn(data, data.location);
            //SpawnTest(data, data.Location);
        }
        //foreach (SpawnData data in SpawnMonsters)
        //{
        //    Spawn(data, data.location);
        //}
    }

    private Transform SetParent()
    {
        GameObject go = GameObject.Find("Units");
        if (go == null)
            go = new GameObject("Units");
        return go.transform;
    }
}