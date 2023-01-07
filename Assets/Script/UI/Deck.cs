using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<Hand> HandList;

    void OnMouseDown()
    {
        //전투 시작 후 초기 멀리건 4장
        for (int i = 0; i < 4; i++) {
            AddCharacter();
        }
    }

    /*
    void Awake()
    {
        //전투 시작 후 초기 멀리건 4장
        for (int i = 0; i < 4; i++) {
            AddCharacter();
        }
    }
    */

    public void AddCharacter()
    {
        //1,2,3,4 순으로 Hand의 Character가 null이면 1개 추가
        foreach (Hand h in HandList)
        {
            if (h.HandChar == null)
            {
                h.SetCharacter(GameManager.Instance.DataMNG.RandomChar());
                break;
            }
        }
    }
    public BattleUnit HandDel(int handIndex)
    {
        //handIndex는 1부터 시작하기에 -1 해야함
        BattleUnit returnChar;

        returnChar = HandList[handIndex-1].DelCharacter();

        for (int i = (handIndex-1)+1; i < 4; i++)
        {
            HandList[i-1].SetCharacter(HandList[i].DelCharacter());
        }

        AddCharacter();

        return returnChar;
    }
}
