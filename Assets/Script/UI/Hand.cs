using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] int handIndex;

    SpriteRenderer SR;

    public BattleUnit HandChar = null;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    public void SetCharacter(BattleUnit ch)
    {
        Debug.Log("Hand " + handIndex + " set");
        HandChar = ch;
        if (HandChar != null)
        {
            GetComponent<Renderer>().enabled = true;
            SR.sprite = HandChar.characterSO.sprite;
        }

    }

    public BattleUnit DelCharacter()
    {
        Debug.Log("Hand " + handIndex + " clear");
        BattleUnit returnChar = HandChar;
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
