using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerSkill : UI_Scene
{
    [SerializeField] private GameObject PlayerSkillCardPrefabs;
    [SerializeField] private Transform Grid;

    private UI_PlayerSkillCard _selectedCard = null;

    public bool Used = false;// once per turn


    public void SetSkill(List<PlayerSkill> skillList)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject.Instantiate(PlayerSkillCardPrefabs, Grid).GetComponent<UI_PlayerSkillCard>().Set(this, skillList[i]);
        }
    }

    public void OnClickHand(UI_PlayerSkillCard card)
    {
        PreparePhase prepare = (PreparePhase)BattleManager.Phase.Prepare;
        
        if (!prepare.isFirst && !Used && BattleManager.Mana.CanUseMana(card.GetSkill().GetManaCost()))
        {
            if (card != null && card == _selectedCard)
            {
                //선택 취소
                CancelSelect();
                card.GetSkill().CancelSelect();
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

    public  void CancelSelect()
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
