using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBackgroundManager : MonoBehaviour
{
    public GameObject PhanuelBackground;
    public GameObject HorusBackground;
    public GameObject DefaultBackground;

    void Start()
    {
        if(GameManager.Data.Map.GetStage(99).Name == StageName.BossBattle)
        {
            if(GameManager.Data.Map.GetStage(99).StageID == 0)
            {
                PhanuelBackground.SetActive(true);
            }
            else
            {
                HorusBackground.SetActive(true);
            }
        }
        else
        {
            DefaultBackground.SetActive(true);
        }
    }
}
