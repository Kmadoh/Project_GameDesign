using UnityEditor;

[CustomEditor(typeof(TimeScaleChangeOnInput)), CanEditMultipleObjects]
public class TimeScaleChangeOnInputEditor : Editor
{
    private SerializedProperty revertMode;
    private SerializedProperty targetTimeScale;
    private SerializedProperty timeToReach;
    private SerializedProperty timeScaleCurve;
    private SerializedProperty revertTimerDuration;
    private SerializedProperty inputChoiceIndex;
    private SerializedProperty inputName;

    private void OnEnable ()
    {
        revertMode = serializedObject.FindProperty("revertMode");
        targetTimeScale = serializedObject.FindProperty("targetTimeScale");
        timeToReach = serializedObject.FindProperty("timeToReach");
        timeScaleCurve = serializedObject.FindProperty("timeScaleCurve");
        revertTimerDuration = serializedObject.FindProperty("revertTimerDuration");
        inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
        inputName = serializedObject.FindProperty("inputName");
    }

    private void OnValidate()
    {
        UIHelper.InitializeStyles();
    }

    public override void OnInspectorGUI ()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
                inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
                EditorGUILayout.PropertyField(revertMode);
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(targetTimeScale);
                EditorGUILayout.PropertyField(timeToReach);
                EditorGUILayout.PropertyField(timeScaleCurve);
            }
            EditorGUILayout.EndVertical();

            if (revertMode.intValue == (int)TimeScaleChangeOnInput.RevertMode.Timer)
            {
                EditorGUILayout.BeginVertical(UIHelper.subStyle1);
                {
                    EditorGUILayout.PropertyField(revertTimerDuration);
                }
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}