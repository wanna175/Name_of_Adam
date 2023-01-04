using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer SR;

    Character chara;
    #region isOnTile
    bool _isOnTile;
    public bool isOnTile => _isOnTile;
    #endregion
    #region Loc X, Y
    int _LocX, _LocY;
    public int LocX => _LocX;
    public int LocY => _LocY;
    #endregion
    public bool CanSelect = false;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.color = Color.gray;

        _isOnTile = false;
        chara = null;
    }

    public void GetLocate(int x, int y)
    {
        _LocX = x;
        _LocY = y;
    }

    public void EnterTile(Character ch)
    {
        _isOnTile = true;
        chara = ch;
    }

    public void ExitTile()
    {
        _isOnTile = false;
        chara = null;
    }

    public void SetCanSelect(bool bo)
    {
        if (bo) {
            CanSelect = true;
            SR.color = Color.yellow;
        }
        else
        {
            CanSelect = false;
            SR.color = Color.gray;
        }

    }

    private void OnMouseDown()
    {
        if (CanSelect)
        {
            GameManager.Instance.BattleMNG.BattleField.CanSelectClear();
            GameManager.Instance.BattleMNG.SelectedChar.TileSelected(LocY, LocX);
            return;
        }
        if(chara != null)
        {
            if (chara.characterSO.team == Team.Player)
            {
                GameManager.Instance.BattleMNG.SelectedChar = chara;

                List<Vector2> vecList = chara.characterSO.HaveTargeting();
                if (vecList != null)
                {
                    GameManager.Instance.BattleMNG.BattleField.CanSelectClear();
                    Tile[,] tiles = GameManager.Instance.BattleMNG.BattleField.TileArray;

                    for(int i = 0; i < vecList.Count; i++)
                    {
                        int x = chara.LocX - (int)vecList[i].x;
                        int y = chara.LocY - (int)vecList[i].y;
                        Debug.Log(y + ", " + x);
                        if (0 <= x && x < 8)
                        {
                            if (0 <= y && y < 3)
                            {
                                tiles[y, x].SetCanSelect(true);
                            }
                        }
                    }
                }
            }
        }
    }

    #region OnAttack
    public void OnAttack(Character ch)
    {
        StartCoroutine(CoOnAttack(ch));
    }
    IEnumerator CoOnAttack(Character AttackChar)
    {
        SR.color = Color.white;

        if (chara != null)
        {
            if (AttackChar.characterSO.team != chara.characterSO.team)
            {
                chara.GetDamage(AttackChar.characterSO.stat.ATK);
            }
        }

        yield return new WaitForSeconds(0.5f);

        SR.color = Color.gray;

    }
    #endregion

    #region OnHeal
    public void OnHeal(Character ch)
    {
        StartCoroutine(CoOnHeal(ch));
    }
    IEnumerator CoOnHeal(Character AttackChar)
    {
        SR.color = Color.white;

        if (chara != null)
        {
            if (AttackChar.characterSO.team == chara.characterSO.team)
            {
                chara.GetDamage(-AttackChar.characterSO.stat.ATK);
            }
        }

        yield return new WaitForSeconds(0.5f);

        SR.color = Color.gray;

    }
    #endregion
}
