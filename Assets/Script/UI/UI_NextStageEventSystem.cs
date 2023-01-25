using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_NextStageEventSystem : EventTrigger
{
    public override void OnPointerClick(PointerEventData data)
    {
        if(data.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedBox = data.pointerCurrentRaycast.gameObject;

            if (!clickedBox.CompareTag("StageSelectBox"))
                return;
            
            int index = Int32.Parse(clickedBox.name.Split("_")[1]);

            // 입력된 정보를 StageManager로 전송
            GameManager.Instance.StageMNG.StageSelect(index);
        }
    }
}
