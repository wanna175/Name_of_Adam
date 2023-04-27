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
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck");
        /*
        Debug.Log("CLICK");
        UnitSpawner us = BattleManager.Instance.GetComponent<UnitSpawner>();

        //us.newSpawn(u, new Vector2(0, 0));
        us.DeckSpawn(u, new Vector2(0, 0));
        */
    }
}
