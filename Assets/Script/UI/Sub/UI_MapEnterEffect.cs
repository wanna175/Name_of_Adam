using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UI_MapEnterEffect : MonoBehaviour
{
    public GameObject MapEnterEffect;

    private void Awake()
    {
        if(GameManager.Data.Map.CurrentTileID == 0)
        {
            MapEnterEffect.SetActive(true);
        }
        else
        {
            MapEnterEffect.SetActive(false);
        }
    }
}
