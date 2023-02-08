using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManaGuage : MonoBehaviour
{
    [SerializeField] Image ManaGauge;
    [SerializeField] Text ManaText;

    private BattleDataManager _BattleDataMNG;

    private void Start()
    {
        _BattleDataMNG = GameManager.Battle.Data;

        _BattleDataMNG.SetManaGuage(this);
        _BattleDataMNG.InitMana();
        _BattleDataMNG.ChangeMana(4);
    }

    public void DrawGauge()
    {
        float MaxManaCost = 10;
        float ManaCost = (float)_BattleDataMNG.ManaCost;

        ManaGauge.fillAmount = ManaCost / MaxManaCost;
        ManaText.text = ManaCost.ToString();
    }

}
