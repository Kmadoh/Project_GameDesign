using UnityEditor;

[CustomEditor(typeof(LoadSceneAtPoint)), CanEditMultipleObjects]
public class LoadSceneAtPointEditor : Editor
{
    private SerializedProperty sceneChoiceIndex;
    private SerializedProperty sceneToLoad;
    
    private SerializedProperty useTag;
    private SerializedProperty tagName;

    private void OnEnable ()
    {
        useTag = serializedObject.FindProperty("useTag");
        tagName = serializedObject.FindProperty("tagName");
        
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
            sceneChoiceIndex.intValue = EditorGUILayout.Popup("Scene to load : ", sceneChoiceIndex.intValue, Helper.GetSceneNames());
            sceneToLoad.stringValue = Helper.GetSceneNames()[sceneChoiceIndex.intValue];
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(useTag);
                if (useTag.boolValue)
                {
                    tagName.stringValue = EditorGUILayout.TagField("Tag : ", tagName.stringValue);
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