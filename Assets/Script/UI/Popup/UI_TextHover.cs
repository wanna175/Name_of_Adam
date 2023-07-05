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

        Debug.Log(position);

        if (position.x > 1920 - 200)
            posX = 1920 - 200;
        else
            posX = position.x;

        if (position.y < 120)
            posY = 120;
        else
            posY = position.y;

        _block.GetComponent<Transform>().position = new(posX, posY);
    }
}
