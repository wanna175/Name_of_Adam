using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RewardScene : MonoBehaviour
{
    [SerializeField] private List<DeckUnit> _afterBattleUnits;
    [SerializeField] private List<UI_RewardUnit> _rewardUnitList;
    [SerializeField] private TMP_Text _darkEssenceResult;
    [SerializeField] private GameObject _rewardUnitPrefab;
    [SerializeField] private Transform _unitScrollViewGrid;

    private const float _createUnitSlotDelay = 0.5f;

    public bool IsEndCreate = false;

    public void Init(List<DeckUnit> afterBattleEndUnits)
    {
        this.GetComponent<FadeController>().StartFadeIn();

        int difference = GameManager.Data.DarkEssense - BattleManager.Data.BattlePrevDarkEssence;
        _darkEssenceResult.text = (difference >= 0) ? "+" + difference.ToString() : difference.ToString();
        _afterBattleUnits = afterBattleEndUnits;

        StartCoroutine(CreateUnitSlotsWithDelay(_afterBattleUnits));
    }

    IEnumerator CreateUnitSlotsWithDelay(List<DeckUnit> afterBattleEndUnits)
    {
        // 패널 초기화
        foreach (var rewardUnit in _unitScrollViewGrid.GetComponentsInChildren<UI_RewardUnit>())
            Destroy(rewardUnit.gameObject);
        _rewardUnitList.Clear();

        var prevUnitDict = new Dictionary<int, RewardUnit>(BattleManager.Data.BattlePrevUnitDict);

        // 생성 시작
        for (int i = 0; i < afterBattleEndUnits.Count; i++)
        {
            if (prevUnitDict.TryGetValue(afterBattleEndUnits[i].UnitID, out RewardUnit prevUnit))
            {
                //기존에 있던 유닛
                SetContent(i, prevUnit, afterBattleEndUnits[i].DeckUnitTotalStat.FallMaxCount, afterBattleEndUnits[i].DeckUnitTotalStat.FallCurrentCount, UnitState.Default);

                prevUnitDict.Remove(afterBattleEndUnits[i].UnitID);
            }
            else
            {
                //새로 플레이어 덱에 들어온 유닛
                RewardUnit newUnit = new RewardUnit(afterBattleEndUnits[i].PrivateKey, afterBattleEndUnits[i].Data.Name, 0, afterBattleEndUnits[i].Data.CorruptPortraitImage);

                SetContent(i, newUnit, afterBattleEndUnits[i].DeckUnitTotalStat.FallMaxCount, afterBattleEndUnits[i].DeckUnitTotalStat.FallCurrentCount, UnitState.New);
            }

            yield return new WaitForSeconds(_createUnitSlotDelay);
        }

        int idx = afterBattleEndUnits.Count;
        foreach (RewardUnit rewardUnit in prevUnitDict.Values)//죽은 유닛
        {
            RewardUnit deadUnit = new(rewardUnit.PrivateKey, rewardUnit.Name, 0, rewardUnit.Image);
            SetContent(idx++, deadUnit, 0, 0, UnitState.Dead);

            yield return new WaitForSeconds(_createUnitSlotDelay);
        }

        IsEndCreate = true;
        BattleManager.Data.BattlePrevUnitDict.Clear();
    }

    private void CreateUnitSlots(List<DeckUnit> afterBattleEndUnits)
    {
        // 패널 초기화
        foreach (var rewardUnit in _unitScrollViewGrid.GetComponentsInChildren<UI_RewardUnit>())
            Destroy(rewardUnit.gameObject);
        _rewardUnitList.Clear();

        var prevUnitDict = new Dictionary<int, RewardUnit>(BattleManager.Data.BattlePrevUnitDict);

        // 생성 시작
        for (int i = 0; i < afterBattleEndUnits.Count; i++)
        {
            if (prevUnitDict.TryGetValue(afterBattleEndUnits[i].UnitID, out RewardUnit prevUnit))
            {
                //기존에 있던 유닛
                SetContent(i, prevUnit, afterBattleEndUnits[i].DeckUnitTotalStat.FallMaxCount, afterBattleEndUnits[i].DeckUnitTotalStat.FallCurrentCount, UnitState.Default);

                prevUnitDict.Remove(afterBattleEndUnits[i].UnitID);
            }
            else
            {
                //새로 플레이어 덱에 들어온 유닛
                RewardUnit newUnit = new RewardUnit(afterBattleEndUnits[i].PrivateKey, afterBattleEndUnits[i].Data.Name, 0, afterBattleEndUnits[i].Data.CorruptPortraitImage);

                SetContent(i, newUnit, afterBattleEndUnits[i].DeckUnitTotalStat.FallMaxCount, afterBattleEndUnits[i].DeckUnitTotalStat.FallCurrentCount, UnitState.New);
            }
        }

        int idx = afterBattleEndUnits.Count;
        foreach (RewardUnit rewardUnit in prevUnitDict.Values)//죽은 유닛
        {
            RewardUnit deadUnit = new(rewardUnit.PrivateKey, rewardUnit.Name, 0, rewardUnit.Image);
            SetContent(idx++, deadUnit, 0, 0, UnitState.Dead);
        }
    }

    public void SetContent(int idx, RewardUnit rewardUnit, int maxFaith, int currentFaith, UnitState unitState)
    {
        UI_RewardUnit newObject = GameObject.Instantiate(_rewardUnitPrefab, _unitScrollViewGrid).GetComponent<UI_RewardUnit>();
        _rewardUnitList.Add(newObject);
        _rewardUnitList[idx].gameObject.SetActive(true);
        _rewardUnitList[idx].Init(rewardUnit, maxFaith, currentFaith, unitState);
        _rewardUnitList[idx].FadeIn();

        if (idx >= 10)
            Invoke("ResetViewRect", 0.05f);
    }

    private void ResetViewRect()
    {
        var rectTr = _unitScrollViewGrid.GetComponent<RectTransform>();
        rectTr.anchoredPosition = new Vector2(0, rectTr.sizeDelta.y);
    }

    public void EndFadeIn()
    {
        if (!IsEndCreate)
        {
            CreateUnitSlots(_afterBattleUnits);
            StopAllCoroutines();

            for (int i = 0; i < _rewardUnitList.Count; i++)
                _rewardUnitList[i].EndFadeIn();

            IsEndCreate = true;
            BattleManager.Data.BattlePrevUnitDict.Clear();
        }
    }
}
