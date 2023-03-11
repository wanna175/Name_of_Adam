using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/Unit")]
public class UnitDataSO : ScriptableObject
{
    // 변경되는 값들

    [SerializeField] private Stat _rawStat;
    public Stat RawStat => _rawStat;

    [SerializeField] private List<Passive> _stigma;
    public List<Passive> Stigma => _stigma;

    // 변경되지않는 값들

    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private string _description;
    public string Description => _description;

    [SerializeField] private Faction _faction;
    public Faction Faction => _faction;

    [SerializeField] private Rarity _rarity;
    public Rarity Rarity => _rarity;

    [SerializeField] private Sprite _image;
    public Sprite Image => _image;

    [SerializeField] private int _darkEssenseDrop;
    public int DarkEssenseDrop => _darkEssenseDrop;

    [SerializeField] private int _darkEssenseCost;
    public int DarkEssenseCost => _darkEssenseCost;

    [SerializeField] private TargetType _targetType;
    public TargetType TargetType => _targetType;

    [SerializeField] private BehaviorType _behaviorType;
    public BehaviorType BehaviorType => _behaviorType;

    [SerializeField] private UnitSkill _unitSkill;
    public UnitSkill UnitSkill => _unitSkill;


    #region RangeEditor
    const int Arow = 5;
    const int Acolumn = 11;

    const int Mrow = 5;
    const int Mcolumn = 5;

    [SerializeField] [HideInInspector] private bool[] _attackRange = new bool[Arow * Acolumn];
    public bool[] AttackRange => _attackRange;
    [SerializeField] [HideInInspector] private bool[] _moveRange = new bool[Mrow * Mcolumn];
    public bool[] MoveRange => _moveRange;
    [SerializeField] [HideInInspector] private bool[] _splashRange = new bool[Arow * Acolumn];
    public bool[] SplashRange => _splashRange;

}

[CustomEditor(typeof(UnitDataSO))]
public class RangeEditor : Editor
{
    #region AttackRange

    const int Arow = 5;
    const int Acolumn = 11;

    const int Mrow = 5;
    const int Mcolumn = 5;

    UnitDataSO _range;
    bool[] atkRange;
    bool[] moveRange;
    bool[] splashRange;

    private void OnEnable()
    {
        _range = target as UnitDataSO;
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
            if (i % Acolumn == 0)
                GUILayout.BeginHorizontal();

            GUI.color = Color.white;
            if (atkRange[i])
                GUI.color = Color.red;

            if (i == Arow * Acolumn >> 1)
                GUI.color = Color.green;


            SerializedProperty a = serializedObject.FindProperty("_attackRange").GetArrayElementAtIndex(i);
            atkRange[i] = EditorGUILayout.Toggle(atkRange[i]);
            a.boolValue = atkRange[i];

            if (i % Acolumn == Acolumn - 1)
                GUILayout.EndHorizontal();
        }

        #endregion

        #region MoveRange

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("이동 범위");

        moveRange = _range.MoveRange;

        for (int i = 0; i < moveRange.Length; i++)
        {
            if (i % Mcolumn == 0)
                GUILayout.BeginHorizontal();

            GUI.color = Color.white;
            if (moveRange[i])
                GUI.color = Color.red;

            if (i == Mrow * Mcolumn >> 1)
                GUI.color = Color.green;


            SerializedProperty b = serializedObject.FindProperty("_moveRange").GetArrayElementAtIndex(i);
            moveRange[i] = EditorGUILayout.Toggle(moveRange[i]);
            b.boolValue = moveRange[i];

            if (i % Mcolumn == Mcolumn - 1)
                GUILayout.EndHorizontal();
        }

        #endregion


        #region SplashRange

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("광역 범위");

        splashRange = _range.SplashRange;

        for (int i = 0; i < splashRange.Length; i++)
        {
            if (i % Acolumn == 0)
                GUILayout.BeginHorizontal();

            GUI.color = Color.white;
            if (splashRange[i])
                GUI.color = Color.red;

            if (i == Arow * Acolumn >> 1)
                GUI.color = Color.green;


            SerializedProperty s = serializedObject.FindProperty("_splashRange").GetArrayElementAtIndex(i);
            splashRange[i] = EditorGUILayout.Toggle(splashRange[i]);
            s.boolValue = splashRange[i];

            if (i % Acolumn == Acolumn - 1)
                GUILayout.EndHorizontal();
        }

        #endregion

        serializedObject.ApplyModifiedProperties();
    }
}
#endregion