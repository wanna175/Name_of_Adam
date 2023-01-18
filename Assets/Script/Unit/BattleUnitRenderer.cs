using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitRenderer : MonoBehaviour
{
    BattleUnit _BattleUnit;
    BattleUnitSO _BattleUnitSO;
    SpriteRenderer _SR;
    Animator _Animator;

    private void Awake()
    {
        _BattleUnit = GetComponent<BattleUnit>();
        _SR = GetComponent<SpriteRenderer>();
        _Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _BattleUnitSO = _BattleUnit.BattleUnitSO;

        // sprite를 배치했다면 변경하기
        if (_BattleUnitSO.sprite != null)
            _SR.sprite = _BattleUnitSO.sprite;

        // 적군일 경우 x축 뒤집기
        _SR.flipX = (!_BattleUnitSO.MyTeam) ? true : false;
    }

    public void SetUnitLayer(int num)
    {
        _SR.sortingOrder = num;
    }

    public void SetIsAttackAnim(bool bo)
    {
        _Animator.SetBool("isAttack", bo);
    }

    public void HitAnim(bool bo)
    {
        _Animator.SetBool("isHit", bo);
    }

    public bool GetFlipX() => _SR.flipX;
}
