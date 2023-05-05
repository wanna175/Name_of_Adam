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


    private List<GameObject> _fallGauge = new List<GameObject>();
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

        if (max > 6) max = 6; // 스마게까지 타락 Max 6

        for(int i=0; i<max; i++)
        {
            GameObject newObject = GameObject.Instantiate(_fallGaugeUnit, _grid);
            _fallGauge.Add(newObject);

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

    public void RefreshFallGauge(float current)
    {
        for (int i = 0; i < current; i++)
            Util.FindChild(_fallGauge[i].gameObject, "Fill").SetActive(true);
    }
}
