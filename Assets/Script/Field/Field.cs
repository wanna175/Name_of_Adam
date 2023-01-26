using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    FieldManager _FieldMNG;

    private void Awake()
    {
        _FieldMNG = GameManager.Instance.FieldMNG;
        _FieldMNG.Init(transform);
    }
}