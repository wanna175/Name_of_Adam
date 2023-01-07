using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    // 전투를 진행중인 캐릭터가 들어있는 리스트
    #region BattleCharList

    #region BattleCharList  
    List<BattleUnit> _BattleCharList = new List<BattleUnit>();
    public List<BattleUnit> BattleCharList => _BattleCharList;
    #endregion  

    // 리스트에 캐릭터를 추가 / 제거
    #region CharEnter / Exit
    public void BCL_CharEnter(BattleUnit ch)
    {
        BattleCharList.Add(ch);
    }
    public void BCL_CharExit(BattleUnit ch)
    {
        BattleCharList.Remove(ch);
    }
    #endregion

    #region OrderSort

    public void BattleOrderReplace()
    {
        BCL_SpeedSort();
    }

    // 일단 선택 정렬으로 정렬, 나중에 바꾸기
    void BCL_SpeedSort()
    {
        for (int i = 0; i < BattleCharList.Count; i++)
        {
            BattleUnit max = null;
            for (int j = i; j < BattleCharList.Count; j++)
            {
                if (i == j)
                {
                    max = BattleCharList[j];
                }
                else if (BattleCharList[j].GetSpeed() > max.GetSpeed())
                {
                    CharSwap(i, j);
                }
                else if (BattleCharList[j].GetSpeed() == max.GetSpeed())
                {
                    if (BattleCharList[j].LocX < max.LocX)
                    {
                        CharSwap(i, j);
                    }
                    else if (BattleCharList[j].LocX == max.LocX)
                    {
                        if (BattleCharList[j].LocY < max.LocY)
                        {
                            CharSwap(i, j);
                        }
                    }
                }
            }
        }
    }

    void CharSwap(int a, int b)
    {
        BattleUnit dump = BattleCharList[a];
        BattleCharList[a] = BattleCharList[b];
        BattleCharList[b] = dump;
    }

    #endregion

    #endregion

    #region FieldData

    // 필드의 최대 넓이
    const int MaxFieldX = 8;
    const int MaxFieldY = 3;

    Tile[,] _TileArray;
    public Tile[,] TileArray => _TileArray;

    // 필드의 생성을 위한 필드의 위치
    public Vector3 FieldPosition => new Vector3(0, -1.4f, 0);

    // 필드 생성
    public void FieldSet(Transform trans, GameObject TilePrefabs)
    {
        _TileArray = new Tile[MaxFieldY, MaxFieldX];

        Vector3 vec = trans.position;

        float disX = trans.localScale.x / MaxFieldX;
        float disY = trans.localScale.y / MaxFieldY;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                float x = (disX * j) + (disX * 0.5f);
                float y = disY * i;

                GameObject tile = GameObject.Instantiate(TilePrefabs, trans);
                tile.transform.position = new Vector3(vec.x + x, vec.y + y);

                _TileArray[i + 1, j + 4] = tile.GetComponent<Tile>();
                _TileArray[i + 1, j + 4].Init();
                _TileArray[i + 1, j + 4].GetLocate(i + 1, j + 4);
            }
        }
    }

    // 지정한 위치에 있는 타일의 좌표를 반환
    public Vector3 GetTileLocate(int x, int y)
    {
        Vector3 vec = _TileArray[y, x].transform.position;

        float sizeX = _TileArray[y, x].transform.localScale.x * 0.5f;
        float sizeY = _TileArray[y, x].transform.localScale.y * 0.5f;

        vec.x += sizeX;
        vec.y += sizeY;

        return vec;
    }

    public void CanSelectClear()
    {
        for (int i = 0; i < MaxFieldY; i++)
        {
            for (int j = 0; j < MaxFieldX; j++)
            {
                _TileArray[i, j].SetCanSelect(false);
            }
        }
    }


    public void EnterTile(BattleUnit ch, int x, int y)
    {
        _TileArray[y, x].EnterTile(ch);
    }

    public void ExitTile()
    {
        _isOnTile = false;
        chara = null;
    }

    #endregion

    #region ManaGuage

    const int _MaxManaCost = 10;
    public int MaxManaCost => _MaxManaCost;
    public int ManaCost = 0;

    public void InitMana()
    {
        ManaCost = 0;
    }

    public void AddMana(int value)
    {
        if (10 <= ManaCost + value)
            ManaCost = 10;
        else
            ManaCost += value;
    }

    #endregion

    #region DeckCharList
    List<Character> _DeckCharList = new List<Character>();
    public List<Character> DeckCharList => _DeckCharList;

    public void AddCharToDeck(Character ch) {
        DeckCharList.Add(ch);
    }

    public void RemoveCharFromDeck(Character ch) {
        DeckCharList.Remove(ch);
    }

    public Character RandomChar() {
        if (DeckCharList.Count == 0)
        {
            return null;
        }
        int randNum = Random.Range(0, DeckCharList.Count);
        
        Character ch = DeckCharList[randNum];
        DeckCharList.RemoveAt(randNum);

        return ch;
    }
    
    #endregion
}
