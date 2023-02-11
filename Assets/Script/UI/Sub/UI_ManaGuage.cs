using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_ManaGuage : MonoBehaviour
{
    [SerializeField] public TMP_Text ManaText;
    private BattleDataManager _BattleDataMNG;

    private void Start()
    {
        
    }

    public void Print_Mana()
    {
        ManaText.text = _BattleDataMNG.GetMana().ToString();
    }

    
}
