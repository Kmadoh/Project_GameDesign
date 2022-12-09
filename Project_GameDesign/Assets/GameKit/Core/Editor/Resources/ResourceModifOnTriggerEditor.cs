using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResourceModifOnTrigger))]
public class ResourceModifOnTriggerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	[Tooltip("Used to check public variables from the target class")]
	private ResourceModifOnTrigger myObject;

	private SerializedProperty useTag;
	private SerializedProperty tagName;
	
	private SerializedProperty useCollidingResourceManager;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceID;
	private SerializedProperty resourceAmount;
	
	private SerializedProperty spawnedFXOnTrigger;
	private SerializedProperty FXLifeTime;
	private SerializedProperty destroyAfter;
	
	private SerializedProperty displayDebugInfo;

	private int resourceChoiceIndex;

	private void OnEnable ()
	{
		myObject = (ResourceModifOnTrigger)target;

		useTag = serializedObject.FindProperty("useTag");
		tagName = serializedObject.FindProperty("tagName");
		
		useCollidingResourceManager = serializedObject.FindProperty("useCollidingResourceManager");
		resourceManager = serializedObject.FindProperty("resourceManager");
		resourceID = serializedObject.FindProperty("resourceID");
		resourceAmount = serializedObject.FindProperty("resourceAmount");
		
		spawnedFXOnTrigger = serializedObject.FindProperty("spawnedFXOnTrigger");
		FXLifeTime = serializedObject.FindProperty("FXLifeTime");
		destroyAfter = serializedObject.FindProperty("destroyAfter");
		
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		
	}

	private void InitGUI()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
	}

	private void DisplayToolbarMenu()
	{
		EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Tag", "Resources", "Other" }, GUILayout.MinHeight(25));
			
			if (displayDebugInfo.boolValue)
			{
				if (GUILayout.Button("Debug ON", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
				{
					displayDebugInfo.boolValue = !displayDebugInfo.boolValue;
				}
			}
			else
			{
				if (GUILayout.Button("Debug OFF", UIHelper.redButtonStyle, GUILayout.MaxHeight(20f)))
				{
					displayDebugInfo.boolValue = !displayDebugInfo.boolValue;
				}
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private void AssignTab()
	{
		currentTab = toolBarTab switch
		{
			0 => "Tag",
			1 => "Resources",
			2 => "Other",
			_ => currentTab
		};

		//Apply modified properties to avoid data loss
		if (!EditorGUI.EndChangeCheck()) return;
		
		serializedObject.ApplyModifiedProperties();
		GUI.FocusControl(null);
	}

	private void HandleTabs()
	{
		switch (currentTab)
		{
			case "Tag":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(useTag);
					if (useTag.boolValue)
					{
						tagName.stringValue = EditorGUILayout.TagField("Tag : ", tagName.stringValue);
					}
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Resources":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(useCollidingResourceManager);

					if (!useCollidingResourceManager.boolValue)
					{
						EditorGUILayout.PropertyField(resourceManager);
						if (resourceManager.Exists())
						{
							resourceChoiceIndex = EditorGUILayout.Popup("Resource ID : ", resourceChoiceIndex, myObject.resourceManager.idList.ToArray());
							resourceID.stringValue = myObject.resourceManager.idList[resourceChoiceIndex];
						}
					}
					else
					{
						EditorGUILayout.PropertyField(resourceID);
					}
					
					EditorGUILayout.PropertyField(resourceAmount);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Other":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(spawnedFXOnTrigger);

					if (spawnedFXOnTrigger.Exists())
					{
						EditorGUILayout.PropertyField(FXLifeTime);
					}
					
					EditorGUILayout.PropertyField(destroyAfter);
				}
				EditorGUILayout.EndVertical();
			}
			break;
		}
	}

	private void HandleDebugMessages()
	{
		if (!displayDebugInfo.boolValue) return;
	}

	public override void OnInspectorGUI ()
	{
		InitGUI();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			DisplayToolbarMenu();

			AssignTab();

			EditorGUI.BeginChangeCheck();

			HandleTabs();

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
		}
		EditorGUILayout.EndVertical();
	}
}