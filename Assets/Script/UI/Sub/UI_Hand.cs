using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Hand : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private DeckUnit _handUnit = null;
    private GameObject _highlight = null;
    public bool IsSelected = false;
    

    private void Start()
    {
        _highlight = Util.FindChild(gameObject, "Highlight");
        _highlight.SetActive(false);
    }

    public void SetHandUnit(DeckUnit unit)
    {
        _handUnit = unit;
        SetUnitInfo();
    }

    private void SetUnitInfo()
    {
        // UI가 완성된 후에 디테일한 요소 추가
        GetComponent<Image>().sprite = _handUnit.Data.Image;
    }

    public DeckUnit GetHandUnit()
    {
        return _handUnit;
    }

    public bool IsHandNull()
    {
        if (_handUnit == null)
            return true;
        else
            return false;
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
        Debug.Log("Hand Click");
        if (GameManager.Battle.UnitSpawn(_handUnit))
        {
            if (IsSelected)
                IsSelected = false;
            else
                IsSelected = true;
        }
        
        

    }
}
