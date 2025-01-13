using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EliteReward : UI_Popup
{
    [SerializeField]
    private GameObject backPanel;

    [SerializeField]
    private List<UI_EliteCard> uI_EliteCards;

    private int _stageAct;

    private DeckUnit GetRandomUnit(List<UnitDataSO> unitLists, List<int> selectedUnitNumbers)
    {
        int unitNumber = Random.Range(0, unitLists.Count);

        while (selectedUnitNumbers.Contains(unitNumber))
        {
            Debug.Log($"'{unitNumber}�� ����'��(��) �ߺ��� �����Դϴ�.");
            unitNumber = Random.Range(0, unitLists.Count);
        }

        DeckUnit deckUnit = new DeckUnit();
        deckUnit.Data = unitLists[unitNumber];
        deckUnit.HallUnitID = -1;

        SetRandomStigmas(deckUnit);
        selectedUnitNumbers.Add(unitNumber);

        return deckUnit;
    }

    private void SetRandomStigmas(DeckUnit deckUnit)
    {
        int addStigmaNum = 1;

        switch (_stageAct)
        {
            case 0: addStigmaNum = 1; break;
            case 1: addStigmaNum = 2; break;
        }

        for (int i = 0; i < addStigmaNum; i++)
        {
            Stigma stigma = GameManager.Data.StigmaController.GetRandomStigmaAsUnit(GameManager.Data.StigmaController.StigmaProbability, deckUnit.Data.name);
            deckUnit.AddStigma(stigma);
        }
    }

    public void EnablePanel(bool isEnable)
    => backPanel.SetActive(isEnable);

    public void SetRewardPanel()
    {
        List<UnitDataSO> normalUnits = new();
        List<int> selectedUnitNumbers = new();
        _stageAct = GameManager.Data.StageAct;

        var units = GameManager.Resource.LoadAll<UnitDataSO>($"ScriptableObject/UnitDataSO");
        foreach (var unit in units)
        {
            if (unit.Rarity == Rarity.Normal && !unit.IsBattleOnly)
            {
                normalUnits.Add(unit);
            }
        }

        foreach (var card in uI_EliteCards)
        {
            DeckUnit randUnit = GetRandomUnit(normalUnits, selectedUnitNumbers);
            card.Init(randUnit);
        }

        EnablePanel(true);
    }
}
