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
        SetProgressText(_progress.NormalWin, 10, $"�Ϲ� ���� {_progress.NormalWin}ȸ �¸�", ProgressList[0]); //����� �� ��
        SetProgressText(_progress.EliteWin, 50, $"����Ʈ ���� {_progress.EliteWin}ȸ �¸�", ProgressList[1]); //
        SetProgressText(_progress.BossWin, 100, $"���� ���� {_progress.BossWin}ȸ �¸�", ProgressList[2]); //
        SetProgressText(_progress.NormalKill, 2, $"�Ϲ� ���� {_progress.NormalKill}ȸ óġ", ProgressList[3]); //
        SetProgressText(_progress.EliteKill, 5, $"����Ʈ ���� {_progress.EliteKill}ȸ óġ", ProgressList[4]); //
        SetProgressText(_progress.PhanuelKill, 30, "�ٴ��� óġ", ProgressList[5]); //
        SetProgressText(_progress.HorusKill, 30, "ȣ�罺 óġ", ProgressList[6]); //
        SetProgressText(_progress.FishKill, 30, "����� óġ", ProgressList[7]); 
        SetProgressText(_progress.NormalFall, 5, $"�Ϲ� ���� {_progress.NormalFall}ȸ Ÿ��", ProgressList[8]); //
        SetProgressText(_progress.EliteFall, 50, $"����Ʈ ���� {_progress.EliteFall}ȸ Ÿ��", ProgressList[9]); //
        SetProgressText(_progress.PhanuelFall, 200, "�ٴ��� Ÿ��", ProgressList[10]); //
        SetProgressText(_progress.HorusFall, 200, "ȣ�罺 Ÿ��", ProgressList[11]); //
        SetProgressText(_progress.FishFall, 200, "����� Ÿ��", ProgressList[12]); 
        SetProgressText(_progress.SecChapterClear, 1000, "2����� Ŭ����", ProgressList[13]); 

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
