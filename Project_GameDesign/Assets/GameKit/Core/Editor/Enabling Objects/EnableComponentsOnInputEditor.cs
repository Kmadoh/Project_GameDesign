using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnableComponentsOnInput)), CanEditMultipleObjects]
public class EnableComponentsOnInputEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;

	private string currentTab;

	private SerializedProperty revertOn;
	private SerializedProperty revertAfterCooldown;

	private SerializedProperty disableInstead;
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty components;
	private SerializedProperty inputName;
	
	private bool showComponents = false;
	private int inputChoiceIndex;

	private void OnEnable()
	{
		revertOn = serializedObject.FindProperty("revertOn");
		revertAfterCooldown = serializedObject.FindProperty("revertAfterCooldown");
		
		disableInstead = serializedObject.FindProperty("disableInstead");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		
		components = serializedObject.FindProperty("components");
		inputName = serializedObject.FindProperty("inputName");
	}

	public override void OnInspectorGUI()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] {"Input", "Components"},
					GUILayout.MinHeight(25));

				if (disableInstead.boolValue)
				{
					if (GUILayout.Button("Disabling Components", UIHelper.redButtonStyle, GUILayout.MaxHeight(25f), GUILayout.MaxWidth(150f)))
					{
						disableInstead.boolValue = !disableInstead.boolValue;
					}
				}
				else
				{
					if (GUILayout.Button("Enabling Components", UIHelper.greenButtonStyle, GUILayout.MaxHeight(25f), GUILayout.MaxWidth(150f)))
					{
						disableInstead.boolValue = !disableInstead.boolValue;
					}
				}

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

			currentTab = toolBarTab switch
			{
				0 => "Input",
				1 => "Components",
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
				case "Input":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{						
						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							inputChoiceIndex = EditorGUILayout.Popup("Input : ", inputChoiceIndex, Helper.GetInputAxes());
							inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex];
						}
						EditorGUILayout.EndVertical();
						
						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							EditorGUILayout.PropertyField(revertOn);
							if (revertOn.intValue == (int)EnableComponentsOnInput.RevertOn.Timer)
							{
								EditorGUILayout.PropertyField(revertAfterCooldown);
							}
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndVertical();
				} 
					break;

				case "Components":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.BeginHorizontal();
						{
							if (!showComponents)
							{
								if (GUILayout.Button(" Show Components (" + components.arraySize + ")",
									    UIHelper.redButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide Components (" + components.arraySize + ")",
									    UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = false;
								}
							}

							if (GUILayout.Button(" Add Component ", UIHelper.greenButtonStyle,
								    GUILayout.MaxHeight(20f)))
							{
								AddComponent();

								if (showComponents == false)
								{
									showComponents = true;
								}
							}
						}
						EditorGUILayout.EndHorizontal();

						if (showComponents)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								for (int i = 0; i < components.arraySize; i++)
								{
									EditorGUILayout.BeginHorizontal(UIHelper.subStyle2);
									{
										SerializedProperty component = components.GetArrayElementAtIndex(i);
										
										component.objectReferenceValue = (Behaviour) EditorGUILayout.ObjectField(component.objectReferenceValue, typeof(Behaviour), true, GUILayout.MaxWidth(200f));
									
										if (GUILayout.Button("X", UIHelper.redButtonStyle, GUILayout.MaxWidth(50)))
										{
											RemoveComponent(i);
										}

									}
									EditorGUILayout.EndHorizontal();
								}
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

			#region DebugMessages

			if (displayDebugInfo.boolValue)
			{
				if (components.arraySize == 0)
				{
					if (GUILayout.Button("No Components to Enable/Disable set ! Click here to add one",
						    UIHelper.redButtonStyle))
					{
						AddComponent();
						showComponents = true;

						toolBarTab = 2;
						currentTab = "Components";
					}
				}
			}
			#endregion
		}
		EditorGUILayout.EndVertical();
	}

	private void AddComponent ()
	{
		components.InsertArrayElementAtIndex(components.arraySize);
	}

	private void RemoveComponent (int index)
	{
		components.DeleteArrayElementAtIndex(index);
	}
}