using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StigmaInterface 
{
    void OnStigmataSelected(Stigma stigma);
    void OnSelectStigmataBestowalUnit(DeckUnit unit);
    List<Stigma> ResetStigmataList(DeckUnit stigmataTargetUnit);
}
