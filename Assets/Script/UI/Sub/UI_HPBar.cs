using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    [Header("HP 관련")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _playerBar;
    [SerializeField] private Image _enemyBar;

    [Header("타락 게이지 관련")]
    [SerializeField] private Transform _grid;
    [SerializeField] private GameObject _fallGaugeUnit; // 타락 게이지 각 보석


    private List<Transform> _fallGauge = new List<Transform>();
    private Team team;

    public void SetHPBar(Team team, Transform trans)
    {
        if (team == Team.Player)
            _playerBar.gameObject.SetActive(true);
        else
            _enemyBar.gameObject.SetActive(true);

        this.team = team;
    }

    public void SetFallBar(DeckUnit unit)
    {
        int max = unit.Stat.FallMaxCount;
        int current = unit.Stat.FallCurrentCount;

        for(int i=0; i<max; i++)
        {
            GameObject newObject = GameObject.Instantiate(_fallGaugeUnit, _grid);

            if(i >= current)
                Util.FindChild(newObject, "Fill").SetActive(false);
        }
    }

    public void RefreshHPBar(float amount)
    {
        if (team == Team.Player)
            _playerBar.fillAmount = amount;
        else
            _enemyBar.fillAmount = amount;
    }
}
