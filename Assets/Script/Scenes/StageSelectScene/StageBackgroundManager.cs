using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject _yohrnBackground;
    [SerializeField] private GameObject _saviorBackground;
    [SerializeField] private GameObject _phanuelBackground;
    [SerializeField] private GameObject _defaultBackground;

    void Start()
    {
        if (GameManager.Data.Map.GetStage(99).Name == StageName.BossBattle)
        {
            if (GameManager.Data.Map.GetStage(99).StageID == 0)
            {
                _phanuelBackground.SetActive(true);
            }
            else if (GameManager.Data.Map.GetStage(99).StageID == 1)
            {
                _saviorBackground.SetActive(true);
            }
            else if (GameManager.Data.Map.GetStage(99).StageID == 2)
            {
                _yohrnBackground.SetActive(true);
            }
        }
        else
        {
            _defaultBackground.SetActive(true);
        }
    }
}
