using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TurnChange : EventTrigger
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button On");
        GameManager.Instance.BattleMNG.PrepareEnd();
        GameManager.Instance.BattleMNG.EngageStart();
    }
}