using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTestUnit : BattleUnit
{
    private SpriteRenderer srenderer;
    BattleTestDataContainer data;

    private void Awake()
    {
        srenderer = GetComponent<SpriteRenderer>();
        data = GameObject.Find("@BattleManager").GetComponent<BattleTestManager>().dataContainer;
    }

    public void TestUnitDiedEvent()
    {
        //자신이 사망 시 체크
        if (ActiveTimingCheck(ActiveTiming.UNIT_DEAD))
        {
            return;
        }


        BattleManager.Instance.UnitDeadEvent(this);

        GameObject go = GameManager.VisualEffect.StartVisualEffect(data.DeadEffect, this.transform.position);

        go.GetComponent<SpriteRenderer>().flipX = GetFlipX();

        StartCoroutine(UnitDeadEffect());
        GameManager.Sound.Play("Dead/DeadSFX");
    }

    private IEnumerator UnitDeadEffect()
    {
        Color c = srenderer.color;

        while (c.a > 0)
        {
            c.a -= 0.01f;

            srenderer.color = c;

            yield return null;
        }

        BattleManager.Instance.GetComponent<TestUnitSpawner>().RrestoreUnit(gameObject);
    }

    public void TestUnitFallEvent()
    {
        //타락 시 체크
        if (ActiveTimingCheck(ActiveTiming.FALLED))
        {
            return;
        }

        //타락 이벤트 시작
        GameManager.Sound.Play("UI/FallSFX/Fall");

        GameObject go = GameManager.Resource.Instantiate("Effect/Corruption");
        go.GetComponent<Corruption>().Init(this, BattleManager.Instance.StigmaSelectEvent);
        go.GetComponent<Animator>().runtimeAnimatorController = data.FallEffect;
        go.transform.position = transform.position;
    }

    private bool ActiveTimingCheck(ActiveTiming activeTiming, BattleUnit receiver = null, int num = 0)
    {
        bool skipNextAction = false;

        foreach (Stigma stigma in StigmaList)
        {
            if (stigma.ActiveTiming == activeTiming)
            {
                stigma.Use(this, receiver);
            }
        }

        foreach (Buff buff in Buff.CheckActiveTiming(activeTiming))
        {
            buff.SetValue(num);
            skipNextAction = buff.Active(this, receiver);
        }

        Buff.CheckCountDownTiming(activeTiming);

        BattleUnitChangedStat = Buff.GetBuffedStat();

        return skipNextAction;
    }

}