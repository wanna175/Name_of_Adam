using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NextStageSign : MonoBehaviour
{
    public int a = 0;
    [SerializeField] List<Image> Container;

    private void Start()
    {
        SetNextStageSign();
    }

    public void SetNextStageSign()
    {
        List<EncountStage> StageList = GameManager.Instance.StageMNG.GetStage();
        EncountStage[] StageArray = GameManager.Instance.StageMNG.GetStageArray();

        if(StageList.Count != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                int rand = Random.Range(0, StageList.Count);

                StageArray[i] = StageList[rand];
                StageList.Remove(StageList[rand]);
            }

            DrawSign(false);
        }
        else
        {
            // boss
            Debug.Log("보스 등장");
            StageArray[1] = EncountStage.Boss;
            DrawSign(true);
        }
    }

    void DrawSign(bool isBoss)
    {
        if (isBoss)
        {
            Container[0].gameObject.SetActive(false);
            Container[2].gameObject.SetActive(false);

            Container[1].color = Color.black;
            return;
        }
        Container[0].gameObject.SetActive(true);
        Container[2].gameObject.SetActive(true);

        for (int i = 0; i < Container.Count; i++)
        {
            switch (GameManager.Instance.StageMNG.StageArray[i])
            {
                case EncountStage.Monster:
                    Container[i].color = Color.red;
                    break;
                case EncountStage.Elite:
                    Container[i].color = Color.blue;
                    break;
                case EncountStage.Event:
                    Container[i].color = Color.yellow;
                    break;
            }
        }
    }
}