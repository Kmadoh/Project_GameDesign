using UnityEditor;

[CustomEditor(typeof(LifeToAlpha))]
public class LifeToAlphaEditor : Editor
{
    private SerializedProperty lifeToTrack;
    private SerializedProperty imageToTweak;
    private SerializedProperty invert;

    private void OnEnable ()
    {
        lifeToTrack = serializedObject.FindProperty("lifeToTrack");
        imageToTweak = serializedObject.FindProperty("imageToTweak");
        invert = serializedObject.FindProperty("invert");
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
                EditorGUILayout.PropertyField(lifeToTrack);
                EditorGUILayout.PropertyField(imageToTweak);
                EditorGUILayout.PropertyField(invert);
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