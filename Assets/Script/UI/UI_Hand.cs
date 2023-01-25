using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Hand : MonoBehaviour
{
    [SerializeField] int handIndex;

    private Image _Image;
    private DeckUnit HandUnit = null;

    void Start()
    {
        _Image = GetComponent<Image>();
    }

    public void SetHandDeckUnit(DeckUnit unit)
    {
        Debug.Log("Hand " + handIndex + " set");
        HandUnit = unit;
        if (HandUnit != null)
        {
            GetComponent<Image>().enabled = true;
            _Image.sprite = HandUnit.GetUnitSO().sprite;
        }

    }

    public DeckUnit GetHandDeckUnit()
    {
        return HandUnit;
    }

    public bool isHandNull()
    {
        if (HandUnit == null)
            return true;
        else
            return false;
    }

    public DeckUnit RemoveHandDeckUnit()
    {
        DeckUnit returnUnit = HandUnit;
        HandUnit = null;
        
        GetComponent<Image>().enabled = false;
        
        return returnUnit;
    }

    void OnMouseDown() 
    {
        Debug.Log("Hand: " + handIndex);
        if (GameManager.Instance.BattleMNG.BattleDataMNG.CanUseMana(HandUnit.GetUnitSO().ManaCost)){
            GameManager.Instance.UIMNG.Hands.SetHand(handIndex);
        }
        else
        {
            //shake
        }
    }
}
