using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Hand : MonoBehaviour
{
    private UI_Hands _hands;

    private Image _Image;
    private Unit _HandUnit = null;

    void Start()
    {
        _hands = GetComponentInParent<UI_Hands>();
        _Image = GetComponent<Image>();

        GetComponentInChildren<Text>().text = Random.Range(0, 10).ToString();
    }

    public void SetHandUnit(Unit unit)
    {
        _HandUnit = unit;
        if (_HandUnit != null)
        {
            GetComponent<Image>().enabled = true;
            _Image.sprite = _HandUnit.Data.Image;
        }

    }

    public Unit GetHandUnit()
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

    public Unit RemoveHandUnit()
    {
        Unit returnUnit = _HandUnit;
        _HandUnit = null;
        
        GetComponent<Image>().enabled = false;
        
        return returnUnit;
    }

    void OnMouseDown() 
    {
        Debug.Log("Hand Click");
        _hands.OnHandClick(this);
    }
}
