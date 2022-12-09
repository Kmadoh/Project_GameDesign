using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnableComponentsInRange)), CanEditMultipleObjects]
public class EnableComponentsInRangeEditor : Editor
{
[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private SerializedProperty range;
	private SerializedProperty specificTarget;

	private SerializedProperty layerMask;
	private SerializedProperty revertWhenOutOfRange;
	
	private SerializedProperty disableInstead;
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty findWithTag;
	private SerializedProperty targetTagName;
	
	private SerializedProperty components;

	private bool showComponents = false;
	private void OnEnable ()
	{
		range = serializedObject.FindProperty("range");
		specificTarget = serializedObject.FindProperty("specificTarget");

		layerMask = serializedObject.FindProperty("layerMask");
		revertWhenOutOfRange = serializedObject.FindProperty("revertWhenOutOfRange");
		
		disableInstead = serializedObject.FindProperty("disableInstead");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		
		findWithTag = serializedObject.FindProperty("findWithTag");
		targetTagName = serializedObject.FindProperty("targetTagName");
		
		components = serializedObject.FindProperty("components");
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
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "General", "Targets", "Components"}, GUILayout.MinHeight(25));
				
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
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			currentTab = toolBarTab switch
			{
				0 => "General",
				1 => "Targets",
				2 => "Components",
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
				case "General":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(range);
						EditorGUILayout.PropertyField(revertWhenOutOfRange);
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Targets":
				{
						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							if (findWithTag.boolValue)
							{
								if (GUILayout.Button("Check Specific Target", UIHelper.greenButtonStyle))
								{
									findWithTag.boolValue = false;
								}
								
								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									targetTagName.stringValue = EditorGUILayout.TagField("Target Tag ", targetTagName.stringValue);
									EditorGUILayout.PropertyField(layerMask);
								}
								EditorGUILayout.EndVertical();
							}
							else
							{			
								if (GUILayout.Button("Check From Tag", UIHelper.greenButtonStyle))
								{
									findWithTag.boolValue = true;
								}
								
								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									EditorGUILayout.PropertyField(specificTarget);								
								}
								EditorGUILayout.EndVertical();
							}
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

							if (GUILayout.Button(" Add Component ", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
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

			// Can be used to display contextual error messages
			#region DebugMessages
			if (displayDebugInfo.boolValue)
			{
				if (components.arraySize == 0)
				{
					if (GUILayout.Button("No Components to Enable/Disable set ! Click here to add one", UIHelper.redButtonStyle))
					{
						AddComponent();
						showComponents = true;
						
						toolBarTab = 2;
						currentTab = "Components";
					}
				}
				
				if (!specificTarget.Exists() && targetTagName.stringValue == "")
				{
					if (GUILayout.Button("Either set a specific target or a tag to look for", UIHelper.redButtonStyle))
					{
						toolBarTab = 1;
						currentTab = "Targets";
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
