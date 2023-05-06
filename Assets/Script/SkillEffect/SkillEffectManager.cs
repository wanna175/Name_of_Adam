using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectManager
{
    GameObject root;
    Queue<GameObject> EffectQueue;

    public SkillEffectManager()
    {
        root = new GameObject { name = "SkillEffectContainer" };
        EffectQueue = new Queue<GameObject>();
    }
    
    public GameObject StartSkillEffect(RuntimeAnimatorController _animator, Vector3 position)
    {
        GameObject go;

        if (EffectQueue.Count == 0)
            go = GameManager.Resource.Instantiate("SkillEffect", root.transform);
        else
            go = EffectQueue.Dequeue();

        go.transform.position = position;
        go.GetComponent<Animator>().runtimeAnimatorController = _animator;
        go.SetActive(true);
        go.GetComponent<Animator>().Play("Effect");
        
        return go;
    }

    public void RestoreSkillEffect(GameObject go)
    {
        go.SetActive(false);
        EffectQueue.Enqueue(go);
    }
}
