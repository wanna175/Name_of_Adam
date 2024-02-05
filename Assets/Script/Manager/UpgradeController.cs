using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class UpgradeController
{
    public UpgradeController()
    {
        LoadUpgradeList();
    }

    private List<UpgradeData> _tier1UpgradeList = new();
    private List<UpgradeData> _tier2UpgradeList = new();
    private List<UpgradeData> _tier3UpgradeList = new();

    private void LoadUpgradeList()
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>($"Data/Upgrade");
        UpgradeLoader upgradeLoader = JsonUtility.FromJson<UpgradeLoader>(textAsset.text);

        foreach (UpgradeData data in upgradeLoader.UpgradeData)
        {
            if (data.Rarity == 1)
            {
                _tier1UpgradeList.Add(data);
            }
            else if (data.Rarity == 2)
            {
                _tier2UpgradeList.Add(data);
            }
            else if (data.Rarity == 3)
            {
                _tier3UpgradeList.Add(data);
            }
        }
    }

    public Upgrade DataToUpgrade(UpgradeData data)
    {
        Upgrade upgrade = new();

        upgrade.UpgradeData = data;

        upgrade.UpgradeName = data.Name;
        upgrade.UpgradeImage88 = GameManager.Resource.Load<Sprite>($"Arts/UI/Upgrade/" + data.Image + "_88");
        upgrade.UpgradeImage160 = GameManager.Resource.Load<Sprite>($"Arts/UI/Upgrade/" + data.Image + "_160");

        upgrade.UpgradeStat = new();

        //데이터의 범위 요소를 처리
        //HP
        if (data.HP.Contains("~"))
        {
            string[] splitList = data.HP.Split("~");
            string hp = splitList[Random.Range(0, splitList.Length)];

            upgrade.UpgradeStat.CurrentHP += int.Parse(hp);
            upgrade.UpgradeStat.MaxHP += int.Parse(hp);

            upgrade.UpgradeData.HP = hp;
        }
        else
        {
            upgrade.UpgradeStat.CurrentHP += int.Parse(data.HP);
            upgrade.UpgradeStat.MaxHP += int.Parse(data.HP);
        }

        //ATK
        if (data.ATK.Contains("~"))
        {
            string[] splitList = data.ATK.Split("~");
            string atk = splitList[Random.Range(0, splitList.Length)];

            upgrade.UpgradeStat.ATK += int.Parse(atk);

            upgrade.UpgradeData.ATK = atk;
        }
        else
        {
            upgrade.UpgradeStat.ATK += int.Parse(data.ATK);
        }

        //SPD
        if (data.SPD.Contains("~"))
        {
            string[] splitList = data.SPD.Split("~");
            string spd = splitList[Random.Range(0, splitList.Length)];

            upgrade.UpgradeStat.SPD += int.Parse(spd);

            upgrade.UpgradeData.SPD = spd;
        }
        else
        {
            upgrade.UpgradeStat.SPD += int.Parse(data.SPD);
        }

        //COST
        if (data.COST.Contains("~"))
        {
            string[] splitList = data.COST.Split("~");
            string cost = splitList[Random.Range(0, splitList.Length)];

            upgrade.UpgradeStat.ManaCost += int.Parse(cost);

            upgrade.UpgradeData.COST = cost;
        }
        else
        {
            upgrade.UpgradeStat.ManaCost += int.Parse(data.COST);
        }

        upgrade.UpgradeDescription = data.Description
                                                .Replace("(HP)", upgrade.UpgradeStat.MaxHP.ToString())
                                                .Replace("(ATK)", upgrade.UpgradeStat.ATK.ToString())
                                                .Replace("(SPD)", upgrade.UpgradeStat.SPD.ToString())
                                                .Replace("(COST)", upgrade.UpgradeStat.ManaCost.ToString());

        return upgrade;
    }

    public Upgrade GetRandomUpgrade()
    {
        UpgradeData upgrade = new();
        List<int> probability = new() { 96, 87 };
        int randNum;

        randNum = Random.Range(0, 100);

        if (randNum >= probability[0])
        {
            if (_tier3UpgradeList.Count > 0)
                upgrade = _tier3UpgradeList[Random.Range(0, _tier3UpgradeList.Count)];
        }
        else if (randNum >= probability[1])
        {
            if (_tier2UpgradeList.Count > 0)
                upgrade = _tier2UpgradeList[Random.Range(0, _tier2UpgradeList.Count)];
        }
        else
        {
            if (_tier1UpgradeList.Count > 0)
                upgrade = _tier1UpgradeList[Random.Range(0, _tier1UpgradeList.Count)];
        }

        return DataToUpgrade(upgrade);
    }
}