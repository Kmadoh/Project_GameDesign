using UnityEditor;

[CustomEditor(typeof(MoveTowardsClickDirection)), CanEditMultipleObjects]
public class MoveTowardsClickDirectionEditor : Editor
{
    private SerializedProperty cam;
    private SerializedProperty lockOnX;
    private SerializedProperty lockOnY;
    private SerializedProperty lockOnZ;
    private SerializedProperty clickMask;
    private SerializedProperty moveSpeed;
    private SerializedProperty useInertia;
    private SerializedProperty keepVelocity;
    private SerializedProperty displayDirection;
    
    private SerializedProperty rigid;
    private SerializedProperty lineRenderer;

    private void OnEnable ()
    {
        cam = serializedObject.FindProperty("cam");
        lockOnX = serializedObject.FindProperty("lockOnX");
        lockOnY = serializedObject.FindProperty("lockOnY");
        lockOnZ = serializedObject.FindProperty("lockOnZ");
        
        clickMask = serializedObject.FindProperty("clickMask");
        moveSpeed = serializedObject.FindProperty("moveSpeed");
        useInertia = serializedObject.FindProperty("useInertia");
        keepVelocity = serializedObject.FindProperty("keepVelocity");
        displayDirection = serializedObject.FindProperty("displayDirection");
        rigid = serializedObject.FindProperty("rigid");
        lineRenderer = serializedObject.FindProperty("lineRenderer");
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
                EditorGUILayout.PropertyField(rigid);
                EditorGUILayout.PropertyField(cam);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(displayDirection);
                if (displayDirection.boolValue)
                {
                    EditorGUILayout.PropertyField(lineRenderer);
                }
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(lockOnX);
                EditorGUILayout.PropertyField(lockOnY);
                EditorGUILayout.PropertyField(lockOnZ);
                EditorGUILayout.PropertyField(clickMask);
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(moveSpeed);
                EditorGUILayout.PropertyField(useInertia);
                EditorGUILayout.PropertyField(keepVelocity);
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