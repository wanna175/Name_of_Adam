﻿using System.Collections;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);

        if (resource == null)
        {
            Debug.Log($"Failed to load Resource : {path}");
            return null;
        }

        return resource;
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);

        if (resources == null || resources.Length == 0)
        {
            Debug.Log($"Failed to load Resource : {path}");
            return null;
        }

        return resources;
    }

    public T LoadSO<T>(string path) where T : Object { return Load<T>($"ScriptObjects/{path}"); }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        
        // 이름에 (Clone) 삭제
        GameObject go = Object.Instantiate(prefab, parent);
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);
        
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;
        Object.Destroy(go);
    }
}