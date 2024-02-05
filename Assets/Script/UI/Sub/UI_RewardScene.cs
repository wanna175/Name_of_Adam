using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RewardScene : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<UI_UnitReward> contents;
    [SerializeField] private TMP_Text darkness;

    [SerializeField] private GameObject content_prefab;
    [SerializeField] private Transform view_grid;
    private bool FadeEnd = false;
    public bool isEndFade => FadeEnd;
    #endregion

    #region 함수
    public void Init(int changeDarkness)
    {
        this.GetComponent<FadeController>().StartFadeIn();
        darkness.text = changeDarkness.ToString();
    }
    public void setContent(int idx, RewardUnit rewardUnit, int curFall, UnitState unitState)
    {
        if (idx > contents.Count-1)
        {
            UI_UnitReward newObject = GameObject.Instantiate(content_prefab, view_grid).GetComponent<UI_UnitReward>();
            contents.Add(newObject);
        }
        contents[idx].gameObject.SetActive(true);
        contents[idx].Init(rewardUnit.image, rewardUnit.name,curFall, rewardUnit.DarkEssence, unitState);
    }
    public void setFadeIn(int idx)
    {
        StartCoroutine(ContentFadeIn(idx));
    }
    private IEnumerator ContentFadeIn(int cnt)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        int i=0;
        while (cnt >i) {
            contents[i].FadeIn();
            i++;
            yield return wait;
        }
        FadeEnd = true;
        yield return null;
    } 
    public void EndFadeIn()
    {
        if (!FadeEnd)
        {
            for (int i = 0; i < contents.Count; i++)
                contents[i].EndFadeIn();
            StopAllCoroutines();
            FadeEnd = true;
        }
    }
    #endregion
}
