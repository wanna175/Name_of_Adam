using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Buff : UI_Base, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _buffText;
    [SerializeField] private Image _buffImage;

    private UI_HPBar _hpbar;
    public Buff BuffInBlock;

    public void Set(UI_HPBar hpbar, Buff buff)
    {
        _hpbar = hpbar;
        BuffInBlock = buff;
        if (_buffImage.sprite != null)
            _buffImage.sprite = BuffInBlock.Sprite;

        RefreshBuffDisplayNumber();
    }

    public void RefreshBuffDisplayNumber()
    {
        if (BuffInBlock.GetBuffDisplayNumber() == -1)
        {
            _buffText.text = "";
        }
        else
        {
            _buffText.text = BuffInBlock.GetBuffDisplayNumber().ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hpbar.BuffHoverIn();
        Vector2 hoverPosition = new(eventData.position.x, eventData.position.y);
        GameManager.UI.ShowHover<UI_TextHover>().SetText(BuffInBlock.GetDescription(), hoverPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hpbar.BuffHoverOut();
        GameManager.UI.CloseHover();
    }
}
