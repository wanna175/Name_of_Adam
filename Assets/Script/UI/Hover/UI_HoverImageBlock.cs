using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_HoverImageBlock : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image Image;
    private string _text;
    private bool _isEnable;

    public void Set(Sprite image, string text)
    {
        Image.sprite = image;
        _text = text;
    }

    public void EnableUI(bool isEnable)
    {
        _isEnable = isEnable;
        if (_isEnable) 
            Image.color = new Color(1f, 1f, 1f, 1f);
        else
            Image.color = new Color(1f, 1f, 1f, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isEnable)
            GameManager.UI.ShowHover<UI_TextHover>().SetText(_text, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isEnable)
            GameManager.UI.CloseHover();
    }
}
