using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private BattleUnit _unit = null;
    public BattleUnit Unit => _unit;
    public bool UnitExist { get { if (Unit == null) return false; return true; } }
    public Action<Tile> OnClickAction = null;

    public Tile Init(Vector3 position)
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = Color.white;
        //color.a = 0;
        transform.position = position;
        return this;
    }

    public void EnterTile(BattleUnit unit)
    {
        if (UnitExist)
        {
            Debug.Log("타일에 유닛이 존재합니다.");
            return;
        }
 
        _unit = unit;
    }

    public void ExitTile()
    {
        _unit = null;
    }

    public void SetColor(Color color)
    {
        _renderer.color = color;
    }

    private void OnMouseDown()
    {
        //OnClickAction(this);
        BattleManager.Instance.OnClickTile(this);
    }

    private void OnMouseEnter()
    {
        BattleManager.Field.MouseEnterTile(this);
    }

    private void OnMouseExit()
    {
        BattleManager.Field.MouseExitTile(this);
    }
}
