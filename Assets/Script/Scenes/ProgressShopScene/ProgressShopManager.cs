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

            info_name.text = GameManager.OutGameData.GetProgressItem(id).Name;
            info_description.text = GameManager.OutGameData.GetProgressItem(id).Description.Replace("\\n", "\n");

            if (!GameManager.OutGameData.GetBuyable(id) && !GameManager.OutGameData.GetProgressItem(id).IsLock)
            {
                ChangeBtnImage(false);
                disabled_info_cost.text = "구매 완료";
            }
            else if (!GameManager.OutGameData.GetBuyable(id) && GameManager.OutGameData.GetProgressItem(id).IsLock)
            {
                ChangeBtnImage(false);
                disabled_info_cost.text = GameManager.OutGameData.GetProgressItem(id).Cost.ToString();

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
        if (!GameManager.OutGameData.GetBuyable(selectedID))
        {
            return;
        }

        GameManager.OutGameData.BuyProgressItem(selectedID);
        ProgressCoin.text = GameManager.OutGameData.GetProgressCoin().ToString();
        ChangeBtnImage(false);
        disabled_info_cost.text = "구매 완료";
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

    public void SetNodeImage()
    {
        foreach(ShopNode node in ShopNodes)
        {
            node.SetImage();
        }
    }
}
