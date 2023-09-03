using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_Hand : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private UI_UnitCard _unitCard;
    [SerializeField] private GameObject _hand;
    [SerializeField] private Image _unitImage;
    private DeckUnit _handUnit = null;
    private UI_Hands _hands;

    private UI_Info _hoverInfo;
    private UI_Info _selectInfo;

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

    public void SetUnitInfo()
    {
        Debug.Log(_handUnit.Data.Name);
        _unitImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/Units/Unit_Portrait/" + _handUnit.Data.Name + "_Å¸¶ô");
        _unitCard.SetHand(_unitImage.sprite, _handUnit.DeckUnitTotalStat.ManaCost.ToString());
    }

    public DeckUnit GetUnit()
    {
        return _handUnit;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
        _hand.transform.localScale = new Vector3 (1.3f, 1.3f, 1f);
        _hoverInfo = BattleManager.BattleUI.ShowInfo();
        _hoverInfo.SetInfo(_handUnit, Team.Player);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleManager.BattleUI.CloseInfo(_hoverInfo);
        _hand.transform.localScale = new Vector3(1f, 1f, 1f);
        if (IsSelected)
            return;
        _highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _hands.OnClickHand(this);
        }
    }

    public void ChangeSelectState(bool b)
    {
        IsSelected = b;
        _highlight.SetActive(b);

        /*
        if (IsSelected)
        {
            _selectInfo = BattleManager.BattleUI.ShowInfo();
            _selectInfo.SetInfo(_handUnit, Team.Player);
        }
        else
        {
            BattleManager.BattleUI.CloseInfo(_selectInfo);
        }
        */
    }
}
