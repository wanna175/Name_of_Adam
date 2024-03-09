using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerSkill : UI_Scene
{
    [SerializeField] private GameObject PlayerSkillCardPrefabs;
    [SerializeField] private Transform Grid;

    private UI_PlayerSkillCard _selectedCard = null;
    private List<UI_PlayerSkillCard> _currentCardList = new();

    public bool Used = false;// once per turn

    public void SetSkill(List<PlayerSkill> skillList)
    {
        for (int i = 0; i < 3; i++)
        {
            UI_PlayerSkillCard playerSkillCard = GameObject.Instantiate(PlayerSkillCardPrefabs, Grid).GetComponent<UI_PlayerSkillCard>();
            playerSkillCard.Set(this, skillList[i]);
            _currentCardList.Add(playerSkillCard);
        }
    }

    public void RefreshSkill(List<PlayerSkill> skillList)
    {
        for (int i = 0; i < 3; i++)
        {
            _currentCardList[i].Set(this, skillList[i]);
        }

        InableCheck();
    }

    public void OnClickHand(UI_PlayerSkillCard card)
    {
        //PreparePhase prepare = (PreparePhase)BattleManager.Phase.Prepare;
        if (!Used && BattleManager.Mana.CanUseMana(card.GetSkill().GetManaCost()) && GameManager.Data.CanUseDarkEssense(card.GetSkill().GetDarkEssenceCost()))
        {
            if (!BattleManager.BattleUI.UI_hands.IsSelectedHandNull)
            {
                BattleManager.BattleUI.UI_hands.CancelSelect();
            }

            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
            if (card != null && card == _selectedCard)
            {
                //���� ���
                CancelSelect();
                card.GetSkill().CancelSelect();
            }
            else if (_selectedCard != null && card != _selectedCard)
            {
                //���� ���� ��� �� �缱��
                _selectedCard.GetSkill().CancelSelect();
                OnSelect(card);
                card.GetSkill().OnSelect();
            }
            else
            {
                //����
                OnSelect(card);
                card.GetSkill().OnSelect();
            }
        }
        else
        {
            GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
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

    public void InableSkill(bool isUsed)
    {
        Used = isUsed;

        for (int i = 0; i < 3; i++)
        {
            _currentCardList[i].ChangeInable(Used);
        }
    }

    public void InableCheck()
    {
        for (int i = 0; i < 3; i++)
        {
            if (BattleManager.Mana.CanUseMana(_currentCardList[i].GetSkill().GetManaCost()) && 
                GameManager.Data.CanUseDarkEssense(_currentCardList[i].GetSkill().GetDarkEssenceCost()))
            {
                _currentCardList[i].ChangeInable(false);
            }
            else
            {
                _currentCardList[i].ChangeInable(true);
            }
        }
    }

    public UI_PlayerSkillCard GetSelectedCard() => _selectedCard;
}
