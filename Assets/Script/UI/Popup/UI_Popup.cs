using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Popup 형태 UI의 부모 클래스입니다.
/// </summary>
public class UI_Popup : UI_Base
{
    private string SceneName = null;
    //각 팝업마다 현재 실행중인SceneName을 쓸수 있게 하기위함

    #region 함수
    //현재 씬 네임을 불러온다.
    protected string currentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    #endregion
}
