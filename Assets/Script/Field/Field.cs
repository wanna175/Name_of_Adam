using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] GameObject TilePrefabs;
    
    private void Awake()
    {
        GameManager.Instance.DataMNG.FieldSet(transform, TilePrefabs);

        transform.position = GameManager.Instance.DataMNG.FieldPosition;
        transform.eulerAngles = new Vector3(30, 0, 0);
    }
}
