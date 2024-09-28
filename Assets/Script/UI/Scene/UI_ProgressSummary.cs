using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Device;

[Serializable]
public class ProgressText
{
    public GameObject ProgressObject;
    public TMP_Text Progress_Count;
    public TMP_Text Progress_Point;
}

public class UI_ProgressSummary : UI_Popup
{
    [SerializeField] public List<ProgressText> ProgressList;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private TMP_Text _title;

    private int _totalScore;
    private Progress _progress;

    public void Init(string title)
    {
        _progress = GameManager.Data.GameData.Progress;
        _title.text = title;
        SetProgressText();

        if (!GameManager.OutGameData.Data.IsOnTooltipForSanctumInBattle)
        {
            GameManager.OutGameData.Data.IsOnTooltipForSanctumInBattle = true;
            UI_SystemInfo systemInfo = GameManager.UI.ShowPopup<UI_SystemInfo>();
            systemInfo.Init("TooltipForSanctumInBattle", "");

            GameManager.OutGameData.SaveData();
        }
    }

    public void SetProgressText()
    {
        SetProgressText(_progress.NormalWin, 10, $"{GameManager.Locale.GetLocalizedBattleScene("Normal battles won")} {_progress.NormalWin} {GameManager.Locale.GetLocalizedBattleScene("times_won")}", ProgressList[0]);
        SetProgressText(_progress.EliteWin, 70, $"{GameManager.Locale.GetLocalizedBattleScene("Elite battles won")} {_progress.EliteWin} {GameManager.Locale.GetLocalizedBattleScene("times_won")}", ProgressList[1]);
        SetProgressText(_progress.BossWin, 100, $"{GameManager.Locale.GetLocalizedBattleScene("Boss battles won")} {_progress.BossWin} {GameManager.Locale.GetLocalizedBattleScene("times_won")}", ProgressList[2]);
        SetProgressText(_progress.NormalKill, 3, $"{GameManager.Locale.GetLocalizedBattleScene("Normal units defeated")} {_progress.NormalKill} {GameManager.Locale.GetLocalizedBattleScene("times_defeated")}", ProgressList[3]);
        SetProgressText(_progress.EliteKill, 25, $"{GameManager.Locale.GetLocalizedBattleScene("Elite units defeated")} {_progress.EliteKill} {GameManager.Locale.GetLocalizedBattleScene("times_defeated")}", ProgressList[4]);
        SetProgressText(_progress.PhanuelKill, 200, $"{GameManager.Locale.GetLocalizedBattleScene("Phanuel defeated")}", ProgressList[5]);
        SetProgressText(_progress.SaviorKill, 200, $"{GameManager.Locale.GetLocalizedBattleScene("Savior defeated")}", ProgressList[6]);
        SetProgressText(_progress.YohrnKill, 200, $"{GameManager.Locale.GetLocalizedBattleScene("Yohrn defeated")}", ProgressList[7]);
        SetProgressText(_progress.NormalFall, 5, $"{GameManager.Locale.GetLocalizedBattleScene("Normal units corrupted")} {_progress.NormalFall} {GameManager.Locale.GetLocalizedBattleScene("times_corrupted")}", ProgressList[8]);
        SetProgressText(_progress.EliteFall, 50, $"{GameManager.Locale.GetLocalizedBattleScene("Elite units corrupted")} {_progress.EliteFall} {GameManager.Locale.GetLocalizedBattleScene("times_corrupted")}", ProgressList[9]);
        SetProgressText(_progress.PhanuelFall, 300, $"{GameManager.Locale.GetLocalizedBattleScene("Phanuel corrupted")}", ProgressList[10]);
        SetProgressText(_progress.SaviorFall, 300, $"{GameManager.Locale.GetLocalizedBattleScene("Savior corrupted")}", ProgressList[11]);
        SetProgressText(_progress.YohrnFall, 300, $"{GameManager.Locale.GetLocalizedBattleScene("Yohrn corrupted")}", ProgressList[12]);
        SetProgressText(_progress.AllChapterClear, 1000, $"{GameManager.Locale.GetLocalizedBattleScene("All chapter clear")}", ProgressList[13]);

        TMP_Text totalscore = GameObject.Find("TotalSum").GetComponent<TMP_Text>();
        totalscore.text = _totalScore.ToString();
        GameManager.OutGameData.Data.ProgressCoin += _totalScore;
        GameManager.OutGameData.SaveData();
    }

    public void SetProgressText(int count, int point, string text, ProgressText progressText)
    {
        if (count != 0)
        {
            int tempCount = count;
            int tempPoint = tempCount * point;

            progressText.ProgressObject.SetActive(true);
            progressText.Progress_Count.text = text;
            progressText.Progress_Point.text = tempPoint.ToString();

            _totalScore += tempPoint;
        }
        else
        {
            progressText.ProgressObject.SetActive(false);
        }
    }
    
    public void OnClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallSaveInit(HallSaveCallback);
        this.gameObject.SetActive(false);
    }

    private void HallSaveCallback(DeckUnit deckUnit)
    {
        bool isExist = false;

        foreach (var hallUnit in GameManager.OutGameData.FindHallUnitList())
        {
            if (hallUnit.PrivateKey == deckUnit.PrivateKey)
            {
                UI_HallUnitSave popup = GameManager.UI.ShowPopup<UI_HallUnitSave>();
                popup.Init(hallUnit.ConvertToDeckUnit(), deckUnit);
                isExist = true;
                break;
            }
        }

        if (isExist == false)
        {
            GameManager.OutGameData.AddHallUnit(deckUnit);
            SceneChanger.SceneChange("MainScene");
        }
    }
}
