using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckList : MonoBehaviour
{
    [Serializable]
    public class DeckFormat
    {
        public UnitData Data;
        public Stat ChangedStat;
        public List<Passive> Stigmas = new List<Passive>();
    }

    public List<DeckFormat> deckUnits = new List<DeckFormat>();
}
