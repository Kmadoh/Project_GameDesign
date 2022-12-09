using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bumper)), CanEditMultipleObjects]
public class BumperEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;
	
	private SerializedProperty bumpForce;
	private SerializedProperty bumpTowardsOther;
	private SerializedProperty additionalForceTowardsOther;
	private SerializedProperty preventInputHolding;
	private SerializedProperty tagName;
	private SerializedProperty displayDebugInfo;

	private SerializedProperty useTag;

	private void OnEnable ()
	{
		bumpForce = serializedObject.FindProperty("bumpForce");
		bumpTowardsOther = serializedObject.FindProperty("bumpTowardsOther");
		additionalForceTowardsOther = serializedObject.FindProperty("additionalForceTowardsOther");
		preventInputHolding = serializedObject.FindProperty("preventInputHolding");
		tagName = serializedObject.FindProperty("tagName");

		useTag = serializedObject.FindProperty("useTag");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Forces", "Tag"}, GUILayout.MinHeight(25));

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

		currentTab = toolBarTab switch
		{
			0 => "Forces",
			1 => "Tag",
			_ => currentTab
		};

		//Apply modified properties to avoid data loss
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}

		EditorGUI.BeginChangeCheck();

		switch (currentTab)
		{
			case "Forces":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(bumpForce);
						EditorGUILayout.PropertyField(bumpTowardsOther);

						if (bumpTowardsOther.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(additionalForceTowardsOther);
							}
							EditorGUILayout.EndVertical();
						}
						EditorGUILayout.PropertyField(preventInputHolding);
					}
					EditorGUILayout.EndVertical();
				}
			break;

			case "Tag":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(useTag);

						if (useTag.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								tagName.stringValue = EditorGUILayout.TagField("Tag :", tagName.stringValue);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		EditorGUILayout.Space();

	}
}
