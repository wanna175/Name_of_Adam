using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSceneController : MonoBehaviour
{
    [SerializeField] private GameObject _baptism;
    [SerializeField] private GameObject _stigmata;
    [SerializeField] private GameObject _sacrifice;

    private StageName _stageName;

    private void Awake()
    {
        _stageName = GameManager.Data.Map.GetCurrentStage().Name;

        _baptism.SetActive(_stageName == StageName.Baptism);
        _stigmata.SetActive(_stageName == StageName.Stigmata);
        _sacrifice.SetActive(_stageName == StageName.Sacrifice);
    }
}
