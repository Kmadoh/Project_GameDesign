using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DestroyOnCollide)), CanEditMultipleObjects]
public class DestroyOnCollideEditor : Editor
{
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        useTag = serializedObject.FindProperty("useTag");
        tagName = serializedObject.FindProperty("tagName");
    }

    public override void OnInspectorGUI()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
        {
            EditorGUILayout.PropertyField(useTag);

            if (useTag.boolValue)
            {
                tagName.stringValue = EditorGUILayout.TagField(tagName.stringValue);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}