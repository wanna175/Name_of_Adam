using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Card : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private DeckUnit _cardUnit = null;
    [SerializeField] private GameObject _highlight;
    public bool IsSelected = false;
    
    private void Start()
    {
        _highlight.SetActive(false);
    }

    public void SetUnit(DeckUnit unit)
    {
        _cardUnit = unit;
        SetCardInfo();
    }

    private void SetCardInfo()
    {
        // UI가 완성된 후에 디테일한 요소 추가
        GetComponent<Image>().sprite = _cardUnit.Data.Image;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(_cardUnit);
        ui.init();
    }
}
