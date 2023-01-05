using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] GameObject TilePrefabs;

    const int MaxX = 8;
    const int MaxY = 3;

    public Tile[,] TileArray = new Tile[MaxY, MaxX];

    private void Awake()
    {
        GameManager.Instance.DataMNG.FieldSet(transform, TilePrefabs);
    }
}
