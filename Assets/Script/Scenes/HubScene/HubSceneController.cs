using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSceneController : MonoBehaviour
{
    void Start()
    {
        Init();
    }

    private void Init()
    {

    }

    public void GameStart()
    {
        SceneChanger.SceneChange("CutScene");
    }

    public void ProgressStore()
    {
        SceneChanger.SceneChange("ProgressScene");
    }

    public void Temple()
    {
        SceneChanger.SceneChange("TempleScene");
    }

    public void Dictionary()
    {
        SceneChanger.SceneChange("DictionaryScene");
    }

    public void Exit()
    {
        
    }

}
