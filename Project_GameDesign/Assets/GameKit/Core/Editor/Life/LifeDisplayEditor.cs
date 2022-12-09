using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LifeDisplay)), CanEditMultipleObjects]
public class LifeDisplayEditor : Editor
{
    private SerializedProperty lifeToDisplay;
    private SerializedProperty lifeBarSlider;
    private SerializedProperty lifeBarVisualType;
    private SerializedProperty lifeBarImage;
    private SerializedProperty attackTriggerParameterName;

    private void OnEnable ()
    {
        lifeToDisplay = serializedObject.FindProperty("lifeToDisplay");
        lifeBarVisualType = serializedObject.FindProperty("lifeBarVisualType");
        attackTriggerParameterName = serializedObject.FindProperty("attackTriggerParameterName");
        lifeBarSlider = serializedObject.FindProperty("lifeBarSlider");
        lifeBarImage = serializedObject.FindProperty("lifeBarImage");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.PropertyField(lifeToDisplay);
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(lifeBarVisualType);
                
                switch (lifeBarVisualType.intValue)
                {
                    case (int)LifeDisplay.LifeBarVisualType.Image:
                        EditorGUILayout.PropertyField(lifeBarImage);
                        break;
                    case (int)LifeDisplay.LifeBarVisualType.Slider:
                        EditorGUILayout.PropertyField(lifeBarSlider);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();


        if (lifeToDisplay.propertyType == SerializedPropertyType.ObjectReference && lifeToDisplay.objectReferenceValue == null)
        {
            EditorGUILayout.BeginVertical(UIHelper.warningStyle);
            {
                EditorGUILayout.LabelField("No Life To Display set ! Please add one as reference", EditorStyles.boldLabel);
            }
            EditorGUILayout.EndVertical();
        }
        
        switch (lifeBarVisualType.intValue)
        {
            case (int)LifeDisplay.LifeBarVisualType.Image:
                if (lifeBarImage.propertyType == SerializedPropertyType.ObjectReference && lifeBarImage.objectReferenceValue != null)
                {
                    EditorGUILayout.BeginVertical(UIHelper.warningStyle);
                    {
                        EditorGUILayout.LabelField("No Image set ! Please add one as reference", EditorStyles.boldLabel);
                    }
                    EditorGUILayout.EndVertical();
                }
                break;
            case (int)LifeDisplay.LifeBarVisualType.Slider:
                
                if (lifeBarSlider.propertyType == SerializedPropertyType.ObjectReference && lifeBarSlider.objectReferenceValue != null)
                {
                    EditorGUILayout.BeginVertical(UIHelper.warningStyle);
                    {
                        EditorGUILayout.LabelField("No Slider set ! Please add one as reference", EditorStyles.boldLabel);
                    }
                    EditorGUILayout.EndVertical();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}