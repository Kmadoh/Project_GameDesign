using UnityEditor;

[CustomEditor(typeof(TimeScaleChangeOnTrigger)), CanEditMultipleObjects]
public class TimeScaleChangeOnTriggerEditor : Editor
{
    private SerializedProperty useTag;
    private SerializedProperty tagName;
    private SerializedProperty resetTimeScaleOnLeave;
    private SerializedProperty resetTimeScaleOnDestroy;
    private SerializedProperty targetTimeScale;
    private SerializedProperty timeToReach;
    private SerializedProperty timeScaleCurve;

    private void OnEnable ()
    {
        useTag = serializedObject.FindProperty("useTag");
        tagName = serializedObject.FindProperty("tagName");
        resetTimeScaleOnLeave = serializedObject.FindProperty("resetTimeScaleOnLeave");
        resetTimeScaleOnDestroy = serializedObject.FindProperty("resetTimeScaleOnDestroy");
        
        targetTimeScale = serializedObject.FindProperty("targetTimeScale");
        timeToReach = serializedObject.FindProperty("timeToReach");
        timeScaleCurve = serializedObject.FindProperty("timeScaleCurve");
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
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(targetTimeScale);
                EditorGUILayout.PropertyField(timeToReach);
                EditorGUILayout.PropertyField(timeScaleCurve);
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(resetTimeScaleOnLeave);
                EditorGUILayout.PropertyField(resetTimeScaleOnDestroy);
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