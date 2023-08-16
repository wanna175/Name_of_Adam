using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : UI_Popup
{
    [SerializeField] private GameObject _Tutorial1;
    [SerializeField] private GameObject _Tutorial2;
    [SerializeField] private GameObject _Tutorial3;
    [SerializeField] private GameObject _Tutorial4;
    [SerializeField] private GameObject _Tutorial5;
    [SerializeField] private GameObject _Tutorial6;
    [SerializeField] private GameObject _Tutorial7;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { UIToggle(_Tutorial1); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { UIToggle(_Tutorial2); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { UIToggle(_Tutorial3); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { UIToggle(_Tutorial4); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { UIToggle(_Tutorial5); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { UIToggle(_Tutorial6); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { UIToggle(_Tutorial7); }
    }

    public void UIToggle(GameObject go)
    {
        if(go.activeSelf == false)
        {
            go.SetActive(true);
        }
        else
        {
            go.SetActive(false);
        }
    }
    public void QuitTutorialUI() => GameManager.UI.ClosePopup();
}
