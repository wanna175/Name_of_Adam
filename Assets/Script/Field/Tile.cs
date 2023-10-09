using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _tileFrame;
    private BattleUnit _unit = null;
    public BattleUnit Unit => _unit;
    public bool UnitExist { get { return Unit != null;} }

    private List<EffectTile> _effectTiles = new();

    public Action<Tile> OnClickAction = null;

    public Tile Init(Vector3 position)
    {
        transform.position = position;
        _highlight.SetActive(false);
        return this;
    }

    public void EnterTile(BattleUnit unit)
    {
        if (UnitExist)
        {
            Debug.Log("타일에 유닛이 존재합니다: " + unit.Data.Name);
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

    public void SetEffect(EffectTileType effectType)
    {
        if (CheckEffect(effectType))
            return;

        EffectTile tileEffect = GameManager.Resource.Instantiate("EffectTile/" + effectType.ToString(), transform).GetComponent<EffectTile>();
        tileEffect.SetEffect(effectType);
        _effectTiles.Add(tileEffect);
    }

    public void ClearEffect(EffectTileType effectType)
    {
        for (int i = 0; i < _effectTiles.Count; i++)
        {
            if (_effectTiles[i].GetEffect() == effectType)
            {
                Destroy(_effectTiles[i].gameObject);
                _effectTiles.RemoveAt(i);

                return;
            }
        }
    }

    public bool CheckEffect(EffectTileType effectType)
    {
        foreach (EffectTile tile in _effectTiles)
        {
            if (tile.GetEffect() == effectType)
            {
                return true;
            }
        }

        return false;
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
