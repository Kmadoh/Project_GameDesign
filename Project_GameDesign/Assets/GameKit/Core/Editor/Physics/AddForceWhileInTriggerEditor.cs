using UnityEditor;

[CustomEditor(typeof(AddForceWhileInTrigger)), CanEditMultipleObjects]
public class AddForceWhileInTriggerEditor : Editor
{
    private SerializedProperty addedForce;
    private SerializedProperty isLocal;
    private SerializedProperty overrideForce;
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        addedForce = serializedObject.FindProperty("addedForce");
        isLocal = serializedObject.FindProperty("isLocal");
        overrideForce = serializedObject.FindProperty("overrideForce");
        useTag = serializedObject.FindProperty("useTag");
        tagName = serializedObject.FindProperty("tagName");
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
                EditorGUILayout.PropertyField(addedForce);
                EditorGUILayout.PropertyField(isLocal);
                EditorGUILayout.PropertyField(overrideForce);
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