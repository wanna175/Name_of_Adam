using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NextStageEventSystem : EventTrigger
{
    public override void OnPointerClick(PointerEventData data)
    {
        if(data.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedBox = data.pointerCurrentRaycast.gameObject;

            if (!clickedBox.CompareTag("StageSelectBox"))
                return;
            
            int index = Int32.Parse(clickedBox.name.Split("_")[1]);

            // 클릭했다는 정보를 어딘가로 줘야함
            // StageManager로 전달하는 것이 베스트
            GameManager.Instance.StageMNG.StageSelect(index);
        }
    }
}
