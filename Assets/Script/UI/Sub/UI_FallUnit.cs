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
    private int _fallType = 0; // 0 = ȭ��Ʈ(�Ʊ�) or ����(����), 1 = ����, 2 = �ݻ�

    public void InitFall(Team team, int fallType)
    {
        _team = team;
        _fallType = fallType;

        _anim.SetInteger("Type", _fallType);
        _anim.SetBool("IsPlayer", _team.Equals(Team.Player));
    }

    public void SetVisible(bool isVisible) 
        => _fallImage.gameObject.SetActive(isVisible);

    public void IncreaseGauge()
    {
        _anim.SetBool("IsPlay", true);
        _anim.SetBool("IsBreak", false);
    }

    public void DecreaseGauge()
    {
        _anim.SetBool("IsPlay", true);
        _anim.SetBool("IsBreak", true);
    }
}
