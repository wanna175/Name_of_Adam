using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaGuage : MonoBehaviour
{
    [SerializeField] Image ManaGauge;
    [SerializeField] Text ManaText;

    private ManaManager _ManaMNG;

    private void Start()
    {
        _ManaMNG = GameManager.Instance.BattleMNG.BattleDataMNG.ManaMNG;

        _ManaMNG.SetManaGuage(this);
        _ManaMNG.InitMana();
        _ManaMNG.AddMana(4);
    }

    public void DrawGauge()
    {
        float MaxManaCost = (float)_ManaMNG.MaxManaCost;
        float ManaCost = (float)_ManaMNG.ManaCost;

        ManaGauge.fillAmount = ManaCost / MaxManaCost;
        ManaText.text = ManaCost.ToString();
    }

}
