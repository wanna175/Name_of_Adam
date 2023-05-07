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
    [SerializeField] private UI_Stigma _stigama_small;
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

        unit.SetStigma();
        foreach (Passive sti in unit.Stigmata)
        {
            Debug.Log("³«ÀÎ");
            ³«ÀÎ stig = unit.PassiveToStigma(sti);

            GameObject.Instantiate(_stigama_small, _stigamaGrid).GetComponent<UI_Stigma>().SetImage(unit.GetStigmaImage(stig));
        }
    }
}
