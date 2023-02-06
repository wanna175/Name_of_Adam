using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Hand : MonoBehaviour
{
    private UI_Hands _Hands;

    private Image _Image;
    private DeckUnit _HandUnit = null;

    void Start()
    {
        _Hands = GameManager.UIMNG.Hands;
        _Image = GetComponent<Image>();
    }

    public void SetHandDeckUnit(DeckUnit unit)
    {
        _HandUnit = unit;
        if (_HandUnit != null)
        {
            GetComponent<Image>().enabled = true;
            _Image.sprite = _HandUnit.GetUnitSO().Image;
        }

    }

    public DeckUnit GetHandDeckUnit()
    {
        return _HandUnit;
    }

    public bool IsHandNull()
    {
        if (_HandUnit == null)
            return true;
        else
            return false;
    }

    public DeckUnit RemoveHandDeckUnit()
    {
        DeckUnit returnUnit = _HandUnit;
        _HandUnit = null;
        
        GetComponent<Image>().enabled = false;
        
        return returnUnit;
    }

    void OnMouseDown() 
    {
        Debug.Log("Hand Click");
        _Hands.OnHandClick(this);
    }
}
