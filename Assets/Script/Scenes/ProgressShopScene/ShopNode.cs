using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopNode : MonoBehaviour
{
    public Image NodeImage;
    public Image NodeLine;
    public GameObject Highlighted;
    public int ItemID;

    //private Image NodeLineImage;

    public void Start()
    {
        //NodeImage = GetComponent<Image>();
        //NodeLineImage = NodeLine.GetComponent<Image>();
    }

    public void SetImage()
    {
        if (!GameManager.OutGameData.GetBuyable(ItemID) && GameManager.OutGameData.GetProgressItem(ItemID).IsLock) // 구매 불가능한 노드
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/wlscjreh_icon_03");
        }
        else if (GameManager.OutGameData.GetProgressItem(ItemID).IsUnlocked) // 구매한 노드
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/wlscjreh_icon_01");
            NodeLine.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/line_02");
        }
        else // 구매 가능한 노드
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/wlscjreh_icon_02 lock");
        }

    }
}
