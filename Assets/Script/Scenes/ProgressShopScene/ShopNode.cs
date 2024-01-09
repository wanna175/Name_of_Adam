using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopNode : MonoBehaviour
{
    public Image NodeImage;
    public GameObject NodeLine;
    public int ItemID;

    public void Start()
    {
        NodeImage = GetComponent<Image>();
    }

    public void SetImage()
    {
        Color newColor = Color.yellow;

        if (!GameManager.OutGameData.GetBuyable(ItemID) && GameManager.OutGameData.GetProgressItem(ItemID).IsLock) // 구매 불가능한 노드
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Buttons/Cancel_Button");            
        }
        else if (GameManager.OutGameData.GetProgressItem(ItemID).IsUnlocked) // 구매한 노드
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Buttons/settingIcon");
            NodeLine.GetComponent<Image>().color = newColor;
        }
        else // 구매 가능한 노드
        {
            NodeImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Buttons/deckIcon");
        }

    }
}
