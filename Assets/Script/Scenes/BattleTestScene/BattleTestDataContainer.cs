using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTestDataContainer : MonoBehaviour
{
    [Header("생성 유닛")]
    [SerializeField] public List<SpawnData> SpawnUnits;
    [Space(15f)]
    [Header("타락 이펙트")]
    [SerializeField] public RuntimeAnimatorController FallEffect;
    [Space(5f)]
    [Header("사망 이펙트")]
    [SerializeField] public AnimationClip DeadEffect;
    [Space(5f)]
    [Header("커스텀 이펙트")]
    [SerializeField] public AnimationClip CustomEffect;
}
