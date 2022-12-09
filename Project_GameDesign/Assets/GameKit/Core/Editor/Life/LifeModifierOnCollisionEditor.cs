using UnityEditor;

[CustomEditor(typeof(LifeModifierOnCollision)), CanEditMultipleObjects]
public class LifeModifierOnCollisionEditor : Editor
{
    private SerializedProperty lifeAmountChanged;
    private SerializedProperty selfDestroyAfter;
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        lifeAmountChanged = serializedObject.FindProperty("lifeAmountChanged");
        selfDestroyAfter = serializedObject.FindProperty("selfDestroyAfter");
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
                EditorGUILayout.PropertyField(selfDestroyAfter);
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