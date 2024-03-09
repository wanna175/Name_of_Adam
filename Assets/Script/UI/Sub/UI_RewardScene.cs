using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RewardScene : MonoBehaviour
{
    [SerializeField] private List<UI_RewardUnit> _rewardUnitList;
    [SerializeField] private TMP_Text _darkEssenceResult;
    [SerializeField] private GameObject _rewardUnitPrefab;
    [SerializeField] private Transform _unitScrollViewGrid;

    public bool IsFadeEnd = false;

    public void Init(List<DeckUnit> afterBattleEndUnits)
    {
        this.GetComponent<FadeController>().StartFadeIn();

        int difference = GameManager.Data.DarkEssense - BattleManager.Data.BattlePrevDarkEssence;
        _darkEssenceResult.text = (difference >= 0) ? "+" + difference.ToString() : difference.ToString();

        for (int i = 0; i < afterBattleEndUnits.Count; i++)
        {
            if (BattleManager.Data.BattlePrevUnitDict.TryGetValue(afterBattleEndUnits[i].UnitID, out RewardUnit prevUnit))
            {
                //기존에 있던 유닛
                int faithDifference = prevUnit.Faith - afterBattleEndUnits[i].DeckUnitStat.FallCurrentCount;

                SetContent(i, prevUnit, 4 - afterBattleEndUnits[i].DeckUnitStat.FallCurrentCount, faithDifference, UnitState.Default);
                BattleManager.Data.BattlePrevUnitDict.Remove(afterBattleEndUnits[i].UnitID);
            }
            else
            {
                //새로 플레이어 덱에 들어온 유닛
                RewardUnit newUnit = new(afterBattleEndUnits[i].Data.Name, 0, afterBattleEndUnits[i].Data.CorruptPortraitImage);

                int faithDifference = afterBattleEndUnits[i].Data.RawStat.FallMaxCount - afterBattleEndUnits[i].Data.RawStat.FallCurrentCount - afterBattleEndUnits[i].DeckUnitStat.FallCurrentCount;
                SetContent(i, newUnit, 4 - afterBattleEndUnits[i].DeckUnitStat.FallCurrentCount, faithDifference, UnitState.New);
            }
        }

        int idx = afterBattleEndUnits.Count;
        foreach (RewardUnit rewardUnit in BattleManager.Data.BattlePrevUnitDict.Values)//죽은 유닛
        {
            RewardUnit deadUnit = new(rewardUnit.Name, 0, rewardUnit.Image);
            SetContent(idx++, deadUnit, 0, 0, UnitState.Dead);
        }

        SetFadeIn(afterBattleEndUnits.Count + BattleManager.Data.BattlePrevUnitDict.Count);
        BattleManager.Data.BattlePrevUnitDict.Clear();
    }

    public void SetContent(int idx, RewardUnit rewardUnit, int currentFaith, int faithDifference, UnitState unitState)
    {
        if (idx > _rewardUnitList.Count-1)
        {
            UI_RewardUnit newObject = GameObject.Instantiate(_rewardUnitPrefab  , _unitScrollViewGrid).GetComponent<UI_RewardUnit>();
            _rewardUnitList.Add(newObject);
        }

        _rewardUnitList[idx].gameObject.SetActive(true);
        _rewardUnitList[idx].Init(rewardUnit, currentFaith, faithDifference, unitState);
    }

    public void SetFadeIn(int idx)
    {
        StartCoroutine(ContentFadeIn(idx));
    }

    private IEnumerator ContentFadeIn(int cnt)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        for (int i = 0; i < cnt; i++)
        {
            _rewardUnitList[i].FadeIn();
            yield return wait;
        }

        IsFadeEnd = true;
        yield return null;
    } 

    public void EndFadeIn()
    {
        if (!IsFadeEnd)
        {
            for (int i = 0; i < _rewardUnitList.Count; i++)
                _rewardUnitList[i].EndFadeIn();
            StopAllCoroutines();
            IsFadeEnd = true;
        }
    }
}
