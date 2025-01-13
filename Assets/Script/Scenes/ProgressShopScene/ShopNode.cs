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
    public GameObject Block;

    //private Image NodeLineImage;

    public void Start()
    {
        //NodeImage = GetComponent<Image>();
        //NodeLineImage = NodeLine.GetComponent<Image>();
        
    }

    public void SetImage()
    {
        if (!GameManager.OutGameData.GetBuyable(ItemID) && !GameManager.OutGameData.IsUnlockedItem(ItemID)) // ���� �Ұ����� ���
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/wlscjreh_icon_02_lock");
            Block.SetActive(true);
        }
        else if (GameManager.OutGameData.IsUnlockedItem(ItemID)) // ������ ���
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/wlscjreh_icon_01");
            NodeLine.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/line_02");
            Block.SetActive(false);
        }
        else // ���� ������ ���
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/ProgressShop/wlscjreh_icon_02_lock");
            Block.SetActive(false);
        }

    }
}
