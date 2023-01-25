using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer SR;

    #region TileUnit
    private BattleUnit _TileUnit;
    public BattleUnit TileUnit => _TileUnit;
    #endregion
    #region isOnTile
    public bool _isOnTile;
    public bool isOnTile => _isOnTile;
    #endregion
    private FieldManager _FieldMNG;
    public bool CanSelect = false;

    
    private void Start()
    {
        _FieldMNG = GameManager.Instance.FieldMNG;
        SR = GetComponent<SpriteRenderer>();
        SR.color = Color.white;

        _TileUnit = null;
        _isOnTile = false;
        CanSelect = false;
    }

    #region Enter & Exit Tile

    public void EnterTile(BattleUnit _unit)
    {
        _isOnTile = true;
        _TileUnit = _unit;
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
            SetColor(Color.yellow);
        }
        else
        {
            CanSelect = false;
            SetColor(Color.white);
        }
    }

    public void SetColor(Color color)
    {
        SR.color = color;
    }

    // 유닛에서 실행하는 메서드
    public BattleUnit OnAttack()
    {
        // 타일이 공격범위 내에 있을 시
        // 타일에 대한 이펙트 등의 공격 처리

        return _TileUnit;
    }

    private void OnMouseDown()
    {
        // 필드에 자신이 클릭되었다는 정보를 준다.
        // 그러면 필드가 내가 어디에 위치해있는 타일인지 찾을 것

        _FieldMNG.TileClick(this);
    }

    
    // 
    // 지워질 것들
    // ↓↓↓↓↓↓↓↓
    
    public void OnFall(BattleUnit ch)
    {
        StartCoroutine(CoOnFall(ch));
    }
    IEnumerator CoOnFall(BattleUnit AttackChar)
    {
        SR.color = Color.yellow;

        if (_TileUnit != null)
        {
            if (AttackChar.BattleUnitSO.MyTeam != _TileUnit.BattleUnitSO.MyTeam)
            {
                _TileUnit.UnitAction.SetFallGauge(1);
            }
        }

        yield return new WaitForSeconds(0.5f);

        SR.color = Color.white;

    }
}
