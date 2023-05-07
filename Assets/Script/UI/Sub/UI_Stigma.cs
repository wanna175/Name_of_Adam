using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Stigma : UI_Base
{
    [SerializeField] Image StigmaImage;

    public void SetImage(Sprite image)
    {
        StigmaImage.sprite = image;
    }
}
