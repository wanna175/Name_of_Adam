using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer SR;

    private BattleUnit _TileUnit;
    #region Loc X, Y
    int _LocX, _LocY;
    public int LocX => _LocX;
    public int LocY => _LocY;
    #endregion
    #region isOnTile
    bool _isOnTile;
    public bool isOnTile => _isOnTile;
    #endregion
    private Field _field;
    public bool CanSelect = false;


    void Start()
    {
        _field = transform.parent.GetComponent<Field>();
        SR = GetComponent<SpriteRenderer>();
        SR.color = Color.gray;

        _TileUnit = null;
        _isOnTile = false;
        CanSelect = false;
    }

    #region Enter & Exit Tile
    public void EnterTile(BattleUnit ch)
    {
        _isOnTile = true;
        _TileUnit = ch;
    }

    public void ExitTile()
    {
        _isOnTile = false;
        _TileUnit = null;
    }
    #endregion

    public void SetCanSelect(bool bo)
    {
        if (bo)
        {
            CanSelect = true;
            SR.color = Color.yellow;
        }
        else
        {
            CanSelect = false;
            SR.color = Color.gray;
        }
    }

    // 유닛에서 실행하는 메서드
    public BattleUnit OnAttack()
    {
        // 타일이 공격범위 내에 있을 시
        // 타일에 대한 공격 처리

        return _TileUnit;
    }

    private void OnMouseDown()
    {
        // 필드에 자신이 클릭되었다는 정보를 준다.
        // 그러면 필드가 내가 어디에 위치해있는 타일인지 찾을 것
        _field.TileClick(this);
    }

    
    // 
    // 지워질 것들
    // 캐릭터 컴포넌트 분리시킬 때 얘들도 같이 수정하기
    // ↓↓↓↓↓↓↓↓

    #region OnAttack
    public void OnAttack(BattleUnit ch)
    {
        StartCoroutine(CoOnAttack(ch));
    }
    IEnumerator CoOnAttack(BattleUnit AttackChar)
    {
        SR.color = Color.white;

        if (_TileUnit != null)
        {

            if (AttackChar.characterSO.team != _TileUnit.characterSO.team)
            {
                GameManager.Instance.BattleMNG.BattleCutScene(transform.parent, AttackChar, _TileUnit);
                _TileUnit.GetDamage(AttackChar.GetStat().ATK);
                Debug.Log($"{AttackChar.gameObject.name}' ATK : {AttackChar.GetStat().ATK}");
            }
        }

        yield return new WaitForSeconds(0.5f);

        SR.color = Color.gray;

    }
    #endregion

    #region OnHeal
    public void OnHeal(BattleUnit ch)
    {
        StartCoroutine(CoOnHeal(ch));
    }
    IEnumerator CoOnHeal(BattleUnit AttackChar)
    {
        SR.color = Color.white;

        if (_TileUnit != null)
        {
            if (AttackChar.characterSO.team == _TileUnit.characterSO.team)
            {
                _TileUnit.GetDamage(-AttackChar.GetStat().ATK);
            }
        }

        yield return new WaitForSeconds(0.5f);

        SR.color = Color.gray;

    }
    #endregion

    #region OnFall
    public void OnFall(BattleUnit ch)
    {
        StartCoroutine(CoOnFall(ch));
    }
    IEnumerator CoOnFall(BattleUnit AttackChar)
    {
        SR.color = Color.yellow;

        if (_TileUnit != null)
        {
            if (AttackChar.characterSO.team != _TileUnit.characterSO.team)
            {
                _TileUnit.SetFallGauge(1);
            }
        }

        yield return new WaitForSeconds(0.5f);

        SR.color = Color.gray;

    }
    #endregion
}
