using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressShopSceneController : MonoBehaviour
{
    [SerializeField] GameObject UI_IncarnaShop;
    [SerializeField] GameObject UI_UpgradeShop;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        UI_IncarnaShop.SetActive(false);
        UI_UpgradeShop.SetActive(true);
    }

    public void UpgradeShopBtn()
    {
        UI_IncarnaShop.SetActive(false);
        UI_UpgradeShop.SetActive(true);
    }

    public void IncarnaBtn()
    {
        UI_IncarnaShop.SetActive(true);
        UI_UpgradeShop.SetActive(false);
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        SceneChanger.SceneChange("MainScene");
    }
}
