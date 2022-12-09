using UnityEditor;

[CustomEditor(typeof(EventBase)), CanEditMultipleObjects]
public class EventBaseEditor : Editor
{
    private SerializedProperty loop;
    private SerializedProperty triggeredEvents;

    private void OnEnable ()
    {
        loop = serializedObject.FindProperty("loop");
        triggeredEvents = serializedObject.FindProperty("triggeredEvents");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.HelpBox("This script is mostly used as a base to trigger Events, you might want to use the other Event scripts instead.", MessageType.Warning, true);
            EditorGUILayout.PropertyField(loop);
            
            EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(triggeredEvents);
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