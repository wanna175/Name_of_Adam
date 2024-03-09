using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Credit : MonoBehaviour
{
    public GameObject UI_Credit1;
    public GameObject UI_Credit2;

    public void Start()
    {
        init();
    }

    public void init()
    {
        UI_Credit1.SetActive(true);
        UI_Credit2.SetActive(false);
    }

    public void ClickBtnR()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_Credit1.SetActive(false);
        UI_Credit2.SetActive(true);
    }

    public void ClickBtnL()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_Credit1.SetActive(true);
        UI_Credit2.SetActive(false);
    }

    public void ClickQuit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");

        UI_Credit1.SetActive(true);
        UI_Credit2.SetActive(false);

        gameObject.SetActive(false);
    }
}
