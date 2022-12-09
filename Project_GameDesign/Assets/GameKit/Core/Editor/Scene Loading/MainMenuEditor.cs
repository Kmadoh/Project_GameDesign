using UnityEditor;

[CustomEditor(typeof(MainMenu))]
public class MainMenuEditor : Editor
{
    private SerializedProperty sceneChoiceIndex;
    private SerializedProperty sceneToLoad;
    
    private void OnEnable ()
    {
        sceneChoiceIndex = serializedObject.FindProperty("sceneChoiceIndex");
        sceneToLoad = serializedObject.FindProperty("sceneToLoad");
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
                sceneChoiceIndex.intValue = EditorGUILayout.Popup("First Scene : ", sceneChoiceIndex.intValue, Helper.GetSceneNames());
                sceneToLoad.stringValue = Helper.GetSceneNames()[sceneChoiceIndex.intValue];
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