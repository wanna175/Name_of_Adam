using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer SR;
    private BattleUnit _unitOnTile = null;
    public BattleUnit UnitOnTile => _unitOnTile;
    public bool IsOnTile { get { if (UnitOnTile == null) return false; return true; } }

    private Field _field;

    public void Init()
    {
        _field = GameManager.BattleMNG.Field;
        SR = GetComponent<SpriteRenderer>();
        SR.color = Color.white;
    }

    public void EnterTile(BattleUnit _unit)
    {
        _unitOnTile = _unit;
    }

    public void ExitTile()
    {
        _unitOnTile = null;
    }

    public void SetColor(Color color)
    {
        SR.color = color;
    }

    private void OnMouseDown()
    {
        _field.TileClick(this);
    }
}
