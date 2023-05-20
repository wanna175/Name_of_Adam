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
        _block.GetComponent<Transform>().position = position;
    }
}
