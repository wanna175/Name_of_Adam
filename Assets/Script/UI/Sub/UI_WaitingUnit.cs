using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingUnit : MonoBehaviour
{
    private Image _Image;

    void Start()
    {
        _Image = GetComponent<Image>();
    }

    public void SetUnit(Sprite s)
    {
        Debug.Log("1");
        _Image.enabled = true;
        //_Image.sprite = s;
        Debug.Log("2");
    }

    public void RemoveUnit()
    {
        _Image.enabled = false;
    }
}
