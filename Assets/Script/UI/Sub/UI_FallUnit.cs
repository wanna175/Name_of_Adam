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

    private int FallCount = 0;
    public int GetDouble()
    {
        return FallCount;
    }
    public void FillGauge()
    {
        switch (FallCount)
        {
            case 1:
                this.gameObject.SetActive(false);
                FallCount--;
                break;
            case 2:
                FallCount--;
                break;
        }
    }

    public void EmptyGauge()
    {
        switch (FallCount)
        {
            case 0:
                this.gameObject.SetActive(true);
                FallCount++;
                break;
            case 1:
                FallCount++;
                break;
        }
    }

    public void SwitchCountImage(Team team)
    {
        if(team == Team.Player)
        {
            _redCount.gameObject.SetActive(false);
            _whiteCount.gameObject.SetActive(true);
        }
        else
        {
            _redCount.gameObject.SetActive(true);
            _whiteCount.gameObject.SetActive(false);
        }
    }
}
