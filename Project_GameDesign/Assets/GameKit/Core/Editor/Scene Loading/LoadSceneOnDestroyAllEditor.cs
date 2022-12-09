using UnityEditor;

[CustomEditor(typeof(LoadSceneOnDestroyAll)), CanEditMultipleObjects]
public class LoadSceneOnDestroyAllEditor : Editor
{
    private SerializedProperty searchByTag;
    private SerializedProperty tagName;
    private SerializedProperty entities;
    private SerializedProperty sceneChoiceIndex;
    private SerializedProperty sceneToLoad;

    private void OnEnable ()
    {
        entities = serializedObject.FindProperty("entities");
        searchByTag = serializedObject.FindProperty("searchByTag");
        tagName = serializedObject.FindProperty("tagName");
        sceneChoiceIndex = serializedObject.FindProperty("sceneChoiceIndex");
        sceneToLoad = serializedObject.FindProperty("sceneToLoad");
    }

    private void OnValidate()
    {
       
    }

    public override void OnInspectorGUI ()
    { 
        UIHelper.InitializeStyles();
        
        serializedObject.Update();
        
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.PropertyField(entities);

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(searchByTag);

                if (searchByTag.boolValue)
                {
                    tagName.stringValue = EditorGUILayout.TagField("Tag : ", tagName.stringValue);
                }
            }
            EditorGUILayout.EndVertical();
            
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