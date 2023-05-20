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

        if (EffectQueue.Count == 0)
            go = GameManager.Resource.Instantiate("VisualEffect", root.transform);
        else
            go = EffectQueue.Dequeue();

        go.transform.position = position;
        
        AnimationClip originalClip = go.GetComponent<Animator>().runtimeAnimatorController.animationClips[0];
        RuntimeAnimatorController myController = go.GetComponent<Animator>().runtimeAnimatorController;
        AnimatorOverrideController myOverrideController = myController as AnimatorOverrideController;
        if (myOverrideController != null)
            myController = myOverrideController.runtimeAnimatorController;

        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = myController;
        overrideController[originalClip] = clip;

        go.GetComponent<Animator>().runtimeAnimatorController = overrideController;
        go.SetActive(true);
        
        return go;
    }

    public void RestoreVisualEffect(GameObject go)
    {
        EffectQueue.Enqueue(go);
        go.SetActive(false);
    }
}
