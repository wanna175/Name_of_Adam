using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FallUnit : MonoBehaviour
{
    [SerializeField] private GameObject _fill;

    public void FillGauge()
    {
        _fill.SetActive(true);
    }

    public void EmptyGauge()
    {
        _fill.SetActive(false);
    }
}
