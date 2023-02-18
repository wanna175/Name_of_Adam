using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StageName
{
    낙인소,
    강화소,
    엘리트,
    전투,
    탕녀,
    물음표
}

struct StageInfo
{
    public StageName Stage;
    public int MaxAppear;
    public int Count_Max;
    public int Count_Remain;
}



public class StageManager
{
    [SerializeField] List<StageInfo> StageInfoList;

}