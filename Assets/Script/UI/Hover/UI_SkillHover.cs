using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillHover : UI_Hover
{
    [SerializeField] public TextMeshProUGUI _name;
    [SerializeField] public TextMeshProUGUI _cost;
    [SerializeField] public TextMeshProUGUI _description;

    [SerializeField] public GameObject _block;

    public void SetSkillHover(string name, int mana, int darkessence, string description, Vector2 position)
    {
        float posX;
        float posY;

        _name.text = name;
        _cost.text =  "마나 " + mana + " 검은정수 " + darkessence;
        _description.text = description;

        if (position.x > 1920 - 300)
            posX = 1920 - 300;
        else
            posX = position.x;

        if (position.y < 150)
            posY = 150;
        else
            posY = position.y;

        _block.GetComponent<Transform>().position = new(posX, posY);
    }
}
