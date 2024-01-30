using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_StigmaHover : UI_Hover, IPointerEnterHandler, IPointerExitHandler
{
    private Stigma _stigma;

    private bool _isEnable = false;

    public void SetStigma(Stigma stigma)
    {
        _stigma = stigma;
        SetEnable(true);
    }

    public void SetEnable(bool isEnable)
        => _isEnable = isEnable;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isEnable)
            GameManager.UI.ShowHover<UI_TextHover>().SetText(_stigma.Description, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isEnable)
            GameManager.UI.CloseHover();
    }
}
