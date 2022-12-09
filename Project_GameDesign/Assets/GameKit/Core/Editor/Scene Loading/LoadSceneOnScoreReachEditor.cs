using UnityEditor;

[CustomEditor(typeof(LoadSceneOnScoreReach)), CanEditMultipleObjects]
public class LoadSceneOnScoreReachEditor : Editor
{
    private SerializedProperty scoreMgr;
    private SerializedProperty scoreToReach;
    private SerializedProperty compareCheck;
    
    private SerializedProperty sceneChoiceIndex;
    private SerializedProperty sceneToLoad;
    
    private void OnEnable ()
    {
        scoreMgr = serializedObject.FindProperty("scoreMgr");
        scoreToReach = serializedObject.FindProperty("scoreToReach");
        compareCheck = serializedObject.FindProperty("compareCheck");
        
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
            EditorGUILayout.PropertyField(scoreMgr);
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(scoreToReach);
                EditorGUILayout.PropertyField(compareCheck);
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