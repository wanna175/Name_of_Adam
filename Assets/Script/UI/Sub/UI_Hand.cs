using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Hand : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private UI_UnitCard _unitCard;

    private DeckUnit _handUnit = null;
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
        _unitCard.Set(_handUnit.Data.Image, _handUnit.Data.Name, _handUnit.Stat.ManaCost.ToString());
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

    public void ChangeSelectState(bool b)
    {
        IsSelected = b;
        _highlight.SetActive(b);
    }
}
