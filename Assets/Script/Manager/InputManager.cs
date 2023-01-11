using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //UI들의 상호작용 관련 변수 관리
    [SerializeField] Hands hands;
    
    #region Hands
    public int ClickedHand = 0;
    public DeckUnit ClickedUnit = null;

    public void SetHand(int handIndex, DeckUnit unit)
    {
        ClickedHand = handIndex;
        ClickedUnit = unit;
    }

    public void DeleteHand(int handIndex)
    {
        hands.RemoveHand(handIndex);
    }

    public void ClearHand()
    {
        ClickedHand = 0;
        ClickedUnit = null;
    }

    #endregion
}