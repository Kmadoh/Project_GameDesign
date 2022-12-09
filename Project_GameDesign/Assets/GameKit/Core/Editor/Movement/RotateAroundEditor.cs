using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RotateAround)), CanEditMultipleObjects]
public class RotateAroundEditor : Editor
{
    [Tooltip("Used to check public variables from the target class")]
    private RotateAround myObject;
    private SerializedObject soTarget;

    private SerializedProperty targetTransform;
    private SerializedProperty rotateAxis;
    private SerializedProperty rotateSpeed;
    private SerializedProperty useInput;

    private void OnEnable ()
    {
        myObject = (RotateAround)target;
        soTarget = new SerializedObject(target);

        targetTransform = soTarget.FindProperty("targetTransform");
        rotateAxis = soTarget.FindProperty("rotateAxis");
        rotateSpeed = soTarget.FindProperty("rotateSpeed");
        useInput = soTarget.FindProperty("useInput");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.PropertyField(targetTransform);

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(rotateAxis);
                EditorGUILayout.PropertyField(rotateSpeed);
                
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(useInput);
                if (myObject.useInput)
                {
                    myObject.inputChoiceIndex = EditorGUILayout.Popup("Input Axis : ", myObject.inputChoiceIndex, Helper.GetInputAxes());
                    myObject.inputName = Helper.GetInputAxes()[myObject.inputChoiceIndex];
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}