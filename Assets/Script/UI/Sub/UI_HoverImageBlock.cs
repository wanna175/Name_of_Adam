using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_HoverImageBlock : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image Image;
    private string _text;

    public void Set(Sprite image, string text)
    {
        Image.sprite = image;
        _text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.UI.ShowHover<UI_TextHover>().SetText(_text, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.UI.CloseHover();
    }
}
