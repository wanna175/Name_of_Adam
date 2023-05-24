using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PassiveManager
{
    public enum PassiveManageType
    {
        Entire,
        Unique,
        Common,
    }

    private Dictionary<PassiveManageType, List<Passive>> _passiveDict = new Dictionary<PassiveManageType, List<Passive>>();
    public Dictionary<PassiveManageType, List<Passive>> PassiveDict => _passiveDict;

    public PassiveManager()
    {
        LoadPassiveList();
    }

    private void LoadPassiveList()
    {
        List<Passive> common = new List<Passive>();
        List<Passive> unique = new List<Passive>();
        List<Passive> entire = new List<Passive>();

        string path = "Assets/Resources/Prefabs/Passive";
        string[] files = Directory.GetFiles(path, "*.prefab");
            
        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            if (fileName == "Passive")
                continue;

            GameObject go = GameManager.Resource.Load<GameObject>("Prefabs/Passive/" + fileName);
            Passive passive = go.GetComponent<Passive>();

            if (passive.IsSpecial)
                unique.Add(passive);
            else
                common.Add(passive);

            entire.Add(passive);
        }

        PassiveDict.Add(PassiveManageType.Common, common);
        PassiveDict.Add(PassiveManageType.Unique, unique);
        PassiveDict.Add(PassiveManageType.Entire, entire);
    }

    public Passive GetRandomPassive(PassiveManageType passiveType = PassiveManageType.Common)
    {
        int randNum = Random.Range(0, PassiveDict[passiveType].Count);
        return PassiveDict[passiveType][randNum];
    }
}