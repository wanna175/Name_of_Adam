using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _playerBar;
    [SerializeField] private Image _enemyBar;

    public void SetHPBar(Team team, Transform trans)
    {
        if (team == Team.Player)
            _playerBar.gameObject.SetActive(true);
        else
            _enemyBar.gameObject.SetActive(true);

        Vector3 vector = Camera.main.WorldToScreenPoint(trans.position);
        Debug.Log(vector);
        _background.transform.position = vector;
    }
}
