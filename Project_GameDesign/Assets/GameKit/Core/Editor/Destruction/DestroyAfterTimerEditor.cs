using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(DestroyAfterTimer)), CanEditMultipleObjects]
public class DestroyAfterTimerEditor : Editor
{
    private SerializedProperty lifeTime;

    private void OnEnable ()
    {
        lifeTime = serializedObject.FindProperty("lifeTime");
    }

    public override void OnInspectorGUI()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
        {
            EditorGUILayout.PropertyField(lifeTime);
        }
        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}