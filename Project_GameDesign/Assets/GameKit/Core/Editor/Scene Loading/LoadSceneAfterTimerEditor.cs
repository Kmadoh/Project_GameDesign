using UnityEditor;

[CustomEditor(typeof(LoadSceneAfterTimer)), CanEditMultipleObjects]
public class LoadSceneAfterTimerEditor : Editor
{
    private SerializedProperty timeBeforeLoading;
    private SerializedProperty sceneChoiceIndex;
    private SerializedProperty sceneToLoad;
    
    private void OnEnable ()
    {
        timeBeforeLoading = serializedObject.FindProperty("timeBeforeLoading");
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
                EditorGUILayout.PropertyField(timeBeforeLoading);
                sceneChoiceIndex.intValue = EditorGUILayout.Popup("Scene to load : ", sceneChoiceIndex.intValue, Helper.GetSceneNames());
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