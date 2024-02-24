using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StigmaDescription : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _description;
    [SerializeField] Image _image;

    public void SetStigma(Stigma stigma)
    {
        _name.text = stigma.Name;
        _description.text = stigma.Description;
        _image.sprite = stigma.Sprite_28;
    }
}
