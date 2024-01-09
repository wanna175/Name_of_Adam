using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ProgressShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI info_name;
    [SerializeField] private TextMeshProUGUI info_description;
    [SerializeField] private TextMeshProUGUI info_cost;
    [SerializeField] private TextMeshProUGUI info_btn_text;
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
        if(selectedID == id && ItemInfo.activeSelf == true)
        {
            ItemInfo.SetActive(false);
        }
        else if (selectedID == id && ItemInfo.activeSelf == false)
        {
            ItemInfo.SetActive(true);
        }
        else
        {
            selectedID = id;

            ItemInfo.SetActive(true);

            info_name.text = GameManager.OutGameData.GetProgressItem(id).Name;
            info_description.text = GameManager.OutGameData.GetProgressItem(id).Description;

            if (!GameManager.OutGameData.GetBuyable(id) && !GameManager.OutGameData.GetProgressItem(id).IsLock)
            {
                info_cost.text = "구매 완료";
            }
            else
            {
                info_cost.text = GameManager.OutGameData.GetProgressItem(id).Cost.ToString();
            }
        }
    }

    public void OnBuyBtnClick()
    {
        if (!GameManager.OutGameData.GetBuyable(selectedID))
        {
            return;
        }

        GameManager.OutGameData.GetProgressItem(selectedID).IsUnlocked = true;
        GameManager.OutGameData.BuyProgressItem(selectedID);
        ProgressCoin.text = GameManager.OutGameData.GetProgressCoin().ToString();
        info_cost.text = "구매 완료";
        SetNodeImage();
    }

    public void SetNodeImage()
    {
        foreach(ShopNode node in ShopNodes)
        {
            node.SetImage();
        }
    }
}
