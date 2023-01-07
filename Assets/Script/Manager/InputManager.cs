using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //UI들의 상호작용 관련 변수 관리
    [SerializeField] Deck deck;
    
    #region Deck
    public int ClickedHand = 0;
    public BattleUnit ClickedChar = null;

    public void setHand(int handIndex, BattleUnit ch)
    {
        ClickedHand = handIndex;
        ClickedChar = ch;
    }

    public void DeleteHand(int handIndex)
    {
        deck.HandDel(handIndex);
    }

    public void ClearHand()
    {
        ClickedHand = 0;
        ClickedChar = null;
    }

    #endregion
}
