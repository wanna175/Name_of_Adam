using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private BattleUnit _unit = null;
    public BattleUnit Unit => _unit;
    // IsOnTIle의 이름은 유닛이 가지는 이름같다. 위에 유닛이 있다는 것을 알리는 이름이 더 좋아보임
    public bool IsOnTile { get { if (Unit == null) return false; return true; } }
    public Action<Tile> OnClickAction = null;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = Color.white;
    }

    public Tile Init(Vector3 position, Action<Tile> action)
    {
        transform.position = position;
        OnClickAction = action;
        return this;
    }

    public void EnterTile(BattleUnit unit)
    {
        if (IsOnTile)
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
        OnClickAction(this);
    }
}
