//using UnityEngine;
//using UnityEditor;


//[CustomEditor(typeof(EndGameRequirements))]
//public class EndGameRequirementsEditor : Editor
//{
//    EndGameRequirements endGameRequirements;

//    private void OnEnable()
//    {
//        endGameRequirements = target as EndGameRequirements;
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        EditorGUI.BeginChangeCheck();
//        endGameRequirements.SetIndex = GUILayout.Toolbar(endGameRequirements.SetIndex, new string[] { "Odd", "Even", "Boss" });
//        EditorGUI.EndChangeCheck();
//    }
//}
