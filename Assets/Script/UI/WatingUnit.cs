using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatingUnit : MonoBehaviour
{
    private SpriteRenderer SR;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    public void SetUnit(Sprite s)
    {
        GetComponent<Renderer>().enabled = true;
        SR.sprite = s;
    }

    public void RemoveUnit()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
