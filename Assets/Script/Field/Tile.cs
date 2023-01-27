using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer SR;

    private BattleUnit _unitOnTile;
    public BattleUnit UnitOnTile => _unitOnTile;
    public bool IsOnTile { get { if (UnitOnTile == null) return false; return true; } }

    private FieldManager _FieldMNG;
    public bool CanSelect = false;

    
    private void Start()
    {
        _FieldMNG = GameManager.Instance.FieldMNG;
        SR = GetComponent<SpriteRenderer>();
        SR.color = Color.white;

        _unitOnTile = null;
        CanSelect = false;
    }

    public void EnterTile(BattleUnit _unit)
    {
        _unitOnTile = _unit;
    }

    public void ExitTile()
    {
        _unitOnTile = null;
    }
    

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

    private void OnMouseDown()
    {
        // 필드에 자신이 클릭되었다는 정보를 준다.
        // 그러면 필드가 내가 어디에 위치해있는 타일인지 찾을 것

        _FieldMNG.TileClick(this);
    }
}
