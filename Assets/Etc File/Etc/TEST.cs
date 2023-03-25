using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    [SerializeField] DeckUnit u;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("CLICK");
        UnitSpawner us = GameManager.Battle.GetComponent<UnitSpawner>();

        //us.newSpawn(u, new Vector2(0, 0));
        us.DeckSpawn(u, new Vector2(0, 0));

    }
}
