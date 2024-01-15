using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FallUnit : MonoBehaviour
{
    [SerializeField] private GameObject _fill;
    [SerializeField] private Image _redCount;
    [SerializeField] private Image _whiteCount;
    [SerializeField] private Animator anim;

    private int FallCount = 0;
    public int GetDouble()
    {
        return FallCount;
    }
    public void FillGauge()
    {
        anim.SetBool("isBreak", true);
        switch (FallCount)
        {
            case 1:
                FallCount--;
                break;
            case 2:
                FallCount--;
                break;
        }
    }

    public void EndFillAnim()
    {
        this.gameObject.SetActive(false);
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
            if (FallCount < 2)
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
}
