using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatingUnit : MonoBehaviour
{
    SpriteRenderer SR;

    void start()
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
