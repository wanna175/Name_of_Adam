using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    #region DeckCharList
    List<BattleUnit> _DeckCharList = new List<BattleUnit>();
    public List<BattleUnit> DeckCharList => _DeckCharList;

    public void AddCharToDeck(BattleUnit ch) {
        DeckCharList.Add(ch);
    }

    public void RemoveCharFromDeck(BattleUnit ch) {
        DeckCharList.Remove(ch);
    }

    public BattleUnit RandomChar() {
        if (DeckCharList.Count == 0)
        {
            return null;
        }
        int randNum = Random.Range(0, DeckCharList.Count);
        
        BattleUnit ch = DeckCharList[randNum];
        DeckCharList.RemoveAt(randNum);

        return ch;
    }
    
    #endregion
}
