using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_FallUnit : MonoBehaviour
{
    [SerializeField] private GameObject _fill;
    [SerializeField] private Image _fallImage;
    [SerializeField] private Animator _anim;

    private Team _team;
    private int _fallType = 0; // 0 = 화이트(아군) or 레드(적군), 1 = 남색, 2 = 금색

    public void InitFall(Team team, int fallType)
    {
        _team = team;
        _fallType = fallType;

        _anim.SetInteger("Type", _fallType);
        _anim.SetBool("IsPlayer", _team.Equals(Team.Player));
    }

    public void SetVisible(bool isVisible) 
        => _fallImage.gameObject.SetActive(isVisible);

    public void IncreaseGauge(float delay) => StartCoroutine(StartIncreaseGauge(delay));

    public void DecreaseGauge(float delay) => StartCoroutine(StartDecreaseGauge(delay));

    IEnumerator StartIncreaseGauge(float delay)
    {
        yield return new WaitForSeconds(delay);
        _anim.SetBool("IsPlay", true);
        _anim.SetBool("IsBreak", false);
    }

    IEnumerator StartDecreaseGauge(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(delay);
        _anim.SetBool("IsPlay", true);
        _anim.SetBool("IsBreak", true);
    }
}
