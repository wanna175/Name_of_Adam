using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UnitCard : UI_Scene
{
    [SerializeField] private Image _unitImage;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;

    public void SetCard(Sprite image, string name, string cost)
    {
        _unitImage.sprite = image;
        _name.text = name;
        _cost.text = cost;
    }

    public void SetHand(Sprite image, string cost)
    {
        _unitImage.sprite = image;
        _cost.text = cost;
    }
}
