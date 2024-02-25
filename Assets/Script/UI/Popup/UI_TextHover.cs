using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_TextHover : UI_Hover
{
    [SerializeField] public TextMeshProUGUI _text;
    [SerializeField] public GameObject _block;

    public void SetText(string text, Vector2 position)
    {
        _text.text = text;

        float posX;
        float posY;
        float width = GameManager.OutGameData.GetResolution().width;

        if (position.x > width - 175)
            posX = width - 175;
        else
            posX = position.x;

        if (position.y < 160)
            posY = 160;
        else
            posY = position.y;

        _block.GetComponent<RectTransform>().anchoredPosition = new(posX, posY);
    }
}
