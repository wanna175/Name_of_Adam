using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerSkill : UI_Scene
{
    [SerializeField] private GameObject PlayerSkillCardPrefabs;
    [SerializeField] private Transform Grid;

    public UI_PlayerSkillCard _selectedCard = null;

    public List<GameObject> curCardList = new();

    public bool Used = false;// once per turn


    public void SetSkill(List<PlayerSkill> skillList)
    {
        for (int i = 0; i < 3; i++)
        {
            //GameObject.Instantiate(PlayerSkillCardPrefabs, Grid).GetComponent<UI_PlayerSkillCard>().Set(this, skillList[i]);
            GameObject Skill = GameObject.Instantiate(PlayerSkillCardPrefabs, Grid);
            Skill.GetComponent<UI_PlayerSkillCard>().Set(this, skillList[i]);
            
            curCardList.Add(Skill);
        }
    }

    public void OnClickHand(UI_PlayerSkillCard card)
    {
        //PreparePhase prepare = (PreparePhase)BattleManager.Phase.Prepare;
        
        if (!Used && BattleManager.Mana.CanUseMana(card.GetSkill().GetManaCost()) && GameManager.Data.CanUseDarkEssense(card.GetSkill().GetDarkEssenceCost()))
        {
            if (card != null && card == _selectedCard)
            {
                //선택 취소
                CancelSelect();
                card.GetSkill().CancelSelect();
            }
            else if (_selectedCard != null && card != _selectedCard)
            {
                //기존 선택 취소 및 재선택
                _selectedCard.GetSkill().CancelSelect();
                OnSelect(card);
                card.GetSkill().OnSelect();
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

    public void CancelSelect()
    {
        if (_selectedCard == null)
            return;

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

    public void InableSkill()
    {
        for (int i = 0; i < 3; i++)
        {
            if(Used == true)
            {
                curCardList[i].GetComponent<UI_PlayerSkillCard>().ChangeInable(true);
            }
            else
            {
                curCardList[i].GetComponent<UI_PlayerSkillCard>().ChangeInable(false);
            }

        }
    }

    public void InableCheck(int manaValue)
    {
        for (int i = 0; i < 3; i++)
        {
            if (manaValue < curCardList[i].GetComponent<UI_PlayerSkillCard>()._skill.GetManaCost())
            {
                curCardList[i].GetComponent<UI_PlayerSkillCard>().ChangeInable(true);
            }
            else
            {
                curCardList[i].GetComponent<UI_PlayerSkillCard>().ChangeInable(false);
            }

        }
    }

    public UI_PlayerSkillCard GetSelectedCard() => _selectedCard;
}
