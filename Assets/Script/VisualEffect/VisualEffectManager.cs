using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectManager
{
    GameObject root;
    Queue<GameObject> EffectQueue;

    public VisualEffectManager()
    {
        if (!GameObject.Find("VisualEffectContainer"))
        {
            root = new GameObject { name = "VisualEffectContainer" };
            root.transform.parent = GameManager.Instance.transform;
            EffectQueue = new Queue<GameObject>();
        }
    }
    
    public GameObject StartVisualEffect(AnimationClip clip, Vector3 position)
    {
        GameObject go;
        Animator _animator;

        if (EffectQueue.Count == 0)
            go = GameManager.Resource.Instantiate("VisualEffect", root.transform);
        else
            go = EffectQueue.Dequeue();

        _animator = go.GetComponent<Animator>();
        go.transform.position = position;
        
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

    public void RestoreVisualEffect(GameObject go)
    {
        EffectQueue.Enqueue(go);
        go.SetActive(false);
    }
}
