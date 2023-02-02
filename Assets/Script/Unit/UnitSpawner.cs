using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct SpawnData
{
    public GameObject prefab;
    public Vector2 location;
}

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] List<SpawnData> SpawnMonsters;

    void Start()
    {
        foreach(SpawnData data in SpawnMonsters)
        {
            GameObject newEnemy = GameObject.Instantiate(data.prefab, transform);
            BattleUnit newUnit = newEnemy.GetComponent<BattleUnit>();
            newUnit.setLocate(data.location);
            newUnit.SetTileColor = GameManager.BattleMNG.Field.SetTileColor;
        }
    }
}
