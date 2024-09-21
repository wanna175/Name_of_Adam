using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Popup 형태 UI의 부모 클래스입니다.
/// </summary>
public class UI_Popup : UI_Base
{
    //현재 씬 네임을 불러온다.
    protected string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public virtual bool ESCAction() => false;
}
