using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnData
{
    public GameObject prefab;
    public Vector2 location;
    public Team team;
    public Passive[] stigmas;
}

// 핸드 -> 필드 위에 생성
// 데이터 -> 필드 생성
public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<SpawnData> SpawnMonsters;

    private Transform parent;

    private void Awake()
    {
        parent = SetParent();
    }

    private void Spawn(SpawnData spawndata, Vector2 location)
    {
        if (BattleManager.Field.TileDict[location].UnitExist)
        {
            Debug.Log("해당 타일에 유닛이 존재합니다.");
        }
        else
        {
            GameObject go = GameObject.Instantiate(spawndata.prefab, parent);
            BattleUnit bu = go.GetComponent<BattleUnit>();

            bu.Init();
            BattleManager.Instance.UnitSetting(bu, location);
        }
    }

    public BattleUnit DeckSpawn(DeckUnit deckUnit, Vector2 location)
    {
        GameObject go = GameManager.Resource.Instantiate("BattleUnits/BattleUnit", parent);
        BattleUnit bu = go.GetComponent<BattleUnit>();

        bu.Data = deckUnit.Data;
        bu.ChangedStat = deckUnit.ChangedStat;

        bu.Skill.Effects = deckUnit.Data.Effects;

        bu.Init();

        BattleUnit spawnedUnit = go.GetComponent<BattleUnit>();
        BattleManager.Instance.UnitSetting(spawnedUnit, location);
        return spawnedUnit;
    }

    public void SpawnInitialUnit()
    {
        List<SpawnData> datas = SceneChanger.SpawnDataList;

        foreach (SpawnData data in datas)
        //foreach (SpawnData data in SpawnMonsters)
        {
            //Debug.Log(data);
            //Debug.Log(data.location);
            Spawn(data, data.location);
        }
    }

    private Transform SetParent()
    {
        GameObject go = GameObject.Find("Units");
        if (go == null)
            go = new GameObject("Units");
        return go.transform;
    }

    // *****
    // 임시임시
    // 팩토리는 다른곳으로 빼는걸로
    //private void BattleUnitFactory(Vector2 coord)
    //{
    //    //범위 외
    //    if (Field.IsPlayerRange(coord) == false || Field.GetUnit(coord) != null)
    //        return;

    //    // ----------------변경 예정------------------------
    //    Unit clickedUnit = _hands.ClickedUnit;
    //    if (clickedUnit == null)
    //        return;

    //    _mana.ChangeMana(-1 * clickedUnit.Data.ManaCost);

    //    GameObject BattleUnitPrefab = GameManager.Resource.Instantiate("Units/BaseUnit");
    //    BattleUnit BattleUnit = BattleUnitPrefab.GetComponent<BattleUnit>();

    //    GameObject go = GameObject.Instantiate(data.prefab, parent);
    //    GameManager.Battle.UnitSetting(go.GetComponent<BattleUnit>(), data.location);

    //    BattleUnit.Data = clickedUnit.Data;
    //    UnitSetting(BattleUnit, coord);

    //    Data.BattleUnitAdd(BattleUnit);
    //    // ------------------------------------------------
    //}


}
