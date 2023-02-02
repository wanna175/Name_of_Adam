using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PhaseChange : EventTrigger
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button On");
        // 여기에 change 넣기
        
        GameManager.BattleMNG.PhaseUpdate();
    }
}