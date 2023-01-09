using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitRenderer : MonoBehaviour
{
    BattleUnit _BattleUnit;
    BattleUnitSO _BattleUnitSO;
    SpriteRenderer _SR;


    private void Awake()
    {
        _BattleUnit = GetComponent<BattleUnit>();
        _SR = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _BattleUnitSO = _BattleUnit.BattleUnitSO;

        // sprite를 배치했다면 변경하기
        if (_BattleUnitSO.sprite != null)
            _SR.sprite = _BattleUnitSO.sprite;

        // 적군일 경우 x축 뒤집기
        _SR.flipX = (_BattleUnitSO.team == Team.Enemy) ? true : false;
    }
}
