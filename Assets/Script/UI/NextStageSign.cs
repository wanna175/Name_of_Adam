using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStageSign : MonoBehaviour
{
    [SerializeField] List<Image> Container;

    List<EncountFlag> StageArray = new List<EncountFlag>();

    public void SetNextStageSign()
    {
        List<EncountFlag> StageList = GameManager.Instance.StageMNG.GetStage();
        StageArray.Clear();

        if(StageList.Count != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                int rand = Random.Range(0, StageList.Count);

                StageArray.Add(StageList[rand]);
                StageList.Remove(StageList[rand]);
            }

            DrawSign();
        }
        else
        {
            // boss
        }
    }

    void DrawSign()
    {
        for(int i = 0; i < Container.Count; i++)
        {
            switch (StageArray[i])
            {
                case EncountFlag.Monster:
                    Container[i].color = Color.red;
                    break;
                case EncountFlag.Elite:
                    Container[i].color = Color.blue;
                    break;
                case EncountFlag.Event:
                    Container[i].color = Color.yellow;
                    break;
            }
        }
    }

    public void StageSelect(int index)
    {
        EncountFlag encount = StageArray[index];

        switch (encount)
        {
            case EncountFlag.Monster:
                GameManager.Instance.StageMNG.encountMonster--;
                break;
            case EncountFlag.Elite:
                GameManager.Instance.StageMNG.encountElite--;
                break;
            case EncountFlag.Event:
                if (--GameManager.Instance.StageMNG.encountEvent < 0)
                    GameManager.Instance.StageMNG.encountEvent = 0;
                break;
        }
    }
}