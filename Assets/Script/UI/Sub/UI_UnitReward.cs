using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UnitState
{
    NEW = 1,
    Die,
    Default
};
public class UI_UnitReward : MonoBehaviour
{
    #region 변수
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text diff;
    [SerializeField] private Image NewImage;
    [SerializeField] private Image DieImage;

    [SerializeField] private Transform _unitInfoFallGrid;
    [SerializeField] private GameObject _fallGaugePrefab;

    private FadeController fc;
    #endregion

    #region 함수
    public void Init(Sprite _unitImg, string _unitName,int cur_fall, int _diff, UnitState unitState)
    {
        if (unitState ==UnitState.NEW)
        {
            NewImage.gameObject.SetActive(true);
            DieImage.gameObject.SetActive(false);
        }
        else if (unitState == UnitState.Default)
        {
            NewImage.gameObject.SetActive(false);
            DieImage.gameObject.SetActive(false);
        }
        else
        {
            NewImage.gameObject.SetActive(false);
            DieImage.gameObject.SetActive(true);
        }
        unitImage.sprite = _unitImg;
        unitName.text = _unitName;
        diff.text = _diff.ToString();

        for (int i = 0; i < cur_fall-_diff; i++)
        {
            UI_FallUnit newObject = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallUnit>();
            newObject.SwitchCountImage(Team.Player);
            if (i >= cur_fall)
                newObject.GetComponent<UI_FallUnit>().SetAnimation();
        }

        fc = this.GetComponent<FadeController>();
    }
    public void FadeIn(float time)
    {
        time *= (float)0.3;
        fc.StartFadeIn(time);
    }
    public void EndFadeIn()
    {
        if(this.gameObject.activeInHierarchy)
            fc.EndFade();
    }

    #endregion
}
