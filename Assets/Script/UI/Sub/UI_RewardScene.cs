using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RewardScene : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<GameObject> contents;//일단 10개까지만 가능하도록 일단 만들자.
    [SerializeField] private TMP_Text darkness;

    private int count=10;
    public bool FadeEnd = false;
    #endregion

    #region 함수
    public void Init(int count,int changeDarkness)
    {
        this.GetComponent<FadeController>().StartFadeIn();
        if (count > 10) this.count = 10;
        else this.count = count;
        for(int i = 0; i < this.count; ++i)
        {
            contents[i].SetActive(true);
        }
        darkness.text = changeDarkness.ToString();
    }
    public void setContent(int idx, RewardUnit rewardUnit)
    {
        UI_UnitReward content = contents[idx].GetComponent<UI_UnitReward>();
        content.Init(rewardUnit.image, rewardUnit.name, rewardUnit.DarkEssence);
        content.FadeIn(idx);
        
    }
    public void EndFadeIn()
    {
        for (int i = 0; i < this.count; i++)
            contents[i].GetComponent<UI_UnitReward>().EndFadeIn();
        FadeEnd = true;
    }
    #endregion
}
