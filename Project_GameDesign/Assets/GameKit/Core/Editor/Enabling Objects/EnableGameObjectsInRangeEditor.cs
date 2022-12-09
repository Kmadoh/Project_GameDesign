using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(EnableGameObjectsInRange)), CanEditMultipleObjects]
public class EnableGameObjectsInRangeEditor : Editor
{
[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private SerializedProperty range;
	private SerializedProperty specificTarget;

	private SerializedProperty layerMask;
	private SerializedProperty revertWhenOutOfRange;
	private SerializedProperty findWithTag;
	private SerializedProperty targetTagName;
	
	private SerializedProperty disableInstead;
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty gameObjectsToEnable;

	private bool showComponents = false;
	private void OnEnable ()
	{
		disableInstead = serializedObject.FindProperty("disableInstead");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		
		range = serializedObject.FindProperty("range");
		specificTarget = serializedObject.FindProperty("specificTarget");
		
		gameObjectsToEnable = serializedObject.FindProperty("gameObjectsToEnable");

		findWithTag = serializedObject.FindProperty("findWithTag");
		targetTagName = serializedObject.FindProperty("targetTagName");
		
		layerMask = serializedObject.FindProperty("layerMask");
		revertWhenOutOfRange = serializedObject.FindProperty("revertWhenOutOfRange");
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
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "General", "Targets", "GameObjects"}, GUILayout.MinHeight(25));
				
				if (disableInstead.boolValue)
				{
					if (GUILayout.Button("Set Active False", UIHelper.redButtonStyle, GUILayout.MaxHeight(25f), GUILayout.MaxWidth(150f)))
					{
						disableInstead.boolValue = !disableInstead.boolValue;
					}
				}
				else
				{
					if (GUILayout.Button("Set Active True", UIHelper.greenButtonStyle, GUILayout.MaxHeight(25f), GUILayout.MaxWidth(150f)))
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
				2 => "GameObjects",
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

				case "GameObjects":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.BeginHorizontal();
						{
							if (!showComponents)
							{
								if (GUILayout.Button(" Show GameObjects (" + gameObjectsToEnable.arraySize + ")",
									    UIHelper.redButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide GameObjects (" + gameObjectsToEnable.arraySize + ")",
									    UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showComponents = false;
								}
							}

							if (GUILayout.Button(" Add GameObject ", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
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
								for (int i = 0; i < gameObjectsToEnable.arraySize; i++)
								{
									EditorGUILayout.BeginHorizontal(UIHelper.subStyle2);
									{
										SerializedProperty component = gameObjectsToEnable.GetArrayElementAtIndex(i);
										
										component.objectReferenceValue = (GameObject) EditorGUILayout.ObjectField(component.objectReferenceValue, typeof(GameObject), true, GUILayout.MaxWidth(200f));
									
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
				if (gameObjectsToEnable.arraySize == 0)
				{
					if (GUILayout.Button("No GameObjects to Enable/Disable set ! Click here to add one", UIHelper.redButtonStyle))
					{
						AddComponent();
						showComponents = true;
						
						toolBarTab = 2;
						currentTab = "Components";
					}
				}
				
				if (specificTarget.objectReferenceValue == null && targetTagName.stringValue == "")
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
		gameObjectsToEnable.InsertArrayElementAtIndex(gameObjectsToEnable.arraySize);
	}

	private void RemoveComponent (int index)
	{
		gameObjectsToEnable.DeleteArrayElementAtIndex(index);
	}
}
