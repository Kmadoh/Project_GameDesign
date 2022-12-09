using UnityEditor;

[CustomEditor(typeof(LifeModifierWhileInTrigger)), CanEditMultipleObjects]
public class LifeModifierWhileInTriggerEditor : Editor
{
    private SerializedProperty lifeAmountChanged;
    private SerializedProperty damageFrequency;
    private SerializedProperty resetCooldownOnLeave;
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        lifeAmountChanged = serializedObject.FindProperty("lifeAmountChanged");
        damageFrequency = serializedObject.FindProperty("damageFrequency");
        resetCooldownOnLeave = serializedObject.FindProperty("resetCooldownOnLeave");
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
                EditorGUILayout.PropertyField(lifeAmountChanged);
                EditorGUILayout.PropertyField(damageFrequency);
                EditorGUILayout.PropertyField(resetCooldownOnLeave);
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