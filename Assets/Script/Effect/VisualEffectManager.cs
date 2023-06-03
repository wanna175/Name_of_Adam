using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimEffects
{
    VisualEffect,
    Corruption,
}

public class VisualEffectManager
{
    GameObject root;
    Dictionary<AnimEffects, Queue<GameObject>> EffectQueue;
    Dictionary<AnimEffects, string> AnimEffectNames;

    public VisualEffectManager()
    {
        if (!GameObject.Find("VisualEffectContainer"))
        {
            EffectQueue = new Dictionary<AnimEffects, Queue<GameObject>>();
            AnimEffectNames = new Dictionary<AnimEffects, string>();

            root = new GameObject { name = "VisualEffectContainer" };
            root.transform.parent = GameManager.Instance.transform;

            foreach (AnimEffects effect in Enum.GetValues(typeof(AnimEffects)))
            {
                EffectQueue.Add(effect, new Queue<GameObject>());
                AnimEffectNames.Add(effect, effect.ToString());
            }
        }

        for (int i = 0; i < 5; i++)
        {
            CreateEffect(AnimEffects.VisualEffect);
            CreateEffect(AnimEffects.Corruption);
        }
    }
    
    public GameObject StartVisualEffect(AnimationClip clip, Vector3 position)
    {
        if (EffectQueue.Count == 0)
            CreateEffect(AnimEffects.VisualEffect);

        GameObject go = EffectQueue[AnimEffects.VisualEffect].Dequeue();
        go.transform.position = position;
        Animator _animator = go.GetComponent<Animator>();

        RuntimeAnimatorController myController = _animator.runtimeAnimatorController;
        AnimatorOverrideController myOverrideController = myController as AnimatorOverrideController;
        if (myOverrideController != null)
            myController = myOverrideController.runtimeAnimatorController;

        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = myController;
        overrideController["VisualEffect"] = clip;

        _animator.runtimeAnimatorController = overrideController;
        go.SetActive(true);
        
        return go;
    }

    public GameObject StartCorruptionEffect(BattleUnit unit, Vector3 position)
    {
        if (EffectQueue.Count == 0)
            CreateEffect(AnimEffects.Corruption);

        GameObject go = EffectQueue[AnimEffects.Corruption].Dequeue();
        go.GetComponent<Corruption>().Init(unit, BattleManager.Instance.StigmaSelectEvent);
        go.transform.position = position;

        return go;
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
