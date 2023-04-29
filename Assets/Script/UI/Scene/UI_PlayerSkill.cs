using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerSkill : UI_Scene
{
    [SerializeField] private GameObject PlayerSkillCardPrefabs;
    [SerializeField] private Transform Grid;
    private UI_PlayerSkillCard _selectedCard = null;
    private BattleManager _battle;

    public bool Used = false;// once per turn

    private void Awake()
    {
        _battle = GameManager.Battle;
        SetSkill();
    }

    public void SetSkill()
    {
        for (int i = 0; i < 4; i++)
        {
            UI_PlayerSkillCard newCard = GameObject.Instantiate(PlayerSkillCardPrefabs, Grid).GetComponent<UI_PlayerSkillCard>();
            newCard.Set(this);
        }
    }

    public void OnClickHand(UI_PlayerSkillCard card)
    {
        if (!Used && _battle.Mana.CanUseMana(20))
        {
            if (card != null && card == _selectedCard)
                CancleSelect();
            else
                SelectCard(card);
        }
        else
        {
            Debug.Log("Can't");
        }


    }

    public void CancleSelect()
    {
        _selectedCard.ChangeSelectState(false);
        _selectedCard = null;
        _battle.FallPlayerSkillReady(false);
    }

    private void SelectCard(UI_PlayerSkillCard card)
    {
        if (_selectedCard != null)
            _selectedCard.ChangeSelectState(false);

        _selectedCard = card;
        _selectedCard.ChangeSelectState(true);
        _battle.FallPlayerSkillReady(true);
    }
}
