using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/Skill", order = 1)]
public class SkillSO : ScriptableObject
{
    [SerializeField] public List<EffectSO> EffectList;
    //public List<EffectSO> EffectList => _EffectList;

    // 이펙트리스트 안의 이펙트들을 순서대로 실행
    public void use(BattleUnit ch)
    {
        CoroutineHandler.Start_Coroutine(EffectUse(ch), EffectList.Count * 0.5f);
    }

    IEnumerator EffectUse(BattleUnit ch)
    {
        for (int i = 0; i < EffectList.Count; i++)
        {
            EffectList[i].Effect(ch);

            yield return new WaitForSeconds(0.5f);
        }
    }
}