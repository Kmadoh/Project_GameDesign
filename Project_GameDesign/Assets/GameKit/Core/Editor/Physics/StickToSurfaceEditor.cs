using UnityEditor;

[CustomEditor(typeof(StickToSurface)), CanEditMultipleObjects]
public class StickToSurfaceEditor : Editor
{
    private SerializedProperty stickOnlyOnTop;
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        stickOnlyOnTop = serializedObject.FindProperty("stickOnlyOnTop");
        useTag = serializedObject.FindProperty("useTag");
        tagName = serializedObject.FindProperty("tagName");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical("MainStyle");
        {
            EditorGUILayout.PropertyField(stickOnlyOnTop);
            
            EditorGUILayout.BeginHorizontal("SubStyle1");
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