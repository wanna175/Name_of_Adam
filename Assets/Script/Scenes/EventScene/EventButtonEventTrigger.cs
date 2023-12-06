using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventButtonEventTrigger : EventTrigger
{
    Action<GameObject> PointerClick;

    public void Init(EventController controller)
    {
        PointerClick += controller.ButtonClick;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        PointerClick(gameObject);
    }
}
