using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LookAtTarget)), CanEditMultipleObjects]
public class LookAtTargetEditor : Editor
{
    private SerializedProperty targetTransform;
    private SerializedProperty rotationSpeed;
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        useTag = serializedObject.FindProperty("useTag");
        targetTransform = serializedObject.FindProperty("targetTransform");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        tagName = serializedObject.FindProperty("tagName");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(useTag);

                if (useTag.boolValue)
                {
                    tagName.stringValue = EditorGUILayout.TagField(tagName.stringValue);
                }
                else
                {
                    EditorGUILayout.LabelField("Target", GUILayout.MaxWidth(70f));
                    targetTransform.objectReferenceValue = (Transform)EditorGUILayout.ObjectField(targetTransform.objectReferenceValue, typeof(Transform), true);
                }
                
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(rotationSpeed);
        }
        EditorGUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}