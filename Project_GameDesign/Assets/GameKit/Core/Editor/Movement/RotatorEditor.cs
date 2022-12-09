using UnityEditor;

[CustomEditor(typeof(Rotator)), CanEditMultipleObjects]
public class RotatorEditor : Editor
{
    private SerializedProperty angularVelocity;
    private SerializedProperty useInput;
    private SerializedProperty inputChoiceIndex;
    private SerializedProperty inputName;

    private void OnEnable ()
    {
        angularVelocity = serializedObject.FindProperty("angularVelocity");
        useInput = serializedObject.FindProperty("useInput");
        inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
        inputName = serializedObject.FindProperty("inputName");
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
                EditorGUILayout.PropertyField(angularVelocity);
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(useInput);
                if (useInput.boolValue)
                {
                    inputChoiceIndex.intValue = EditorGUILayout.Popup("Input Axis : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
                    inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
                }
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