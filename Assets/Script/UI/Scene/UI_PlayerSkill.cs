using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UI_PlayerSkill : UI_Scene
{
    [SerializeField] private GameObject PlayerSkillCardPrefabs;
    [SerializeField] private Transform Grid;

    private UI_PlayerSkillCard _selectedCard = null;
    private List<UI_PlayerSkillCard> _currentCardList = new();

    public bool Used = false;// once per turn

    public void Init(List<PlayerSkill> skillList)
    {
        SetSkill(skillList);
    }

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
        bool isCanUseMana = BattleManager.Mana.CanUseMana(card.GetSkill().GetManaCost());
        bool isCanUseDarkEssense = GameManager.Data.CanUseDarkEssense(card.GetSkill().GetDarkEssenceCost());

        if (!Used)
        {
            if (!isCanUseMana)
            {
                BattleManager.BattleUI.UI_manaGauge.CannotEffect.Create();
                BattleManager.BattleUI.UI_controlBar.CreateSystemInfo(GameManager.Locale.GetLocalizedSystem("ManaIsLow"));
            }

            if (!isCanUseDarkEssense)
            {
                BattleManager.BattleUI.UI_darkEssence.CannotEffect.Create();
                BattleManager.BattleUI.UI_controlBar.CreateSystemInfo(GameManager.Locale.GetLocalizedSystem("DarkEssenceIsLow"));
            }

            if (isCanUseMana && isCanUseDarkEssense)
            {
                if (!BattleManager.BattleUI.UI_hands.IsSelectedHandNull)
                {
                    BattleManager.BattleUI.UI_hands.CancelSelect();
                }

                GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
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
                GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
        }
        else
        {
            GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
            Debug.Log("PlayerSkill is already used.");
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
                GameManager.Data.CanUseDarkEssense(_currentCardList[i].GetSkill().GetDarkEssenceCost()) &&
                !Used)
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
