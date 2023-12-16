using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StigmaInterface 
{
    void OnStigmaSelected(Stigma stigma);
    void OnSelectStigmatization(DeckUnit unit);
    void IsStigmaFull();
}
