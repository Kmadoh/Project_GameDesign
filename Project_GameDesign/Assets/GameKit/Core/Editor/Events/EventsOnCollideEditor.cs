using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventsOnCollide)), CanEditMultipleObjects]
public class EventsOnCollideEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private SerializedProperty useTag;
	private SerializedProperty tagName;
	private SerializedProperty triggeredEvents;
	private SerializedProperty loop;
	
	private SerializedProperty displayDebugInfo;

	private void OnEnable ()
	{
	
		useTag = serializedObject.FindProperty("useTag");
		tagName = serializedObject.FindProperty("tagName");
		loop = serializedObject.FindProperty("loop");
		triggeredEvents = serializedObject.FindProperty("triggeredEvents");
		
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
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Collision", "Events" }, GUILayout.MinHeight(25));
			
			if (displayDebugInfo.boolValue)
			{
				if (GUILayout.Button("Debug ON", UIHelper.greenButtonStyle, GUILayout.MaxHeight(25f), GUILayout.MaxWidth(100f)))
				{
					displayDebugInfo.boolValue = !displayDebugInfo.boolValue;
				}
			}
			else
			{
				if (GUILayout.Button("Debug OFF", UIHelper.redButtonStyle, GUILayout.MaxHeight(25f), GUILayout.MaxWidth(100f)))
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
			0 => "Collision",
			1 => "Events",
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
			case "Collision":
			{
				EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(useTag);

					if (useTag.boolValue)
					{
						tagName.stringValue = EditorGUILayout.TagField(tagName.stringValue);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			break;

			case "Events":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(loop);
					EditorGUILayout.PropertyField(triggeredEvents);
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

			HandleDebugMessages();
		}
		EditorGUILayout.EndVertical();
	}
}