using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Info : UI_Popup
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private TextMeshProUGUI _stat;

    [SerializeField] private UI_HPBar _hpBar;
    [SerializeField] private Transform _stigamaGrid;

    public void Set(DeckUnit unit, Team team, float hp, int fall)
    {
        _name.text = unit.Data.Name;
        _cost.text = unit.Stat.ManaCost.ToString();

        _stat.text = "HP:     " + unit.Stat.HP.ToString() + "\n" +
                       "Attack: " + unit.Stat.ATK.ToString() + "\n" +
                       "Speed:  " + unit.Stat.SPD.ToString();

        _hpBar.SetHPBar(team, null);
        _hpBar.SetFallBar(unit);

        _hpBar.RefreshHPBar(hp);
        _hpBar.RefreshFallGauge(fall);
    }
}
