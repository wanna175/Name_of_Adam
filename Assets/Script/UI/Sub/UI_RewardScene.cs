using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RewardScene : MonoBehaviour
{
    #region ����
    [SerializeField] private List<GameObject> contents;//�ϴ� 10�������� �����ϵ��� �ϴ� ������.
    [SerializeField] private TMP_Text darkness;

    [SerializeField] private GameObject content_prefab;
    [SerializeField] private Transform view_grid;
    private int count;
    public bool FadeEnd = false;
    #endregion

    #region �Լ�
    public void Init(int changeDarkness)
    {
        this.GetComponent<FadeController>().StartFadeIn();
   
        darkness.text = changeDarkness.ToString();
    }
    public void setContent(int idx, RewardUnit rewardUnit,int curFall,UnitState unitState)
    {
        if (idx >= contents.Count)
        {
            GameObject newObject = GameObject.Instantiate(content_prefab, view_grid);
            contents.Add(newObject);
        }
        contents[idx].SetActive(true);
        UI_UnitReward content = contents[idx].GetComponent<UI_UnitReward>();
        content.Init(rewardUnit.image, rewardUnit.name,curFall, rewardUnit.DarkEssence, unitState);
        
        //FadeEnd = content.FadeIn((float)idx,count);
    }
    public void EndFadeIn()
    {
        FadeEnd = true;
        for (int i = 0; i < this.count; i++)
            contents[i].GetComponent<UI_UnitReward>().EndFadeIn();
        //StopAllCoroutines();
    }
    #endregion
}
