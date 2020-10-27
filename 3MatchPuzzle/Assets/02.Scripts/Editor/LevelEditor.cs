using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    Level level;

    private void OnEnable()
    {
        level = target as Level;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle bStyle = new GUIStyle("Button");
        bStyle.alignment = TextAnchor.MiddleCenter;
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("----------------------------");
        int index = 0;
        for (int i = 0; i < 9; i++)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < 9; j++)
            {
                level.Tile[index] = EditorGUILayout.IntField(level.Tile[index], bStyle, GUILayout.Width(20), GUILayout.Height(20));
                index++;
            }
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("------타일 세팅-------");
        EditorGUILayout.LabelField("0. 일반 블록");
        EditorGUILayout.LabelField("1. 콘크리트 블록");
        EditorGUILayout.LabelField("2. 고압분사기(세로)");
        EditorGUILayout.LabelField("3. 고압분사기(가로)");
        EditorGUILayout.LabelField("4. 먼지블록");
        EditorGUILayout.LabelField("5. 십자나무 꽃 블록");
        EditorGUILayout.LabelField("6. 시든 꽃");
        EditorGUILayout.LabelField("7. 생명의 민들레");
    }
}
