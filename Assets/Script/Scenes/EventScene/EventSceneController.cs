using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSceneController : MonoBehaviour
{
    [SerializeField] GameObject _upgrade;
    [SerializeField] GameObject _stigma;
    [SerializeField] GameObject _harlot;

    private string _sceneName;


    private void Awake()
    {
        _upgrade.SetActive(false);
        _stigma.SetActive(false);
        _harlot.SetActive(false);

        //_sceneName = 
    }

    public void Start()
    {
        if(_sceneName == "Upgrade")
        {
            _upgrade.SetActive(true);
        }
        else if(_sceneName == "Stigma")
        {
            _stigma.SetActive(true);
        }
        else if(_sceneName== "Harlot")
        {
            _harlot.SetActive(true);
        }
    }
}
