using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerSkill : UI_Scene
{
    [SerializeField] private GameObject PlayerSkillCardPrefabs;
    [SerializeField] private Transform Grid;
    [SerializeField] private List<PlayerSkill> skillList;

    private UI_PlayerSkillCard _selectedCard = null;

    public bool Used = false;// once per turn

    private void Awake()
    {
        SetSkill();
    }

    public void SetSkill()
    {
        for (int i = 0; i < 4; i++)
        {
            UI_PlayerSkillCard newCard = GameObject.Instantiate(PlayerSkillCardPrefabs, Grid).GetComponent<UI_PlayerSkillCard>();
            newCard.Set(this, skillList[i]);
        }
    }

    public void OnClickHand(UI_PlayerSkillCard card)
    {
        if (!Used)
        {
            if (card != null && card == _selectedCard)
            {
                //선택 취소
                CancleSelect();
                card.GetSkill().CancleSelect();
            }
            else
            {
                //선택
                OnSelect(card);
                card.GetSkill().OnSelect();
            }
        }
        else
        {
            Debug.Log("Can't");
        }
    }

    public  void CancleSelect()
    {
        _selectedCard.ChangeSelectState(false);
        _selectedCard = null;
    }

    public  void OnSelect(UI_PlayerSkillCard card)
    {
        if (_selectedCard != null)
            _selectedCard.ChangeSelectState(false);

        _selectedCard = card;
        _selectedCard.ChangeSelectState(true);
    }

    public UI_PlayerSkillCard GetSelectedCard() => _selectedCard;
}
