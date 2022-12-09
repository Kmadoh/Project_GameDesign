using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Life)), CanEditMultipleObjects]
public class LifeEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private SerializedProperty startLife;
	private SerializedProperty currentLife;
	private SerializedProperty maxLife;

	private SerializedProperty invincibilityDuration;

	private SerializedProperty animator;
	private SerializedProperty hitParameterName;
	
	private SerializedProperty onDamageTaken;
	private SerializedProperty onDeath;
	private SerializedProperty displayDebugInfo;

	private void OnEnable ()
	{
		maxLife = serializedObject.FindProperty("maxLife");
		startLife = serializedObject.FindProperty("startLife");
		currentLife = serializedObject.FindProperty("currentLife");
		invincibilityDuration = serializedObject.FindProperty("invincibilityDuration");

		animator = serializedObject.FindProperty("animator");
		hitParameterName = serializedObject.FindProperty("hitParameterName");
		
		onDamageTaken = serializedObject.FindProperty("onDamageTaken");
		onDeath = serializedObject.FindProperty("onDeath");
		
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
	}
	
	private void DisplayToolbarMenu()
	{
		EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "General", "Events" }, GUILayout.MinHeight(25));

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
			0 => "General",
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
			case "General":
			{
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(maxLife);
						EditorGUILayout.LabelField("Start Life ", GUILayout.MaxWidth(80));
						startLife.intValue = EditorGUILayout.IntSlider(startLife.intValue, 0, maxLife.intValue);
					}

					EditorGUILayout.EndHorizontal();
			
					EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(invincibilityDuration);
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(animator);

						if (animator.propertyType == SerializedPropertyType.ObjectReference && animator.objectReferenceValue != null)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(hitParameterName);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
			}
				break;

			case "Events":
			{
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(onDamageTaken);
					EditorGUILayout.PropertyField(onDeath);
				}
				EditorGUILayout.EndVertical();
			} 
			break;
		}
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();
		
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		if(!Application.isPlaying)
		{
			currentLife.intValue = startLife.intValue;
		}

		EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
		{
			Rect space = GUILayoutUtility.GetRect(GUIContent.none, UIHelper.mainStyle, GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth));
			EditorGUI.ProgressBar(space, (float)currentLife.intValue / (float)maxLife.intValue, "Current Life");
		}
		EditorGUILayout.EndHorizontal();

		DisplayToolbarMenu();

		AssignTab();
		
		EditorGUI.BeginChangeCheck();

		HandleTabs();

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
