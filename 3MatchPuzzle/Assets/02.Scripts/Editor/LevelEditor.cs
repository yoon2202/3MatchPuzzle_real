using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    Level level;
    SerializedProperty Dots;
    SerializedProperty Blocks;

    private void OnEnable()
    {
        level = target as Level;
        Dots = serializedObject.FindProperty("dots");
        Blocks = serializedObject.FindProperty("Blocks");
    }

    public override void OnInspectorGUI()
    {
        GUIStyle bStyle = new GUIStyle("Button");
        bStyle.alignment = TextAnchor.MiddleCenter;
        //base.OnInspectorGUI();

        EditorGUILayout.PropertyField(Dots, new GUIContent("일반블록"));
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(30);

        EditorGUILayout.LabelField("---- 목표 설정 ----");
        level.Score = EditorGUILayout.IntField("스코어 점수", level.Score);
        level.Timer = EditorGUILayout.IntField("시간 제한", level.Timer);

        EditorGUILayout.PropertyField(Blocks, new GUIContent("출현방해 블록 (최대 3개까지 가능)"));
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);


        EditorGUILayout.LabelField("---- 레벨 디자인 ----");
        int index = 0;
        if (level != null && level.Tile.Length > 0)
        {
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
        }
        GUILayout.Space(10);

        if (GUILayout.Button("Stage Save", GUILayout.Width(200), GUILayout.Height(30)))
        {
            EditorUtility.SetDirty(target);
            Debug.Log("저장 완료");
        }
        GUILayout.Space(10);

        EditorGUILayout.LabelField("------블록 목록-------");
        EditorGUILayout.LabelField("0. 기본 블록");
        EditorGUILayout.LabelField("1. 공백");
        EditorGUILayout.LabelField("2. 콘크리트 블록");
        EditorGUILayout.LabelField("3. 고압분사기(세로)");
        EditorGUILayout.LabelField("4. 고압분사기(가로)");

    }
}
