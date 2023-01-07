using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CharacterAdder : MonoBehaviour
{
    [SerializeField] Character add;

    void OnMouseDown()
    {
        GameManager.Instance.DataMNG.AddCharToDeck(add);
    }
}
