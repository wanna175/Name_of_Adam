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
        float ratio = 1920f / Screen.width;

        if (position.x > 1920 - 175 / ratio)
            posX = 1920 - 175 / ratio;
        else
            posX = position.x;

        if (position.y < 160 / ratio)
            posY = 160 / ratio;
        else
            posY = position.y;

        posX *= ratio;
        posY *= ratio;

        _block.GetComponent<RectTransform>().anchoredPosition = new(posX, posY);
    }
}
