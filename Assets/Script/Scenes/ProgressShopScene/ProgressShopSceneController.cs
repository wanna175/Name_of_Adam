using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressShopSceneController : MonoBehaviour
{
    [SerializeField] GameObject UI_IncarnaShop;
    [SerializeField] GameObject UI_UpgradeShop;
    [SerializeField] GameObject UI_IncarnaShopBtn;
    [SerializeField] GameObject UI_UpgradeShopBtn;

    public GameObject ItemInfo;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        UI_IncarnaShop.SetActive(false);
        UI_UpgradeShop.SetActive(true);
        UI_IncarnaShopBtn.GetComponent<Image>().enabled = false;
        UI_UpgradeShopBtn.GetComponent<Image>().enabled = true;
    }

    public void UpgradeShopBtn()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        UI_IncarnaShop.SetActive(false);
        UI_UpgradeShop.SetActive(true);
        UI_IncarnaShopBtn.GetComponent<Image>().enabled = false;
        UI_UpgradeShopBtn.GetComponent<Image>().enabled = true;
        ItemInfo.SetActive(false);
    }

    public void IncarnaBtn()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        UI_IncarnaShop.SetActive(true);
        UI_UpgradeShop.SetActive(false);
        UI_IncarnaShopBtn.GetComponent<Image>().enabled = true;
        UI_UpgradeShopBtn.GetComponent<Image>().enabled = false;
        ItemInfo.SetActive(false);
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        SceneChanger.SceneChange("MainScene");
    }
}
