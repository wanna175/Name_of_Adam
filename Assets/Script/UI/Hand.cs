using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] int handIndex;

    SpriteRenderer SR;

    public Character HandChar = null;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    public void SetCharacter(Character ch)
    {
        Debug.Log("Hand " + handIndex + " set");
        HandChar = ch;
        if (HandChar != null)
        {
            GetComponent<Renderer>().enabled = true;
            SR.sprite = HandChar.characterSO.sprite;
        }

    }

    public Character DelCharacter()
    {
        Debug.Log("Hand " + handIndex + " clear");
        Character returnChar = HandChar;
        HandChar = null;
        
        GetComponent<Renderer>().enabled = false;
        
        return returnChar;
    }

    void OnMouseDown() 
    {
        Debug.Log("Hand: " + handIndex);
        GameManager.Instance.InputMNG.setHand(handIndex, HandChar);
    }
}
