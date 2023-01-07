using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    // 전투를 진행중인 캐릭터가 들어있는 리스트
    #region BattleCharList
    
    #region BattleCharList  
    List<Character> _BattleCharList = new List<Character>();
    public List<Character> BattleCharList => _BattleCharList;
    #endregion  

    // 리스트에 캐릭터를 추가 / 제거
    #region CharEnter / Exit
    public void BCL_CharEnter(Character ch)
    {
        BattleCharList.Add(ch);
    }
    public void BCL_CharExit(Character ch)
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
            Character max = null;
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
        Character dump = BattleCharList[a];
        BattleCharList[a] = BattleCharList[b];
        BattleCharList[b] = dump;
    }

    #endregion

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