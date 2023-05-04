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
    

    public void SetImage(Sprite image)
    {
        _unitImage.sprite = image;
    }

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetCost(string cost)
    {
        _cost.text = cost;
    }
}
