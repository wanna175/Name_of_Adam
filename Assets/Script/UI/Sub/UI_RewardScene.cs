using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RewardScene : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<GameObject> contents;
    [SerializeField] private TMP_Text darkness;

    [SerializeField] private GameObject content_prefab;
    [SerializeField] private Transform view_grid;
    private int count;
    private bool FadeEnd = false;
    public bool isEndFade => FadeEnd;
    #endregion

    #region 함수
    public void Init(int changeDarkness,int cnt)
    {
        this.count = cnt;
        if (count > contents.Count)
        {
            GameObject newObject = GameObject.Instantiate(content_prefab, view_grid);
            newObject.SetActive(false);
            contents.Add(newObject);
        }
        this.GetComponent<FadeController>().StartFadeIn();
   
        darkness.text = changeDarkness.ToString();
    }
    public void setContent(int idx, RewardUnit rewardUnit, int curFall, UnitState unitState)
    {
        /*if (idx >= contents.Count)
        {
            GameObject newObject = GameObject.Instantiate(content_prefab, view_grid);
            contents.Add(newObject);
        }*/
        contents[idx].SetActive(true);
        UI_UnitReward content = contents[idx].GetComponent<UI_UnitReward>();
        content.Init(rewardUnit.image, rewardUnit.name,curFall, rewardUnit.DarkEssence, unitState);
        
        content.FadeIn((float)idx);
    }
    public void EndFadeIn(bool isClick = true)
    {
        FadeEnd = true;
        if (isClick)
        {
            Debug.Log("페이드인 엔드!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!111");
            for (int i = 0; i < this.count; i++)
                contents[i].GetComponent<UI_UnitReward>().EndFadeIn();
        }
        //StopAllCoroutines();
    }
    #endregion
}
