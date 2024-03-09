using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ProgressShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI info_name;
    [SerializeField] private TextMeshProUGUI info_description;
    [SerializeField] private TextMeshProUGUI active_info_cost;
    [SerializeField] private TextMeshProUGUI disabled_info_cost;
    [SerializeField] private GameObject activeBtn;
    [SerializeField] private GameObject disabledBtn;
    [SerializeField] private TMP_Text ProgressCoin;
    private int selectedID;

    public List<ShopNode> ShopNodes;
    public GameObject ItemInfo;
    

    public void Start()
    {
        selectedID = 0;
        ProgressCoin.text = GameManager.OutGameData.GetProgressCoin().ToString();
        SetNodeImage();
    }

    public void OnClickShopNode(int id)
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        ShopNode foundNode = ShopNodes.FirstOrDefault(node => node.ItemID == selectedID);
        ShopNode newfoundNode = ShopNodes.FirstOrDefault(node => node.ItemID == id);

        if (selectedID == id && ItemInfo.activeSelf == true)
        {
            newfoundNode.Highlighted.SetActive(false);
            ItemInfo.SetActive(false);
        }
        else if (selectedID == id && ItemInfo.activeSelf == false)
        {
            newfoundNode.Highlighted.SetActive(true);
            ItemInfo.SetActive(true);
        }
        else
        {
            foundNode.Highlighted.SetActive(false);
            newfoundNode.Highlighted.SetActive(true);

            selectedID = id;

            ItemInfo.SetActive(true);

            info_name.text = GameManager.Locale.GetLocalizedProgress(GameManager.OutGameData.GetProgressItem(id).Name);
            info_description.text = GameManager.Locale.GetLocalizedProgress(GameManager.OutGameData.GetProgressItem(id).Description).Replace("\\n", "\n");

            if (!GameManager.OutGameData.GetBuyable(id) && !GameManager.OutGameData.GetProgressItem(id).IsLock)
            {
                ChangeBtnImage(false);
                disabled_info_cost.text = GameManager.Locale.GetLocalizedProgress("Purchased");
            }
            else if (!GameManager.OutGameData.GetBuyable(id) && GameManager.OutGameData.GetProgressItem(id).IsLock)
            {
                ChangeBtnImage(false);
                disabled_info_cost.text = GameManager.OutGameData.GetProgressItem(id).Cost.ToString();

            }
            else if (GameManager.OutGameData.GetProgressCoin() < GameManager.OutGameData.GetProgressItem(id).Cost)
            {
                ChangeBtnImage(false);
                disabled_info_cost.text = GameManager.OutGameData.GetProgressItem(id).Cost.ToString();
                disabled_info_cost.color = Color.gray;
            }
            else
            {
                ChangeBtnImage(true);
                active_info_cost.text = GameManager.OutGameData.GetProgressItem(id).Cost.ToString();
            }
        }
    }

    public void OnBuyBtnClick()
    {
        if (!GameManager.OutGameData.GetBuyable(selectedID) || GameManager.OutGameData.GetProgressCoin() < GameManager.OutGameData.GetProgressItem(selectedID).Cost)
        {
            return;
        }

        GameManager.Sound.Play("UI/ButtonSFX/UnlockSFX");
        GameManager.OutGameData.BuyProgressItem(selectedID);
        ProgressCoin.text = GameManager.OutGameData.GetProgressCoin().ToString();
        ChangeBtnImage(false);
        disabled_info_cost.text = GameManager.Locale.GetLocalizedProgress("Purchased");
        SetNodeImage();
    }

    public void ChangeBtnImage(bool isActive)
    {
        if (isActive)
        {
            activeBtn.SetActive(true);
            disabledBtn.SetActive(false);
        }
        else
        {
            activeBtn.SetActive(false);
            disabledBtn.SetActive(true);
        }
    }

    public void OnDisabledBtnClick()
    {
        GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
    }

    public void OnMenuBtnClick()
    {
        ShopNode foundNode = ShopNodes.FirstOrDefault(node => node.ItemID == selectedID);
        foundNode.Highlighted.SetActive(false);
    }

    public void SetNodeImage()
    {
        foreach(ShopNode node in ShopNodes)
        {
            node.SetImage();
        }
    }
}
