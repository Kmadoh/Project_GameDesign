using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[CustomEditor(typeof(Spawner))]

public class SpawnerEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private Spawner myObject;

	private SerializedProperty prefabToSpawn;
	private SerializedProperty shareOrientation;

	private SerializedProperty useCooldown;
	private SerializedProperty spawnCooldown;

	private SerializedProperty useInput;
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty spawnInputName;

	private SerializedProperty spawnInsideParent;
	private SerializedProperty parent;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;
	
	private SerializedProperty randomMinOffset;
	private SerializedProperty randomMaxOffset;

	private SerializedProperty destroyOnNoResourceLeft;
	
	private SerializedProperty displayDebugInfo;

	bool showSpawns = false;

	private void OnEnable ()
	{
		myObject = (Spawner)target;

		prefabToSpawn = serializedObject.FindProperty("prefabToSpawn");
		
		shareOrientation = serializedObject.FindProperty("shareOrientation");
		
		randomMinOffset = serializedObject.FindProperty("randomMinOffset");
		randomMaxOffset = serializedObject.FindProperty("randomMaxOffset");

		useCooldown = serializedObject.FindProperty("useCooldown");
		spawnCooldown = serializedObject.FindProperty("spawnCooldown");

		useInput = serializedObject.FindProperty("useInput");
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		spawnInputName = serializedObject.FindProperty("spawnInputName");

		spawnInsideParent = serializedObject.FindProperty("spawnInsideParent");
		parent = serializedObject.FindProperty("parent");

		requireResources = serializedObject.FindProperty("requireResources");
		resourceManager = serializedObject.FindProperty("resourceManager");
		resourceIndex = serializedObject.FindProperty("resourceIndex");
		resourceCostOnUse = serializedObject.FindProperty("resourceCostOnUse");
		destroyOnNoResourceLeft = serializedObject.FindProperty("destroyOnNoResourceLeft");
		
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");

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
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Spawn", "Cooldown", "Input", "Nested Spawning", "Resources" }, GUILayout.MinHeight(25));

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
				0 => "Spawn",
				1 => "Cooldown",
				2 => "Input",
				3 => "Nested Spawning",
				4 => "Resources",
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
				case "Spawn":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{

						EditorGUILayout.Space(5);
						EditorGUILayout.BeginHorizontal();
						{
							if (!showSpawns)
							{
								if (GUILayout.Button(" Show Prefabs (" + prefabToSpawn.arraySize + ")",
									    UIHelper.redButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showSpawns = true;
								}
							}
							else
							{
								if (GUILayout.Button(" Hide Prefabs (" + prefabToSpawn.arraySize + ")",
									    UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
								{
									showSpawns = false;
								}
							}

							if (GUILayout.Button(" Add Prefab ", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
							{
								AddSpawn();

								if (showSpawns == false)
								{
									showSpawns = true;
								}
							}

							if (GUILayout.Button(" Spawn Prefab ", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
							{
								UIHelper.PreShotDirty("SpawnObject", target);
								
								myObject.SpawnObject();
								if (prefabToSpawn.arraySize == 0)
								{
									showSpawns = false;
								}
								
								UIHelper.DirtyStuff(target);
							}
						}
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Space(5);

						if (showSpawns)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								for (int i = 0; i < prefabToSpawn.arraySize; i++)
								{
									EditorGUILayout.BeginHorizontal(UIHelper.subStyle2);
									{
										SerializedProperty prefab = prefabToSpawn.GetArrayElementAtIndex(i).FindPropertyRelative("prefab");
										prefab.objectReferenceValue = (GameObject) EditorGUILayout.ObjectField(prefab.objectReferenceValue, typeof(GameObject), false, GUILayout.MaxWidth(200f));
										
										EditorGUILayout.LabelField("Weight : ", GUILayout.MaxWidth(40f));
										SerializedProperty weight = prefabToSpawn.GetArrayElementAtIndex(i).FindPropertyRelative("weight");
										weight.intValue = EditorGUILayout.IntField(weight.intValue, GUILayout.MaxWidth(40f));

										if (GUILayout.Button("X", UIHelper.redButtonStyle))
										{
											RemoveSpawn(i);
										}

									}
									EditorGUILayout.EndHorizontal();
								}
							}
							EditorGUILayout.EndVertical();
						}

						EditorGUILayout.BeginVertical(UIHelper.mainStyle);
						{
							EditorGUILayout.LabelField("Random Offset");

							Vector3 minOffset = randomMinOffset.vector3Value;
							Vector3 maxOffset = randomMaxOffset.vector3Value;
							EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
							{
								EditorGUILayout.LabelField("X : ", GUILayout.MaxWidth(40f));
								
								minOffset.x = EditorGUILayout.FloatField(minOffset.x, GUILayout.MaxWidth(50f));
								EditorGUILayout.MinMaxSlider(ref minOffset.x, ref maxOffset.x, -100f, 100f, GUILayout.MinWidth(100f));
								maxOffset.x = EditorGUILayout.FloatField(maxOffset.x, GUILayout.MaxWidth(50f));
							}
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
							{
								EditorGUILayout.LabelField("Y : ", GUILayout.MaxWidth(40f));
								minOffset.y = EditorGUILayout.FloatField(minOffset.y, GUILayout.MaxWidth(50f));
								EditorGUILayout.MinMaxSlider(ref minOffset.y, ref maxOffset.y, -100f, 100f, GUILayout.MinWidth(100f));
								maxOffset.y = EditorGUILayout.FloatField(maxOffset.y, GUILayout.MaxWidth(50f));
							}
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
							{
								EditorGUILayout.LabelField("Z : ", GUILayout.MaxWidth(40f));
								minOffset.z = EditorGUILayout.FloatField(minOffset.z, GUILayout.MaxWidth(50f));
								EditorGUILayout.MinMaxSlider(ref minOffset.z, ref maxOffset.z, -100f, 100f, GUILayout.MinWidth(100f));
								maxOffset.z = EditorGUILayout.FloatField(maxOffset.z, GUILayout.MaxWidth(50f));
							}
							EditorGUILayout.EndHorizontal();

							randomMinOffset.vector3Value = minOffset;
							randomMaxOffset.vector3Value = maxOffset;

							EditorGUILayout.PropertyField(shareOrientation);
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Cooldown":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(useCooldown);
						if (useCooldown.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								EditorGUILayout.PropertyField(spawnCooldown);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Input":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(useInput);
						if (useInput.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
								spawnInputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Nested Spawning":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(spawnInsideParent);
						if (spawnInsideParent.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								EditorGUILayout.PropertyField(parent);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Resources":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(requireResources);
						if (requireResources.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								EditorGUILayout.PropertyField(resourceManager);
								EditorGUILayout.PropertyField(resourceIndex);
								EditorGUILayout.PropertyField(resourceCostOnUse);
								EditorGUILayout.PropertyField(destroyOnNoResourceLeft);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				break;
			}

			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				GUI.FocusControl(GUI.GetNameOfFocusedControl());
			}

			// Can be used to display contextual error messages
			#region DebugMessages

			if(displayDebugInfo.boolValue)
			{

				if (!useInput.boolValue && !useCooldown.boolValue)
				{
					EditorGUILayout.BeginHorizontal(UIHelper.warningStyle);
					{
						EditorGUILayout.LabelField("Use Input and/or Cooldown-based spawning !", EditorStyles.boldLabel);
						if(GUILayout.Button("Input", UIHelper.buttonStyle))
						{
							useInput.boolValue = true;
							toolBarTab = 2;
							currentTab = "Input";
						}
						if (GUILayout.Button("Cooldown", UIHelper.buttonStyle))
						{
							useCooldown.boolValue = true;
							toolBarTab = 1;
							currentTab = "Cooldown";
						}
					}
					EditorGUILayout.EndHorizontal();
				}

				if (useInput.boolValue && spawnInputName.stringValue == "Input Name (InputManager)")
				{
					EditorGUILayout.BeginVertical(UIHelper.warningStyle);
					{
						EditorGUILayout.LabelField("No Input set for spawning !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}

				if (useCooldown.boolValue && spawnCooldown.floatValue == 0f)
				{
					EditorGUILayout.BeginVertical(UIHelper.warningStyle);
					{
						EditorGUILayout.LabelField("Cooldown set for spawning is equal to 0 !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}

				if (prefabToSpawn.arraySize == 0 || myObject.prefabToSpawn[0] == null)
				{
					EditorGUILayout.BeginVertical(UIHelper.warningStyle);
					{
						EditorGUILayout.LabelField("No Prefab to spawn set !! Please add one", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}
			}
			
			#endregion
		}
		EditorGUILayout.EndVertical();
	}

	
	private void AddSpawn ()
	{
		UIHelper.PreShotDirty("AddSpawn", target);
		
		myObject.prefabToSpawn.Add(new Spawner.PrefabToSpawn(null, 1));
		
		UIHelper.DirtyStuff(target);
	}

	private void RemoveSpawn (int index)
	{
		UIHelper.PreShotDirty("RemoveSpawn",target);
		
		myObject.prefabToSpawn.RemoveAt(index);

		if (prefabToSpawn.arraySize == 0)
		{ 
			showSpawns = false;
		}
		
		UIHelper.DirtyStuff(target);
	}
}
