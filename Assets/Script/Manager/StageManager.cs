using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncountStage
{
    Monster = 1 << 0,
    Elite   = 1 << 1,
    Event   = 1 << 2,

    Boss
}

public class StageManager
{
    public EncountStage[] StageArray;

    // 각 종류의 스테이지가 얼마나 남았는지 관리
    // 이 데이터들을 어디에다가 관리할까?
    public int encountMonster = 4;
    public int encountElite = 2;
    public int encountEvent = 4;

    public EncountStage[] GetStageArray()
    {
        StageArray = new EncountStage[3];
        return StageArray;
    }

    // 등장할 수 있는 스테이지를 반환
    EncountStage CheckEncount()
    {
        EncountStage flag = new EncountStage();

        if (0 < encountMonster)
            flag |= EncountStage.Monster;
        if (0 < encountElite)
            flag |= EncountStage.Elite;
        if (0 < encountEvent)
            flag |= EncountStage.Event;

        return flag;
    }

    // 등장 가능한 스테이지를 남은 갯수만큼 리스트에 넣고 반환
    public List<EncountStage> GetStage()
    {
        List<EncountStage> EncountList = new List<EncountStage>();

        EncountStage flag = CheckEncount();

        if(flag == 0) // Boss
            return EncountList;

        if((flag & EncountStage.Monster) == EncountStage.Monster)
        {
            for (int i = 0; i < encountMonster; i++)
                EncountList.Add(EncountStage.Monster);
        }
        if ((flag & EncountStage.Elite) == EncountStage.Elite)
        {
            for (int i = 0; i < encountElite; i++)
                EncountList.Add(EncountStage.Elite);
        }
        // 이벤트는 등장가능 횟수를 모두 소모해도 계속 등장한다.
        if (encountEvent <= 2)
        {
            int eve = 2;
            if (flag == EncountStage.Event)
                eve = 3;

            for (int i = 0; i < eve; i++)
                EncountList.Add(EncountStage.Event);
        }
        else
        {
            for (int i = 0; i < encountEvent; i++)
                EncountList.Add(EncountStage.Event);
        }
        

        return EncountList;
    }

    // 선택된 박스에 들어있는 스테이지로 진입
    public void StageSelect(int index)
    {
        // 박스에 들어있는 스테이지를 encount에 할당
        EncountStage encount = GameManager.Instance.StageMNG.StageArray[index];
        
        switch (encount)
        {
            case EncountStage.Monster:
                GameManager.Instance.StageMNG.encountMonster--;
                StageInit(EncountStage.Monster);
                break;
            case EncountStage.Elite:
                GameManager.Instance.StageMNG.encountElite--;
                StageInit(EncountStage.Elite);
                break;
            case EncountStage.Event:
                if (--GameManager.Instance.StageMNG.encountEvent < 0)
                    GameManager.Instance.StageMNG.encountEvent = 0;
                StageInit(EncountStage.Event);
                break;
            case EncountStage.Boss:
                StageInit(EncountStage.Boss);
                break;
        }

        GameManager.Instance.SceneChanger.SceneChange("SampleScene");
    }

    void StageInit(EncountStage stage)
    {
        // 새로 만들어지는 방의 정보

        // 전 스테이지에서 사용된 데이터를 모두 초기화
        GameManager.Instance.DataMNG.BattleCharList.Clear();

        // 방 생성은 여기서? 아니면 배틀매니저에서?

        Debug.Log("Monster : " + encountMonster + ", Elite : " + encountElite + ", Event : " + encountEvent);
    }

}