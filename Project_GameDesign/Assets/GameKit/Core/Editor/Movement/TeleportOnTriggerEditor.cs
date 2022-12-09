using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TeleportOnTrigger)), CanEditMultipleObjects]
public class TeleportOnTriggerEditor : Editor
{
    [Tooltip("Used to check public variables from the target class")]
    private TeleportOnTrigger myObject;

    private SerializedProperty teleportPoint;
    private SerializedProperty offset;
    
    private SerializedProperty useTag;
    private SerializedProperty tagName;
    
    private SerializedProperty requireInput;
    private SerializedProperty displayTeleportPoint;
    
    private SerializedProperty inputChoiceIndex;
    private SerializedProperty inputName;

    private void OnEnable ()
    {
        myObject = (TeleportOnTrigger)target;

        teleportPoint = serializedObject.FindProperty("teleportPoint");
        offset = serializedObject.FindProperty("offset");
        
        useTag = serializedObject.FindProperty("useTag");
        tagName = serializedObject.FindProperty("tagName");
        
        requireInput = serializedObject.FindProperty("requireInput");
        inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
        inputName = serializedObject.FindProperty("inputName");
        
        displayTeleportPoint = serializedObject.FindProperty("displayTeleportPoint");
    }

    public override void OnInspectorGUI ()
    {
        UIHelper.InitializeStyles();

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical(UIHelper.mainStyle);
        {
            EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
            {
                if (teleportPoint.objectReferenceValue != null)
                {
                    if(GUILayout.Button("Select Teleport Point", UIHelper.buttonStyle))
                    {
                        UIHelper.PreShotDirty("SelectTPPoint", target);
                        
                        Selection.activeGameObject = myObject.teleportPoint.gameObject;
                        myObject.SelectTeleportPoint();
                        
                        UIHelper.DirtyStuff(target);
                    }
                }
                else
                {
                    if(GUILayout.Button("Create Teleport Point", UIHelper.buttonStyle))
                    {
                        UIHelper.PreShotDirty("CreateTPPoint", target);
                        
                        myObject.CreateTeleportPoint();
                        
                        UIHelper.DirtyStuff(target);
                    }
                }

                if (displayTeleportPoint.boolValue)
                {
                    if(GUILayout.Button("Show Teleport Point Gizmos", UIHelper.buttonStyle))
                    {
                        UIHelper.PreShotDirty("ShowTPGizmos", target);
                        
                        SceneView.RepaintAll();
                        myObject.ToggleDisplayTeleportPoint(true);    
                        
                        UIHelper.DirtyStuff(target);
                    }
                }
                else
                {
                    if(GUILayout.Button("Hide Teleport Point Gizmos", UIHelper.buttonStyle))
                    {
                        UIHelper.PreShotDirty("HideTPGizmos", target);
                        
                        myObject.ToggleDisplayTeleportPoint(false);  
                        
                        UIHelper.DirtyStuff(target);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(teleportPoint);
                EditorGUILayout.PropertyField(offset);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(useTag);

                if (useTag.boolValue)
                {
                    tagName.stringValue = EditorGUILayout.TagField("Tag Name : ", tagName.stringValue);
                }
                
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(UIHelper.subStyle1);
            {
                EditorGUILayout.PropertyField(requireInput);

                if (requireInput.boolValue)
                {
                    inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
                    inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
                }
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