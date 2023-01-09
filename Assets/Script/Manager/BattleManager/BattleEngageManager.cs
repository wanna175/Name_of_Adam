using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEngageManager : MonoBehaviour
{
    BattleDataManager _BattleDataMNG;

    // 턴 시작이 가능한 상태인가?
    bool CanTurnStart = true;


    private void Start()
    {
        _BattleDataMNG = GameManager.Instance.BattleMNG.BattleDataMNG;
    }


    // 턴 진행
    public void TurnStart()
    {
        // 턴 시작이 가능한 상태라면
        if (CanTurnStart)
        {
            CanTurnStart = false;
            
            // 턴 시작 전에 다시한번 순서를 정렬한다.
            _BattleDataMNG.BattleUnitMNG.BattleOrderReplace();
            
            CoroutineHandler.Start_Coroutine(CharUse(), 5);
        }
    }

    //턴에 딜레이 주기(어떻게 줘야할까?)
    IEnumerator CharUse()
    {
        List<BattleUnit> BattleUnitList = _BattleDataMNG.BattleUnitMNG.BattleUnitList;
        // 필드 위에 올라와있는 캐릭터들의 스킬을 순차적으로 사용한다
        for (int i = 0; i < BattleUnitList.Count; i++)
        {
            if (BattleUnitList[i] == null)
                break;

            BattleUnitList[i].use();

            // 각 스킬의 사용시간은 0.5초로 가정
            // 다음 캐릭터의 행동까지 대기시간은 0.5 X 이펙트 갯수
            // 여기에 컷씬을 넣으려면 다른 식을 사용해야함
            yield return new WaitForSeconds(BattleUnitList[i].BattleUnitSO.SkillLength() * 0.5f);
        }

        TurnEnd();
    }

    void TurnEnd()
    {
        _BattleDataMNG.ManaMNG.AddMana(2);
        CanTurnStart = true;
    }
}
