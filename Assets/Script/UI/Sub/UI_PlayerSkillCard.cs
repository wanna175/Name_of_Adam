using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_PlayerSkillCard : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private TextMeshProUGUI _text;

    private UI_PlayerSkill _playerSkill;
    public bool IsSelected = false;
    private bool _IsLocked = true;


    private void Start()
    {
        _highlight.SetActive(false);
    }

    public void Set(UI_PlayerSkill ps, string text, bool locked=true)
    {
        _playerSkill = ps;
        _text.text = text;
        _IsLocked = locked;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_IsLocked)
            _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_IsLocked)
        { 
            if (IsSelected)
                return;
            _highlight.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_IsLocked)
            _playerSkill.OnClickHand(this);
    }

    public void ChangeSelectState(bool b)
    {
        if (!_IsLocked) 
        {
            IsSelected = b;
            _highlight.SetActive(b);
        }
    }
}
