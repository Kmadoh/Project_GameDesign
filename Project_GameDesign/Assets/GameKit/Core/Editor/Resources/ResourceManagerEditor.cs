using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    [Tooltip("Used to check public variables from the target class")]
    private ResourceManager myObject;

    private SerializedProperty displayedResources;
    private SerializedProperty resources;

    private void OnEnable ()
    {
        myObject = (ResourceManager)target;

        displayedResources = serializedObject.FindProperty("displayedResources");
        resources = serializedObject.FindProperty("resources");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            if (!myObject.hasBegun)
            {
                EditorGUILayout.BeginVertical(UIHelper.subStyle1);
                {
                    EditorGUILayout.PropertyField(displayedResources);
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical(UIHelper.subStyle1);
                {
                    foreach (var resource in myObject.resources)
                    {
                            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
                            {
                                EditorGUILayout.BeginHorizontal(UIHelper.subStyle2);
                                {
                                    EditorGUILayout.LabelField("Resource ID : " + resource.Key);
                                    GUILayout.Box(myObject.resources[resource.Key].resourceIcon.texture);
                                }
                                EditorGUILayout.EndHorizontal();
                                
                                myObject.resources[resource.Key].currentResourceAmount = EditorGUILayout.IntField("Current amount : ", myObject.resources[resource.Key].currentResourceAmount);
                                myObject.resources[resource.Key].maxResourceAmount = EditorGUILayout.IntField("Maximum : ", myObject.resources[resource.Key].maxResourceAmount);
                            }
                            EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}