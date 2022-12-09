using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(ToolbarDummyClass))]
public class CustomToolbarTemplate : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	[Tooltip("Used to check public variables from the target class")]
	private ToolbarDummyClass myObject;
	private SerializedObject soTarget;

	private SerializedProperty exampleVariable1;
	private SerializedProperty exampleVariable2;
	private SerializedProperty exampleVariable3;
	private SerializedProperty exampleVariable4;
	private SerializedProperty exampleVariable5;

	private void OnEnable ()
	{
		myObject = (ToolbarDummyClass)target;
		soTarget = new SerializedObject(target);

		exampleVariable1 = soTarget.FindProperty("exampleVariable1");
		exampleVariable2 = soTarget.FindProperty("exampleVariable2");
		exampleVariable3 = soTarget.FindProperty("exampleVariable3");
		exampleVariable4 = soTarget.FindProperty("exampleVariable4");
		exampleVariable5 = soTarget.FindProperty("exampleVariable5");
	}

	private void InitGUI()
	{
		soTarget.Update();
		EditorGUI.BeginChangeCheck();
	}

	private void OnValidate()
	{
		UIHelper.InitializeStyles();
	}
	
	private void DisplayToolbarMenu()
	{
		EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Tab1", "Tab2", "Tab3", "Tab4" }, GUILayout.MinHeight(25));

			if (myObject.displayDebugInfo)
			{
				if (GUILayout.Button("Debug ON", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
				{
					myObject.displayDebugInfo = !myObject.displayDebugInfo;
				}
			}
			else
			{
				if (GUILayout.Button("Debug OFF", UIHelper.redButtonStyle, GUILayout.MaxHeight(20f)))
				{
					myObject.displayDebugInfo = !myObject.displayDebugInfo;
				}
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private void AssignTab()
	{
		currentTab = toolBarTab switch
		{
			0 => "Tab1",
			1 => "Tab2",
			2 => "Tab3",
			3 => "Tab4",
			_ => currentTab
		};

		//Apply modified properties to avoid data loss
		if (!EditorGUI.EndChangeCheck()) return;
		
		soTarget.ApplyModifiedProperties();
		GUI.FocusControl(null);
	}

	private void HandleTabs()
	{
		switch (currentTab)
		{
			case "Tab1":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(exampleVariable2);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Tab2":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(exampleVariable3);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Tab3":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(exampleVariable4);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Tab4":
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(exampleVariable5);
				}
				EditorGUILayout.EndVertical();
			}
			break;
		}
	}

	private void HandleDebugMessages()
	{
		if (!myObject.displayDebugInfo) return;
		
		// Context-Specific messages
		switch (toolBarTab)
		{
			case 0:
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Test Contextual Warning message", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 1:
			{
				
			}
			break;

			case 2:
			{
				
			}
			break;

			case 3:
			{
				
			}
			break;
		}

		// Generic messages
		EditorGUILayout.BeginVertical(UIHelper.warningStyle);
		{
			EditorGUILayout.LabelField("Test General Warning message", EditorStyles.boldLabel);
		}
		EditorGUILayout.EndVertical();
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
				soTarget.ApplyModifiedProperties();
			}

			HandleDebugMessages();
		}
		EditorGUILayout.EndVertical();
	}
}