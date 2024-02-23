using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FallUnit : MonoBehaviour
{
    [SerializeField] private GameObject _fill;
    [SerializeField] private Image _redCount;
    [SerializeField] private Image _whiteCount;
    [SerializeField] private Image _doubleCount;
    [SerializeField] private Image _tripleCount;
    [SerializeField] private Animator anim;

    private int FallCount = 0;
    public int GetDouble()
    {
        return FallCount;
    }
    public void SetAnimation()
    {
        anim.SetBool("isBreak", true);
    }
    public void FillGauge()
    {
        switch (FallCount)
        {
            case 1:
                FallCount--;
                break;
            case 2:
                FallCount--;
                break;
            case 3:
                FallCount--;
                break;
        }
        anim.SetBool("isBreak", true);
    }

    public void EndFillAnim()
    {
        if (FallCount == 0)
            this.gameObject.SetActive(false);
        else if (FallCount == 1)
        {
            this._doubleCount.gameObject.SetActive(false);
            this._tripleCount.gameObject.SetActive(false);
        }
        else if (FallCount == 2)
        {
            this._tripleCount.gameObject.SetActive(false);
            this._doubleCount.gameObject.SetActive(true);
        }
        anim.Rebind();
    }

    public void EmptyGauge()
    {
        anim.SetBool("isBreak", false);
        switch (FallCount)
        {
            case 0:
                this.gameObject.SetActive(true);
                FallCount++;
                break;
            case 1:
                this._doubleCount.gameObject.SetActive(true);
                FallCount++;
                break;
            case 2:
                this._tripleCount.gameObject.SetActive(true);
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
