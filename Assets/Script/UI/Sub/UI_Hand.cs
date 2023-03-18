using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Hand : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private DeckUnit _handUnit = null;
    [SerializeField] private GameObject _highlight;
    private UI_Hands _hands;
    public bool IsSelected = false;
    

    private void Start()
    {
        _highlight.SetActive(false);
    }

    public void SetUnit(UI_Hands hands, DeckUnit unit)
    {
        _hands = hands;
        _handUnit = unit;
        SetUnitInfo();
    }

    private void SetUnitInfo()
    {
        // UI가 완성된 후에 디테일한 요소 추가
        GetComponent<Image>().sprite = _handUnit.Data.Image;
    }

    public DeckUnit GetUnit()
    {
        return _handUnit;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected)
            return;
        _highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _hands.OnClickHand(this);
    }

    public void ChangeSelectState()
    {
        if (IsSelected)
        {
            IsSelected = false;
            _highlight.SetActive(false);
        }
        else
        {
            IsSelected = true;
            _highlight.SetActive(true);
        }
            
    }
}
