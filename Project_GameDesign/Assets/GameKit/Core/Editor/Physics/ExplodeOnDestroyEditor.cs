using UnityEditor;

[CustomEditor(typeof(ExplodeOnDestroy)), CanEditMultipleObjects]
public class ExplodeOnDestroyEditor : Editor
{
    private SerializedProperty damage;
    private SerializedProperty range;
    private SerializedProperty bumpForce;
    private SerializedProperty upwardsModifier;
    private SerializedProperty explosionLayerMask;
    private void OnEnable ()
    {
        damage = serializedObject.FindProperty("damage");
        range = serializedObject.FindProperty("range");
        bumpForce = serializedObject.FindProperty("bumpForce");
        upwardsModifier = serializedObject.FindProperty("upwardsModifier");
        explosionLayerMask = serializedObject.FindProperty("explosionLayerMask");
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
                EditorGUILayout.PropertyField(damage);
                EditorGUILayout.PropertyField(range);
                EditorGUILayout.PropertyField(explosionLayerMask);
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(bumpForce);
                EditorGUILayout.PropertyField(upwardsModifier);
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