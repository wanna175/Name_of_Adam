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
        _FieldMNG.FieldSet(transform);

        transform.position = _FieldMNG.FieldPosition;
        transform.eulerAngles = new Vector3(16, 0, 0);
    }
}

// 23.01.23 김종석 - 수정된 사항
// 클릭 시 이벤트 처리부분을 FieldManager로 옮김