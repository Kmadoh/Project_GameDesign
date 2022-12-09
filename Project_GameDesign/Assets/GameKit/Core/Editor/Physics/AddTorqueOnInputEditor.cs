using UnityEditor;

[CustomEditor(typeof(AddTorqueOnInput)), CanEditMultipleObjects]
public class AddTorqueOnInputEditor : Editor
{
    private SerializedProperty rigid;
    private SerializedProperty forceMode;
    private SerializedProperty isTorqueLocal;
    private SerializedProperty torque;
    private SerializedProperty inputChoiceIndex;
    private SerializedProperty inputName;

    private void OnEnable ()
    {
        rigid = serializedObject.FindProperty("rigid");
        
        forceMode = serializedObject.FindProperty("forceMode");
        isTorqueLocal = serializedObject.FindProperty("isTorqueLocal");
        torque = serializedObject.FindProperty("torque");
        
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
            EditorGUILayout.PropertyField(rigid);

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(forceMode);
                EditorGUILayout.PropertyField(torque);
                EditorGUILayout.PropertyField(isTorqueLocal);
            }
            EditorGUILayout.EndVertical();
			
            EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
            {
                inputChoiceIndex.intValue = EditorGUILayout.Popup("Input Axis : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
                inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
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