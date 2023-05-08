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

    [SerializeField] private TextMeshProUGUI _skillText;
    [SerializeField] private Image _SkillImage;

    [SerializeField] private Transform _rangeGrid;
    [SerializeField] private GameObject _squarePrefab;

    public void Set(DeckUnit unit, Team team, int currnetHP, int fall)
    {
        _name.text = unit.Data.Name;
        _cost.text = unit.Stat.ManaCost.ToString();

        _stat.text = "HP:     " + currnetHP.ToString() + " / " + unit.Stat.HP.ToString() + "\n" +
                       "Attack: " + unit.Stat.ATK.ToString() + "\n" +
                       "Speed:  " + unit.Stat.SPD.ToString();

        _hpBar.SetHPBar(team, null);
        _hpBar.SetFallBar(unit);

        _hpBar.RefreshHPBar((float)currnetHP / (float)unit.Stat.HP);
        _hpBar.RefreshFallGauge(fall);

        unit.SetStigma();
        foreach (Passive sti in unit.Stigmata)
        {
            Debug.Log("낙인");
            낙인 stig = unit.PassiveToStigma(sti);

            GameObject.Instantiate(_stigama_small, _stigamaGrid).GetComponent<UI_Stigma>().SetImage(unit.GetStigmaImage(stig), unit.GetStigmaText(stig));
        }

        _skillText.text = unit.Data.Description.Replace("(ATK)", unit.Stat.ATK.ToString());

        if (unit.Data.BehaviorType == BehaviorType.근거리)
            _SkillImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/근거리_아이콘");
        else
            _SkillImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/원거리_아이콘");

        foreach (bool range in unit.Data.AttackRange)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _rangeGrid).GetComponent<Image>();
            if (range)
                block.color = Color.red;
            else
                block.color = Color.grey;
        }
    }
}
