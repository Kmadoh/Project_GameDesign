using UnityEditor;

[CustomEditor(typeof(StartingImpulse)), CanEditMultipleObjects]
public class StartingImpulseEditor : Editor
{
    private SerializedProperty impulsePower;
    private SerializedProperty isLocal;
    private SerializedProperty rigid;

    private void OnEnable ()
    {
        impulsePower = serializedObject.FindProperty("impulsePower");
        isLocal = serializedObject.FindProperty("isLocal");
        rigid = serializedObject.FindProperty("rigid");
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
                EditorGUILayout.PropertyField(rigid);
                EditorGUILayout.PropertyField(impulsePower);
                EditorGUILayout.PropertyField(isLocal);
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