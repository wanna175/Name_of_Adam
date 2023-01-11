using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour
{
    [SerializeField] int handIndex;

    private SpriteRenderer SR;

    private DeckUnit HandUnit = null;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    public void SetHandDeckUnit(DeckUnit unit)
    {
        Debug.Log("Hand " + handIndex + " set");
        HandUnit = unit;
        if (HandUnit != null)
        {
            GetComponent<Renderer>().enabled = true;
            SR.sprite = HandUnit.GetUnitSO().sprite;
        }

    }

    public DeckUnit GetHandDeckUnit()
    {
        return HandUnit;
    }

    public DeckUnit RemoveHandDeckUnit()
    {
        Debug.Log("Hand " + handIndex + " clear");
        DeckUnit returnUnit = HandUnit;
        HandUnit = null;
        
        GetComponent<Renderer>().enabled = false;
        
        return returnUnit;
    }

    void OnMouseDown() 
    {
        Debug.Log("Hand: " + handIndex);
        GameManager.Instance.InputMNG.SetHand(handIndex, HandUnit);
    }
}
