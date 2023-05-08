using UnityEngine;
using UnityEditor;


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