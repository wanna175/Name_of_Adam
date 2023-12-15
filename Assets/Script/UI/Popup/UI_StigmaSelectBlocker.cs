using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StigmaSelectBlocker : MonoBehaviour
{
    public void ButtonClick()
    {
        this.gameObject.SetActive(false);

        Transform e = this.transform.parent.GetChild(0);
        e.SetAsLastSibling();
        e.gameObject.SetActive(true);
    }
}
