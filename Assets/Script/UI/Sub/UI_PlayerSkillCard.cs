using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_PlayerSkillCard : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _inactive;
    //[SerializeField] private TextMeshProUGUI _text;

    private UI_PlayerSkill _playerSkill;
    private PlayerSkill _skill;
    public bool IsSelected = false;

    private void Start()
    {
        _highlight.SetActive(false);
        _inactive.SetActive(false);
    }

    public void Set(UI_PlayerSkill ps, PlayerSkill skill)
    {
        _playerSkill = ps;
        _skill = skill;
        //_text.text = skill.GetName();
        GetComponent<Image>().sprite = skill.GetSkillImage();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.UI.ShowHover<UI_SkillHover>().SetSkillHover(_skill.GetName(), _skill.GetManaCost(), _skill.GetDarkEssenceCost(), _skill.GetDescription(), eventData.position);
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.UI.CloseHover();

        if (IsSelected)
            return;
        _highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _playerSkill.OnClickHand(this);
    }

    public void ChangeSelectState(bool b)
    {
        IsSelected = b;
        _highlight.SetActive(b);
    }

    public void ChangeInable(bool b)
    {
        _inactive.SetActive(b);
    }

    public PlayerSkill GetSkill() => _skill;
}
