using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : MonoBehaviour
{
    // public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        // StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    [SerializeField] private List<DeckUnit> _playerDeck = new();
    public List<DeckUnit> PlayerDeck => _playerDeck;

    public void AddDeckUnit(DeckUnit unit)
    {
        PlayerDeck.Add(unit);
    }

    public void RemoveDeckUnit(DeckUnit unit)
    {
        PlayerDeck.Remove(unit);
    }

    public List<DeckUnit> GetDeck()
    {   
        return PlayerDeck;
    }

    public void SetDeck(List<DeckUnit> deck)
    {
        _playerDeck = deck;
    }

    private int _money;
    public int Money => _money;

    public bool MoneyChage(int cost)
    {
        if (_money + cost < 0)
        {
            return false;
        }
        else
        {
            _money += cost;
            return true;
        }
    }

    private int _darkEssense = 4;
    private int DarkEssense => _darkEssense;

    public bool DarkEssenseChage(int cost)
    {
        Debug.Log("Dark Essense: " + _darkEssense + " Change: " + cost);
        if (_darkEssense + cost < 0)
        {
            return false;
        }
        else
        {
            _darkEssense += cost;
            return true;
        }
    }
}
