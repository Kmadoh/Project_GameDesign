using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveTowards)), CanEditMultipleObjects]
public class MoveTowardsEditor : Editor
{
    private SerializedProperty targetTransform;
    private SerializedProperty targetTag;
    private SerializedProperty lookAtTarget;
    private SerializedProperty moveSpeed;
    private SerializedProperty minDistance;
    private SerializedProperty rotationSpeed;

    private void OnEnable ()
    {
        targetTransform = serializedObject.FindProperty("targetTransform");
        targetTag = serializedObject.FindProperty("targetTag");
        lookAtTarget = serializedObject.FindProperty("lookAtTarget");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        minDistance = serializedObject.FindProperty("minDistance");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
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
                EditorGUILayout.PropertyField(targetTransform);

                targetTag.stringValue = EditorGUILayout.TagField("Target Tag", targetTag.stringValue);
            }
            EditorGUILayout.EndVertical();
            

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(lookAtTarget);
                if (lookAtTarget.boolValue)
                {
                    EditorGUILayout.PropertyField(rotationSpeed);
                }
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(moveSpeed);
                EditorGUILayout.PropertyField(minDistance);
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