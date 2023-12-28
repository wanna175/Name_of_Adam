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

    private Progress _progress;
    

    public void Start()
    {
        _progress = GameManager.Data.GameData.Progress;

        SetProgressText();
    }

    public void SetProgressText()
    {
        SetProgressText(_progress.NormalWin, 50, $"일반 전투 {_progress.NormalWin}회 승리", ProgressList[0]); //연결된 값 들
        SetProgressText(_progress.EliteWin, 300, $"엘리트 전투 {_progress.EliteWin}회 승리", ProgressList[1]); //
        SetProgressText(_progress.BossWin, 500, $"보스 전투 {_progress.BossWin}회 승리", ProgressList[2]);
        SetProgressText(_progress.NormalKill, 10, $"일반 유닛 {_progress.NormalKill}회 처치", ProgressList[3]); //
        SetProgressText(_progress.EliteKill, 50, $"엘리트 유닛 {_progress.EliteKill}회 처치", ProgressList[4]); //
        SetProgressText(_progress.NimrodKill, 100, "니므롯 처치", ProgressList[5]);
        SetProgressText(_progress.HorusKill, 100, "호루스 처치", ProgressList[6]);
        SetProgressText(_progress.FishKill, 100, "물고기 처치", ProgressList[7]);
        SetProgressText(_progress.NormalFall, 20, $"일반 유닛 {_progress.NormalFall}회 타락", ProgressList[8]); //
        SetProgressText(_progress.EliteFall, 100, $"엘리트 유닛 {_progress.EliteFall}회 타락", ProgressList[9]); //
        SetProgressText(_progress.NimrodFall, 500, "니므롯 타락", ProgressList[10]);
        SetProgressText(_progress.HorusFall, 500, "호루스 타락", ProgressList[11]);
        SetProgressText(_progress.FishFall, 500, "물고기 타락", ProgressList[12]);
        SetProgressText(_progress.SecChapterClear, 1000, "2장까지 클리어", ProgressList[13]); 
        SetProgressText(_progress.LeftDarkEssence, 10, $"남은 검은 정수 {_progress.LeftDarkEssence}개", ProgressList[14]); //
        SetProgressText(_progress.SurvivedNormal, 15, $"생존한 일반 유닛 {_progress.SurvivedNormal}명", ProgressList[15]); //
        SetProgressText(_progress.SurvivedElite, 25, $"생존한 엘리트 유닛 {_progress.SurvivedElite}명", ProgressList[16]); //
        SetProgressText(_progress.SurvivedBoss, 50, $"생존한 보스 유닛 {_progress.SurvivedBoss}명", ProgressList[17]); //

        TMP_Text totalscore = GameObject.Find("TotalSum").GetComponent<TMP_Text>();
        totalscore.text = TotalScore.ToString();
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
