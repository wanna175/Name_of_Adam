using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillHover : UI_Hover
{
    [SerializeField] public TextMeshProUGUI _description;

    [SerializeField] public GameObject _block;

    public void SetSkillHover(string name, int mana, int darkessence, string description, Vector2 position)
    {
        float posX;
        float posY;
        float ratio = 1920f / Screen.width;
        string costStr = $"<color=white><size=120%>Mana {mana} Dark Essence {darkessence}";

        switch (GameManager.Locale.CurrentLocaleIndex)
        {
            case 0: costStr = $"<color=#9696FF><size=120%>Mana {mana} Dark Essence {darkessence}"; break;
            case 1: costStr = $"<color=#9696FF><size=120%>마나 {mana} 검은 정수 {darkessence}";  break;
        }

        _description.SetText($"<color=#FF9696><size=150%>{name}\n{costStr}\n<color=white><size=100%>{description}");

        if (position.x > 1920 - 300 / ratio)
            posX = 1920 - 300 / ratio;
        else
            posX = position.x;

        if (position.y < 150 / ratio)
            posY = 150 / ratio;
        else
            posY = position.y;

        posX *= ratio;
        posY *= ratio;

        _block.GetComponent<RectTransform>().anchoredPosition = new(posX, posY);
    }
}
