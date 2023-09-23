using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _tileFrame;
    private SpriteRenderer _renderer;
    private BattleUnit _unit = null;
    public BattleUnit Unit => _unit;
    public bool UnitExist { get { if (Unit == null) return false; return true; } }
    public Action<Tile> OnClickAction = null;

    public Tile Init(Vector3 position)
    {
        _renderer = GetComponent<SpriteRenderer>();
        //_renderer.color = Color.white;
        //color.a = 0;
        transform.position = position;
        _highlight.SetActive(false);
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
        if (color.Equals(Color.white))
            _highlight.SetActive(false);
        else
        {
            _highlight.SetActive(true);
            StartCoroutine(ChangeAlphaOverTime(color, _highlight.GetComponent<SpriteRenderer>(), _tileFrame.GetComponent<SpriteRenderer>()));
            //color.a = 150 / 255f;
            //_highlight.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private IEnumerator ChangeAlphaOverTime(Color color, SpriteRenderer highlightSprite, SpriteRenderer tileSprite)
    {
        float duration = 0.3f;
        float elapsedTime = 0f;
        Color tileStartColor = Color.white;
        tileStartColor.a = 0f;
        Color tileEndColor = Color.white;
        Color startColor = color;
        Color endColor = color; // 최종 알파 값은 1로 설정
        endColor.a = 100/255f;

        while (elapsedTime < duration)
        {
            // 시간에 따라 알파 값을 변경합니다.
            float t = elapsedTime / duration;
            highlightSprite.color = Color.Lerp(startColor, endColor, t);
            tileSprite.color = Color.Lerp(tileStartColor, tileEndColor, t);


            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 종료 후 최종 알파 값을 보정합니다.
        tileSprite.color = tileEndColor;
        highlightSprite.color = endColor;
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
