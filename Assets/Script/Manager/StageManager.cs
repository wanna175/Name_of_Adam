using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncountFlag
{
    Monster = 1 << 0,
    Elite   = 1 << 1,
    Event   = 1 << 2
}

public class StageManager : MonoBehaviour
{
    #region StageSign
    [SerializeField] NextStageSign _StageSign;
    public NextStageSign StageSign => _StageSign;
    #endregion

    // 이 데이터들을 어디에다가 관리할까?
    public int encountMonster;
    public int encountElite;
    public int encountEvent;

    EncountFlag CheckEncount()
    {
        EncountFlag flag = new EncountFlag();

        if (0 < encountMonster)
            flag |= EncountFlag.Monster;
        if (0 < encountElite)
            flag |= EncountFlag.Elite;
        if (0 < encountEvent)
            flag |= EncountFlag.Event;

        return flag;
    }

    public List<EncountFlag> GetStage()
    {
        List<EncountFlag> EncountList = new List<EncountFlag>();

        EncountFlag flag = CheckEncount();

        if(flag == 0) // Boss
            return EncountList;

        if((flag & EncountFlag.Monster) == EncountFlag.Monster)
        {
            for (int i = 0; i < encountMonster; i++)
                EncountList.Add(EncountFlag.Monster);
        }
        if ((flag & EncountFlag.Elite) == EncountFlag.Elite)
        {
            for (int i = 0; i < encountElite; i++)
                EncountList.Add(EncountFlag.Elite);
        }
        if (encountEvent < 2)
        {
            for (int i = 0; i < 2; i++)
                EncountList.Add(EncountFlag.Event);
        }
        else
        {
            for (int i = 0; i < encountEvent; i++)
                EncountList.Add(EncountFlag.Event);
        }
        

        return EncountList;
    }
}