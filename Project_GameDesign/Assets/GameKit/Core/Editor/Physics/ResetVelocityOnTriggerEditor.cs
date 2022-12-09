using UnityEditor;

[CustomEditor(typeof(ResetVelocityOnTrigger)), CanEditMultipleObjects]
public class ResetVelocityOnTriggerEditor : Editor
{
    private SerializedProperty resetOwnVelocity;
    private SerializedProperty resetOthersVelocity;
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        useTag = serializedObject.FindProperty("useTag");
        tagName = serializedObject.FindProperty("tagName");
        resetOwnVelocity = serializedObject.FindProperty("resetOwnVelocity");
        resetOthersVelocity = serializedObject.FindProperty("resetOthersVelocity");
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
                EditorGUILayout.PropertyField(resetOwnVelocity);
                EditorGUILayout.PropertyField(resetOthersVelocity);
            }
            EditorGUILayout.EndVertical();
			
            EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(useTag);

                if (useTag.boolValue)
                {
                    tagName.stringValue = EditorGUILayout.TagField(tagName.stringValue);
                }
                
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}