using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct SpawnData
{
    public GameObject prefab;
    public Vector2 location;
    public Team team;
    public Stigma[] stigmas;
}

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<SpawnData> SpawnMonsters;

    public void Init()
    {
        Debug.Log("Spawner Start");
        foreach(SpawnData data in SpawnMonsters)
        {
            GameObject go = GameObject.Instantiate(data.prefab);
            go.GetComponent<BattleUnit>().Init(data.team, data.location);
            // Stigma 추가
        }
    }
}
