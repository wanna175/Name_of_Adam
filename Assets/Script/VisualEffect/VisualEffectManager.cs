using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimEffects
{
    VisualEffect,
    Corruption,
}

public class VisualEffectManager : MonoBehaviour
{
    Transform root;
    Dictionary<AnimEffects, Queue<GameObject>> EffectQueue;
    Dictionary<AnimEffects, string> AnimEffectNames;

    public void Init()
    {
        root = transform;

        EffectQueue = new();
        AnimEffectNames = new();

        foreach (AnimEffects effect in Enum.GetValues(typeof(AnimEffects)))
        {
            EffectQueue.Add(effect, new Queue<GameObject>());
            AnimEffectNames.Add(effect, effect.ToString());
        }

        for (int i = 0; i < 5; i++)
        {
            CreateEffect(AnimEffects.VisualEffect);
            CreateEffect(AnimEffects.Corruption);
        }
    }

    public GameObject StartVisualEffect(string str, Vector3 position)
    {
        AnimationClip clip = GameManager.Resource.Load<AnimationClip>(str);

        return StartVisualEffect(clip, position);
    }
    public GameObject StartVisualEffect(AnimationClip clip, Vector3 position)
    {
        GameObject go;
        try
        {
            go = EffectQueue[AnimEffects.VisualEffect].Dequeue();
        }
        catch
        {
            CreateEffect(AnimEffects.VisualEffect);
            go = EffectQueue[AnimEffects.VisualEffect].Dequeue();
        }

        go.transform.position = position;
        Animator _animator = go.GetComponent<Animator>();

        RuntimeAnimatorController myController = _animator.runtimeAnimatorController;
        AnimatorOverrideController myOverrideController = myController as AnimatorOverrideController;
        if (myOverrideController != null)
            myController = myOverrideController.runtimeAnimatorController;

        AnimatorOverrideController overrideController = new()
        {
            runtimeAnimatorController = myController
        };
        overrideController["VisualEffect"] = clip;

        _animator.runtimeAnimatorController = overrideController;
        go.SetActive(true);

        return go;
        }

    public void StartCorruptionEffect(BattleUnit unit, Vector3 position)
    {
        if (EffectQueue.Count == 0)
            CreateEffect(AnimEffects.Corruption);

        GameObject go = EffectQueue[AnimEffects.Corruption].Dequeue();
        go.GetComponent<Corruption>().Init(unit, BattleManager.Instance.StigmaSelectEvent);
        go.transform.position = position;
    }

    public void StartStigmaEffect(Sprite sprite, Vector3 position)
    {
        AnimationClip clip = GameManager.Resource.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/StigmaEffect");
        GameObject go = StartVisualEffect(clip, position);

        go.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void StartUnitDeadEffect(Vector3 position, bool flip)
    {
        AnimationClip clip = GameManager.Resource.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UnitDeadEffect");
        GameObject go = StartVisualEffect(clip, position);

        go.GetComponent<SpriteRenderer>().flipX = flip;
    }

    public void StartFadeEffect(bool FadeIn)
    {
        GameObject go;
        AnimationClip clip;

        if (FadeIn)
            clip = GameManager.Resource.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/FadeInEffect");
        else
            clip = GameManager.Resource.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/FadeOutEffect");

        go = StartVisualEffect(clip, Vector3.zero);
        go.GetComponent<SpriteRenderer>().sprite = GameManager.Resource.Load<Sprite>("Arts/EffectAnimation/VisualEffect/Sprite/Fade");
    }

    public void StartBenedictionEffect(BattleUnit unit)
    {
        AnimationClip clip = GameManager.Resource.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/BenedictionEffect");
        GameObject go = StartVisualEffect(clip, Vector3.zero);
        go.transform.SetParent(unit.transform);
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<VisualEffect>().SetLoop(true);
    }

    private void CreateEffect(AnimEffects effect)
    {
        GameObject go = GameManager.Resource.Instantiate($"Effect/{AnimEffectNames[effect]}", root.transform);
        RestoreEffect(effect, go);
    }

    public void RestoreEffect(AnimEffects effect, GameObject go)
    {
        EffectQueue[effect].Enqueue(go);
        go.SetActive(false);
    }
}
