using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class UpgradeController
{
    public UpgradeController()
    {
        LoadUpgradeList();
    }

    public readonly float[] UpgradeProbability = new float[] { 0.90f, 0.08f, 0.02f };

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
        //upgrade.UpgradeImage88 = GameManager.Resource.Load<Sprite>($"Arts/UI/Upgrade/" + data.Image + "_88");
        //upgrade.UpgradeImage160 = GameManager.Resource.Load<Sprite>($"Arts/UI/Upgrade/" + data.Image + "_160");

        upgrade.UpgradeImage88 = GameManager.Resource.Load<Sprite>($"Arts/UI/Upgrade/" + data.Image);
        upgrade.UpgradeImage160 = GameManager.Resource.Load<Sprite>($"Arts/UI/Upgrade/" + data.Image);

        /*
        string[] splitDescription = data.Description.Split('\n');
        foreach (string description in splitDescription)
            upgrade.UpgradeDescription += $"{GameManager.Locale.GetLocalizedUpgrade(description)}\n";
        */
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

        return upgrade;
    }

    public Upgrade GetRandomUpgrade()
    {
        UpgradeData upgrade = new();
        int randIndex = RandomManager.GetElement(UpgradeProbability);

        switch (randIndex)
        {
            case 0:
                if (_tier1UpgradeList.Count > 0)
                    upgrade = _tier1UpgradeList[Random.Range(0, _tier1UpgradeList.Count)];
                break;

            case 1:
                if (_tier2UpgradeList.Count > 0)
                    upgrade = _tier2UpgradeList[Random.Range(0, _tier2UpgradeList.Count)];
                break;

            case 2:
                if (_tier3UpgradeList.Count > 0)
                    upgrade = _tier3UpgradeList[Random.Range(0, _tier3UpgradeList.Count)];
                break;
        }

        return DataToUpgrade(upgrade);
    }

    public Upgrade GetRandomUpgrade(DeckUnit unit)
    {
        Upgrade upgrade = new();

        while (true)
        {
            upgrade = GetRandomUpgrade();

            Stat checkStat = unit.Data.RawStat + upgrade.UpgradeStat;

            if (checkStat.MaxHP < 0 || checkStat.ATK <= 0 || checkStat.SPD <= 0 || checkStat.ManaCost < 0)
            {
                continue;
            }
            else
            {
                break;
            }
        }

        return upgrade;

    }

    readonly string _upColorStr = "#FF3838";
    readonly string _downColor = "#5959FF";
    readonly string _tier3Color = "#FF8C00";
    readonly string _tier2Color = "#D1A645";
    readonly string _tier1Color = "#FFFFFF";

    public string GetUpgradeFullDescription(Upgrade upgrade)
    {
        string colorText;
        if (upgrade.UpgradeData.Rarity == 3)
            colorText = _tier3Color;
        else if (upgrade.UpgradeData.Rarity == 2)
            colorText = _tier2Color;
        else
            colorText = _tier1Color;

        return "<color=" + colorText + "><size=150%>" + GameManager.Locale.GetLocalizedUpgrade(upgrade.UpgradeName) + "</size></color>\n\n<size=120%>" +
        GameManager.Data.UpgradeController.GetUpgradeDescription(upgrade) + "</size>";
    }

    public string GetUpgradeDescription(Upgrade upgrade)
    {
        string upgradeDescription = "";
        string[] splitDescription = upgrade.UpgradeData.Description.Split('\n');
        foreach (string description in splitDescription)
            upgradeDescription += $"{GameManager.Locale.GetLocalizedUpgrade(description)}\n";

        //ATK
        if (upgrade.UpgradeStat.ATK > 0)
            upgradeDescription = upgradeDescription.Replace("(ATK)", "<color=" + _upColorStr + ">+(ATK)</color>");
        else if (upgrade.UpgradeStat.ATK < 0)
            upgradeDescription = upgradeDescription.Replace("(ATK)", "<color=" + _downColor + ">(ATK)</color>");

        //HP
        if (upgrade.UpgradeStat.MaxHP > 0)
            upgradeDescription = upgradeDescription.Replace("(HP)", "<color=" + _upColorStr + ">+(HP)</color>");
        else if (upgrade.UpgradeStat.MaxHP < 0)
            upgradeDescription = upgradeDescription.Replace("(HP)", "<color=" + _downColor + ">(HP)</color>");

        //SPD
        if (upgrade.UpgradeStat.SPD > 0)
            upgradeDescription = upgradeDescription.Replace("(SPD)", "<color=" + _upColorStr + ">+(SPD)</color>");
        else if (upgrade.UpgradeStat.SPD < 0)
            upgradeDescription = upgradeDescription.Replace("(SPD)", "<color=" + _downColor + ">(SPD)</color>");

        //COST
        if (upgrade.UpgradeStat.ManaCost > 0)
            upgradeDescription = upgradeDescription.Replace("(COST)", "<color=" + _downColor + ">+(COST)</color>");
        else if (upgrade.UpgradeStat.ManaCost < 0)
            upgradeDescription = upgradeDescription.Replace("(COST)", "<color=" + _upColorStr + ">(COST)</color>");

        upgradeDescription = upgradeDescription
                                                .Replace("(HP)", upgrade.UpgradeStat.MaxHP.ToString())
                                                .Replace("(ATK)", upgrade.UpgradeStat.ATK.ToString())
                                                .Replace("(SPD)", upgrade.UpgradeStat.SPD.ToString())
                                                .Replace("(COST)", upgrade.UpgradeStat.ManaCost.ToString());

        return upgradeDescription;
    }

    public UpgradeData UpgradeUpdateCheck(UpgradeData data)
    {
        UpgradeData updateData = new();

        if (data.Rarity == 1)
        {
            updateData = _tier1UpgradeList.Find(x => x.ID == data.ID);
        }
        else if (data.Rarity == 2)
        {
            updateData = _tier2UpgradeList.Find(x => x.ID == data.ID);
        }
        else if (data.Rarity == 3)
        {
            updateData = _tier3UpgradeList.Find(x => x.ID == data.ID);
        }

        if (updateData.Name != data.Name || updateData.Description != data.Description || updateData.Image != data.Image)
        {
            data.Name = updateData.Name;
            data.Description = updateData.Description;
            data.Image = updateData.Image;
        }

        return data;
    }
}