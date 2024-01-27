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
    private Transform _parent;
    private Queue<GameObject> _unitQueue;

    // BattleScene에서 시작했을 때 생성하는 유닛들(디버그용)
    [SerializeField] List<SpawnData> TestSpawnUnit;

    private void Awake()
    {
        _parent = SetParent();
        _unitQueue = new Queue<GameObject>();
    }
    private void Start()
    {
        for (int i = 0; i < 9; i++)
            CreateUnit();
    }

    public BattleUnit SpawnDataSpawn(SpawnData spawndata)
    {
        if (BattleManager.Field.TileDict[spawndata.location].UnitExist)
        {
            Debug.Log("해당 타일에 유닛이 존재합니다.");

            return null;
        }
        else
        {
            GameObject go = GetUnit();
            BattleUnit unit = go.GetComponent<BattleUnit>();
            unit.DeckUnit.Data = spawndata.unitData;

            unit.Init(spawndata.team);
            unit.UnitSetting(spawndata.location);
            if (unit.Data.UnitActionType != UnitActionType.UnitAction_None && unit.Data.UnitActionType != UnitActionType.UnitAction_Phanuel)
                GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/VisualEffect/UnitSpawnEffectWhite", unit.transform.position);

            return unit;
        }
    }

    public BattleUnit DeckSpawn(DeckUnit deckUnit, Vector2 location)
    {
        GameObject go = GetUnit();
        BattleUnit unit = go.GetComponent<BattleUnit>();
        unit.DeckUnit = deckUnit;

        unit.Init(Team.Player);

        if (BattleManager.Field.UnitSizeCheck(location, deckUnit))
        {
            unit.UnitSetting(location);
        }
        else
        {
            foreach (Vector2 tile in deckUnit.GetUnitSizeRange())
            {
                if (BattleManager.Field.UnitSizeCheck(location - tile, deckUnit))
                {
                    unit.UnitSetting(location - tile);
                    break;
                }
            }
        }

        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/VisualEffect/UnitSpawnEffect", unit.transform.position);

        return unit;
    }

    public ConnectedUnit ConnectedUnitSpawn(BattleUnit origianlUnit, Vector2 location)
    {
        GameObject go = GameManager.Resource.Instantiate("BattleUnits/ConnectedUnit", _parent);

        ConnectedUnit unit = go.GetComponent<ConnectedUnit>();
        unit.DeckUnit = origianlUnit.DeckUnit;

        unit.SetOriginalUnit(origianlUnit);
        unit.Init(origianlUnit.Team);
        unit.UnitSetting(location, true);

        return unit;
    }

    public void SpawnInitialUnit()
    {
        // TestScene일 경우 예외처리
        if (SceneChanger.GetSceneName() == "BattleTestScene")
            return;

        if (GameManager.Data.Map.MapObject == null)
            return;

        StageData stage = GameManager.Data.Map.GetCurrentStage();
        List<StageUnitData> datas = GameManager.Data.StageDatas[stage.StageLevel].Find(x => x.ID == stage.StageID).Units;

        foreach (StageUnitData data in datas)
        {
            SpawnData sd = new();
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/{data.Name}");
            sd.location = data.Location;
            sd.team = Team.Enemy;

            SpawnDataSpawn(sd);
        }
    }

    private void CreateUnit()
    {
        GameObject go = GameManager.Resource.Instantiate("BattleUnits/BattleUnit", _parent);
        go.SetActive(false);

        _unitQueue.Enqueue(go);
    }

    private GameObject GetUnit()
    {
        GameObject go;
        
        if(!_unitQueue.TryDequeue(out go))
        {
            CreateUnit();
            go = _unitQueue.Dequeue();
        }

        go.SetActive(true);
        return go;
    }

    public void RestoreUnit(GameObject go)
    {
        Destroy(go);
        /*
        _unitQueue.Enqueue(go);
        go.SetActive(false);
        */
    }

    private Transform SetParent()
    {
        GameObject go = GameObject.Find("Units");
        if (go == null)
            go = new GameObject("Units");
        return go.transform;
    }
}