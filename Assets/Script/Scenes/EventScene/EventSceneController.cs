using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CUR_EVENT
{
    NONE,
    UPGRADE,
    RELEASE,
    COMPLETE_UPGRADE,
    COMPLETE_RELEASE,
    STIGMA,
    GIVE_STIGMA,
    RECEIVE_STIGMA,
    COMPLETE_STIGMA,
    STIGMA_EXCEPTION,
    HARLOT_RESTORATION,
    COMPLETE_HAELOT,
    HOLL
};
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

        _sceneName = GameManager.Data.Map.GetCurrentStage().Name.ToString();
    }

    public void Start()
    {
        if(_sceneName == "UpgradeStore")
        {
            _upgrade.SetActive(true);
        }
        else if(_sceneName == "StigmaStore")
        {
            _stigma.SetActive(true);
        }
        else if(_sceneName== "Harlot")
        {
            _harlot.SetActive(true);
        }
    }
}
