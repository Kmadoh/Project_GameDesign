using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(EnableComponentsOnTrigger))]
public class EnableComponentsOnTriggerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;

	private string currentTab;

	[Tooltip("Used to check public variables from the target class")]
	private EnableComponentsOnTrigger myObject;

	private SerializedProperty useTag;
	private SerializedProperty tagName;
	
	private SerializedProperty revertOn;
	private SerializedProperty revertAfterCooldown;
	
	private SerializedProperty disableInstead;
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceID;
	private SerializedProperty resourceCostOnUse;
	
	private SerializedProperty requireInput;
	private SerializedProperty inputName;
	
	private SerializedProperty components;

	private bool showComponents = false;
	
	private int resourceChoiceIndex = 0;
	private int inputChoiceIndex = 0;

	private void OnEnable()
	{
		myObject = (EnableComponentsOnTrigger) target;
		
		disableInstead = serializedObject.FindProperty("disableInstead");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");

		useTag = serializedObject.FindProperty("useTag");
		tagName = serializedObject.FindProperty("tagName");
		
		revertOn = serializedObject.FindProperty("revertOn");
		revertAfterCooldown = serializedObject.FindProperty("revertAfterCooldown");
		
		requireResources = serializedObject.FindProperty("requireResources");
		resourceManager = serializedObject.FindProperty("resourceManager");
		resourceID = serializedObject.FindProperty("resourceID");
		resourceCostOnUse = serializedObject.FindProperty("resourceCostOnUse");
		
		requireInput = serializedObject.FindProperty("requireInput");
		inputName = serializedObject.FindProperty("inputName");

		components = serializedObject.FindProperty("components");
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
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] {"General", "Components", "Resources", "Input"},
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
				0 => "General",
				1 => "Components",
				2 => "Resources",
				3 => "Input",
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
					EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
					{						
						EditorGUILayout.PropertyField(useTag);

						if (useTag.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								tagName.stringValue = EditorGUILayout.TagField((tagName.stringValue));
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{	
						EditorGUILayout.PropertyField(revertOn);

						if (revertOn.intValue == (int)EnableComponentsOnTrigger.RevertOn.Timer)
						{
							EditorGUILayout.PropertyField(revertAfterCooldown);
						}
					}
					EditorGUILayout.EndVertical();
				} 
					break;
				
				case "Resources":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{						
						EditorGUILayout.PropertyField(requireResources);

						if (requireResources.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(resourceManager);
								if (resourceManager.Exists())
								{
									resourceChoiceIndex = EditorGUILayout.Popup("Resource ID : ", resourceChoiceIndex, myObject.resourceManager.idList.ToArray());
									resourceID.stringValue = myObject.resourceManager.idList[resourceChoiceIndex];
								}
								else
								{
									EditorGUILayout.PropertyField(resourceID);
								}
								EditorGUILayout.PropertyField(resourceCostOnUse);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				} 
					break;

				case "Input":
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{						
						EditorGUILayout.PropertyField(requireInput);

						if (requireInput.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								inputChoiceIndex = EditorGUILayout.Popup("Input : ", inputChoiceIndex, Helper.GetInputAxes());
								inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex];
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

			// Can be used to display contextual error messages

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

						toolBarTab = 1;
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