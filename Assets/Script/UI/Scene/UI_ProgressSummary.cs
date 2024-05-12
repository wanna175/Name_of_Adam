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

public class UI_ProgressSummary : UI_Scene
{
    [SerializeField]
    public List<ProgressText> ProgressList;
    public int TotalScore;
    public TMP_Text Title;

    private Progress _progress;
    

    public void Start()
    {
        _progress = GameManager.Data.GameData.Progress;

        SetProgressText();
    }

    public void SetProgressText()
    {
        SetProgressText(_progress.NormalWin, 10, $"{GameManager.Locale.GetLocalizedBattleScene("Normal battles won")} {_progress.NormalWin}{GameManager.Locale.GetLocalizedBattleScene("times_won")}", ProgressList[0]); //연결된 값 들
        SetProgressText(_progress.EliteWin, 50, $"{GameManager.Locale.GetLocalizedBattleScene("Elite battles won")} {_progress.EliteWin}{GameManager.Locale.GetLocalizedBattleScene("times_won")}", ProgressList[1]); //
        SetProgressText(_progress.BossWin, 100, $"{GameManager.Locale.GetLocalizedBattleScene("Boss battles won")} {_progress.BossWin}{GameManager.Locale.GetLocalizedBattleScene("times_won")}", ProgressList[2]); //
        SetProgressText(_progress.NormalKill, 2, $"{GameManager.Locale.GetLocalizedBattleScene("Normal units defeated")} {_progress.NormalKill}{GameManager.Locale.GetLocalizedBattleScene("times_defeated")}", ProgressList[3]); //
        SetProgressText(_progress.EliteKill, 25, $"{GameManager.Locale.GetLocalizedBattleScene("Elite units defeated")} {_progress.EliteKill}{GameManager.Locale.GetLocalizedBattleScene("times_defeated")}", ProgressList[4]); //
        SetProgressText(_progress.PhanuelKill, 200, $"{GameManager.Locale.GetLocalizedBattleScene("Banuel defeated")}", ProgressList[5]); //
        SetProgressText(_progress.HorusKill, 200, $"{GameManager.Locale.GetLocalizedBattleScene("Horus defeated")}", ProgressList[6]); //
        SetProgressText(_progress.FishKill, 200, "물고기 처치", ProgressList[7]); 
        SetProgressText(_progress.NormalFall, 5, $"{GameManager.Locale.GetLocalizedBattleScene("Normal units corrupted")} {_progress.NormalFall}{GameManager.Locale.GetLocalizedBattleScene("times_corrupted")}", ProgressList[8]); //
        SetProgressText(_progress.EliteFall, 100, $"{GameManager.Locale.GetLocalizedBattleScene("Elite units corrupted")} {_progress.EliteFall}{GameManager.Locale.GetLocalizedBattleScene("times_corrupted")}", ProgressList[9]); //
        SetProgressText(_progress.PhanuelFall, 500, $"{GameManager.Locale.GetLocalizedBattleScene("Banuel corrupted")}", ProgressList[10]); //
        SetProgressText(_progress.HorusFall, 500, $"{GameManager.Locale.GetLocalizedBattleScene("Horus corrupted")}", ProgressList[11]); //
        SetProgressText(_progress.FishFall, 500, "물고기 타락", ProgressList[12]); 
        SetProgressText(_progress.SecChapterClear, 1000, "2장까지 클리어", ProgressList[13]); 

        TMP_Text totalscore = GameObject.Find("TotalSum").GetComponent<TMP_Text>();
        totalscore.text = TotalScore.ToString();
        GameManager.OutGameData.SetProgressCoin(TotalScore);
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

            TotalScore += tempPoint;
        }
        else
        {
            progressText.ProgressObject.SetActive(false);
        }
    }
    
    public void OnClick()
    {
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallSaveInit(HallSaveCallback);
        GameObject.Find("UI_ProgressSummary").gameObject.SetActive(false);
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
            GameManager.OutGameData.AddHallUnit(deckUnit);
    }
}
