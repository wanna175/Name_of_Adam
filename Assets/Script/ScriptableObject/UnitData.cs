using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/Unit")]
public class UnitData : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private Faction _faction;
    public Faction Faction => _faction;

    [SerializeField] private Sprite _image;
    public Sprite Image => _image;

    [SerializeField] private BehaviorType _behaviorType;
    public BehaviorType BehaviorType => _behaviorType;

    [SerializeField] private Rarity _rarity;
    public Rarity Rarity => _rarity;

    public List<Vector2> Range => GetRange();

    [SerializeField] private TargetType _targetType;
    public TargetType TargetType => _targetType;

    [SerializeField] private Stat _rawStat;
    public Stat RawStat => _rawStat;

    [SerializeField] private int _manaCost;
    public int ManaCost => _manaCost;

    [SerializeField] private int _firstManaCost;
    public int FirstManaCost => _firstManaCost;

    #region RangeEditor
    const int row = 5;
    const int column = 11;

    [SerializeField] [HideInInspector] public bool[] AttackRange = new bool[row * column];

    // 공격범위를 캐릭터의 위치를 (0, 0)으로 간주하고 리스트로 반환
    public List<Vector2> GetRange()
    {
        List<Vector2> RangeList = new List<Vector2>();

        for (int i = 0; i < AttackRange.Length; i++)
        {
            if (AttackRange[i])
            {
                int x = ((i % column) - (column >> 1));
                int y = (i / column) - (row >> 1);

                Vector2 vec = new Vector2(x, y);

                RangeList.Add(vec);
            }
        }

        return RangeList;
    }
}

[CustomEditor(typeof(UnitData))]
public class RangeEditor : Editor
{
    const int row = 5;
    const int column = 11;

    UnitData _range;
    bool[] atkRange;

    private void OnEnable()
    {
        _range = target as UnitData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("공격 범위");

        atkRange = _range.AttackRange;

        for (int i = 0; i < atkRange.Length; i++)
        {
            if (i % column == 0)
                GUILayout.BeginHorizontal();

            GUI.color = Color.white;
            if (atkRange[i])
                GUI.color = Color.red;

            if (i == row * column >> 1)
                GUI.color = Color.green;


            SerializedProperty a = serializedObject.FindProperty("AttackRange").GetArrayElementAtIndex(i);
            atkRange[i] = EditorGUILayout.Toggle(atkRange[i]);
            a.boolValue = atkRange[i];

            if (i % column == column - 1)
                GUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endregion