using UnityEditor;

[CustomEditor(typeof(LoadSceneOnTrigger))]
public class LoadSceneOnTriggerEditor : Editor
{
    private SerializedProperty useTag;
    private SerializedProperty tagName;
    
    private SerializedProperty sceneChoiceIndex;
    private SerializedProperty sceneToLoad;
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
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                sceneChoiceIndex.intValue = EditorGUILayout.Popup("Scene To Load : ", sceneChoiceIndex.intValue, Helper.GetSceneNames());
                sceneToLoad.stringValue = Helper.GetSceneNames()[sceneChoiceIndex.intValue];
            }
            EditorGUILayout.EndVertical();
			
            EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
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