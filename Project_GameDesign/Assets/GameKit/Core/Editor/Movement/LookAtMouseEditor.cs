using UnityEditor;

[CustomEditor(typeof(LookAtMouse)), CanEditMultipleObjects]
public class LookAtMouseEditor : Editor
{
    private SerializedProperty rotationAxis;
    private SerializedProperty rotationSpeed;
    private SerializedProperty layerMask;
    private SerializedProperty offset;

    private void OnEnable ()
    {
        rotationAxis = serializedObject.FindProperty("rotationAxis");
        rotationSpeed = serializedObject.FindProperty("rotationSpeed");
        layerMask = serializedObject.FindProperty("layerMask");
        offset = serializedObject.FindProperty("offset");
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
                EditorGUILayout.PropertyField(rotationAxis);
                EditorGUILayout.PropertyField(rotationSpeed);
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(layerMask);
                EditorGUILayout.PropertyField(offset);
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