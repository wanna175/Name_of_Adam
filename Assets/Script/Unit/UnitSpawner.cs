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
    private Queue<GameObject> UnitQueue;

    // BattleScene에서 시작했을 때 생성하는 유닛들(디버그용)
    [SerializeField] List<SpawnData> TestSpawnUnit;

    private void Awake()
    {
        parent = SetParent();
        UnitQueue = new Queue<GameObject>();
    }
    private void Start()
    {
        for (int i = 0; i < 9; i++)
            CreateUnit();
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
            BattleUnit unit = go.GetComponent<BattleUnit>();
            unit.DeckUnit.Data = spawndata.unitData;

            unit.Init();
            unit.UnitSetting(spawndata.location, spawndata.team);
            GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/VisualEffect/UnitSpawnEffect", unit.transform.position);
        }
    }

    public BattleUnit DeckSpawn(DeckUnit deckUnit, Vector2 location)
    {
        GameObject go = GetUnit();
        BattleUnit unit = go.GetComponent<BattleUnit>();
        unit.DeckUnit = deckUnit;

        unit.Init();
        unit.UnitSetting(location, Team.Player);

        return unit;
    }

    public void SpawnInitialUnit()
    {
        // TestScene일 경우 예외처리
        if (SceneChanger.GetSceneName() == "BattleTestScene")
            return;

        if (GameManager.Data.Map.MapObject == null)
            return;

        DataManager _data = GameManager.Data;
        StageData stage = GameManager.Data.Map.GetCurrentStage();
        List<StageUnitData> datas = GameManager.Data.StageDatas[stage.StageLevel].Find(x => x.ID == stage.StageID).Units;

        foreach (StageUnitData data in datas)
        {
            SpawnData sd = new();
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/{data.Name}");
            sd.location = data.Location;
            sd.team = Team.Enemy;

            InitSpawn(sd);
        }
    }

    private void CreateUnit()
    {
        GameObject go = GameManager.Resource.Instantiate("BattleUnits/BattleUnit", parent);
        go.SetActive(false);

        UnitQueue.Enqueue(go);
    }

    private GameObject GetUnit()
    {
        GameObject go;
        
        if(!UnitQueue.TryDequeue(out go))
        {
            CreateUnit();
            go = UnitQueue.Dequeue();
        }

        go.SetActive(true);
        return go;
    }

    public void RestoreUnit(GameObject go)
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