using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageButtonEventTrigger : EventTrigger
{
    Action<GameObject> PointerEnter;
    Action<GameObject> PointerExit;
    Action<GameObject> PointerClick;

    public void Init(ButtonController controller)
    {
        PointerEnter += controller.HoverEnter;
        PointerExit+= controller.HoverExit;
        PointerClick += controller.ButtonClick;
    }

    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //    PointerEnter(gameObject);
    //}

    //public override void OnPointerExit(PointerEventData eventData)
    //{
    //    PointerExit(gameObject);
    //}

    public override void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Sound.Play("UI/ButtonSFX/ButtonClickSFX");
        PointerClick(gameObject);
    }
}
