using UnityEditor;

[CustomEditor(typeof(PauseMenu))]
public class PauseMenuEditor : Editor
{
    private SerializedProperty pauseMenu;
    private SerializedProperty resumeButton;
    private SerializedProperty inputChoiceIndex;
    private SerializedProperty pauseInputName;
    
    private string testInput;

    private void OnEnable ()
    {
        pauseMenu = serializedObject.FindProperty("pauseMenu");
        resumeButton = serializedObject.FindProperty("resumeButton");
        inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
        pauseInputName = serializedObject.FindProperty("pauseInputName");
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
                EditorGUILayout.PropertyField(pauseMenu);
                EditorGUILayout.PropertyField(resumeButton);
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                inputChoiceIndex.intValue = EditorGUILayout.Popup("Pause Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
                pauseInputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
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