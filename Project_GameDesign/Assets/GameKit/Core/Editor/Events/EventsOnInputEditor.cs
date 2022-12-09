using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventsOnInput)), CanEditMultipleObjects]
public class EventsOnInputEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private SerializedProperty triggeredEvents;
	private SerializedProperty loop;
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;

	private void OnEnable ()
	{
	
		loop = serializedObject.FindProperty("loop");
		triggeredEvents = serializedObject.FindProperty("triggeredEvents");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
		
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
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Input", "Events"}, GUILayout.MinHeight(25));

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
			0 => "Input",
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
			case "Input":
			{
				EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
				{
					inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
					inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
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