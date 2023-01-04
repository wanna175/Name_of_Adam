using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전투를 담당하는 매니저
// 필드와 턴의 관리
// 필드에 올라와있는 캐릭터의 제어를 배틀매니저에서 담당

public class BattleManager : MonoBehaviour
{
    // 마나 UI
    [SerializeField] ManaGuage PlayerMana;
    // 전투가 진행되는 필드
    #region BattleField
    [SerializeField] Field _BattleField;
    public Field BattleField => _BattleField;
    #endregion

    bool CanTurnStart = true;

    // 턴 진행
    public void TurnStart()
    {
        if (CanTurnStart)
        {
            CanTurnStart = false;
            GameManager.Instance.DataMNG.BattleOrderReplace();

            StartCoroutine(CharUse());
        }
    }
    //턴에 딜레이 주기(어떻게 줘야할까?)
    IEnumerator CharUse()
    {
        List<Character> BattleCharList = GameManager.Instance.DataMNG.BattleCharList;

        for (int i = 0; i < BattleCharList.Count; i++)
        {
            BattleCharList[i].use();

            yield return new WaitForSeconds(1f);
        }

        TurnEnd();
    }

    void TurnEnd()
    {
        PlayerMana.AddMana(2);
        CanTurnStart = true;
    }
}