using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        SetProgressText(_progress.NormalWin, 10, $"일반 전투 {_progress.NormalWin}회 승리", ProgressList[0]); //연결된 값 들
        SetProgressText(_progress.EliteWin, 50, $"엘리트 전투 {_progress.EliteWin}회 승리", ProgressList[1]); //
        SetProgressText(_progress.BossWin, 100, $"보스 전투 {_progress.BossWin}회 승리", ProgressList[2]); //
        SetProgressText(_progress.NormalKill, 2, $"일반 유닛 {_progress.NormalKill}회 처치", ProgressList[3]); //
        SetProgressText(_progress.EliteKill, 5, $"엘리트 유닛 {_progress.EliteKill}회 처치", ProgressList[4]); //
        SetProgressText(_progress.PhanuelKill, 30, "바누엘 처치", ProgressList[5]); //
        SetProgressText(_progress.HorusKill, 30, "호루스 처치", ProgressList[6]); //
        SetProgressText(_progress.FishKill, 30, "물고기 처치", ProgressList[7]); 
        SetProgressText(_progress.NormalFall, 5, $"일반 유닛 {_progress.NormalFall}회 타락", ProgressList[8]); //
        SetProgressText(_progress.EliteFall, 50, $"엘리트 유닛 {_progress.EliteFall}회 타락", ProgressList[9]); //
        SetProgressText(_progress.PhanuelFall, 200, "바누엘 타락", ProgressList[10]); //
        SetProgressText(_progress.HorusFall, 200, "호루스 타락", ProgressList[11]); //
        SetProgressText(_progress.FishFall, 200, "물고기 타락", ProgressList[12]); 
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
        if(_progress.BossWin > 0)
        {
            GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallSaveInit(true, (deckUnit) => { GameManager.OutGameData.AddHallUnit(deckUnit, true); });
        }
        else
        {
            GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallSaveInit(false, (deckUnit) => { GameManager.OutGameData.AddHallUnit(deckUnit, false); });
        }
        GameObject.Find("UI_ProgressSummary").gameObject.SetActive(false);
    }
}
