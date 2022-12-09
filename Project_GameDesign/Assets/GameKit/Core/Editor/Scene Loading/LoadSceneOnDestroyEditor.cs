using UnityEditor;

[CustomEditor(typeof(LoadSceneOnDestroy))]
public class LoadSceneOnDestroyEditor : Editor
{
    private SerializedProperty sceneChoiceIndex;
    private SerializedProperty sceneToLoad;
    private void OnEnable ()
    {
        sceneChoiceIndex = serializedObject.FindProperty("sceneChoiceIndex");
        sceneToLoad = serializedObject.FindProperty("sceneToLoad");
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
                sceneChoiceIndex.intValue = EditorGUILayout.Popup("Scene To Load : ", sceneChoiceIndex.intValue, Helper.GetSceneNames());
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