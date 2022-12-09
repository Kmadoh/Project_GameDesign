using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomResource))]
public class CustomResourceEditor : Editor
{
    private SerializedProperty resourceID;
    private SerializedProperty resourceIcon;
    
    private SerializedProperty maxResourceAmount;
    private SerializedProperty startResourceAmount;
    
    private string testInput;

    private void OnEnable ()
    {
        resourceID = serializedObject.FindProperty("resourceID");
        resourceIcon = serializedObject.FindProperty("resourceIcon");
        maxResourceAmount = serializedObject.FindProperty("maxResourceAmount");
        startResourceAmount = serializedObject.FindProperty("startResourceAmount");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(resourceID);
                EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
                {
                    EditorGUILayout.PropertyField(resourceIcon);
                    Sprite tex = (Sprite)resourceIcon.objectReferenceValue;
                    GUILayout.Box(tex.texture);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(startResourceAmount);
                EditorGUILayout.PropertyField(maxResourceAmount);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
