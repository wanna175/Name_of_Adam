using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActSelectSceneController : MonoBehaviour
{
    private void Start()
    {
        GameManager.Sound.Clear();
    }

    public void ActSelect(int act)
    {
        GameManager.OutGameData.Data.Act = act;

        SceneChanger.SceneChange("DifficultySelectScene");
    }
}
