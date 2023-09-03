using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FallUnit : MonoBehaviour
{
    [SerializeField] private GameObject _fill;
    [SerializeField] private Image _redCount;
    [SerializeField] private Image _whiteCount;
    [SerializeField] private float _fadeTime;

    public void FillGauge()
    {
        _fill.SetActive(false);
    }

    public void EmptyGauge()
    {
        _fill.SetActive(true);
    }

    public void SwitchCountImage(Team team)
    {
        if(team == Team.Player)
        {
            _redCount.gameObject.SetActive(true);
            _whiteCount.gameObject.SetActive(false);
        }
        else
        {
            _redCount.gameObject.SetActive(false);
            _whiteCount.gameObject.SetActive(true);
        }
    }
}
