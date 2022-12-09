using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnOnTrigger)), CanEditMultipleObjects]
public class SpawnOnTriggerEditor : Editor
{
	private int toolBarTab;
	private string currentTab;
	
	private SerializedProperty useTagOnTrigger;
	private SerializedProperty tagName;
	private SerializedProperty onlyOnce;

	private SerializedProperty shareOrientation;
	private SerializedProperty prefabToSpawn;

	private SerializedProperty spawnInsideParent;
	private SerializedProperty spawnInsideCollidingObject;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;

	private SerializedProperty requireInput;

	private SerializedProperty parent;
	private SerializedProperty displayDebugInfo;
	private SerializedProperty randomMinOffset;
	private SerializedProperty randomMaxOffset;
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;

	private void OnEnable ()
	{
		////
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
		randomMinOffset = serializedObject.FindProperty("randomMinOffset");
		randomMaxOffset = serializedObject.FindProperty("randomMaxOffset");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		useTagOnTrigger = serializedObject.FindProperty("useTagOnTrigger");
		tagName = serializedObject.FindProperty("tagName");
		onlyOnce = serializedObject.FindProperty("onlyOnce");

		shareOrientation = serializedObject.FindProperty("shareOrientation");
		prefabToSpawn = serializedObject.FindProperty("prefabToSpawn");

		////

		spawnInsideParent = serializedObject.FindProperty("spawnInsideParent");
		spawnInsideCollidingObject = serializedObject.FindProperty("spawnInsideCollidingObject");
		parent = serializedObject.FindProperty("parent");

		////

		requireResources = serializedObject.FindProperty("requireResources");
		resourceManager = serializedObject.FindProperty("resourceManager");
		resourceIndex = serializedObject.FindProperty("resourceIndex");
		resourceCostOnUse = serializedObject.FindProperty("resourceCostOnUse");

		requireInput = serializedObject.FindProperty("requireInput");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();
		
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Collision", "Spawning", "Nested Spawning", "Resources", "Input" }, GUILayout.MinHeight(25));
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

		switch (toolBarTab)
		{
			case 0:
			currentTab = "Collision";
			break;

			case 1:
			currentTab = "Spawning";
			break;

			case 2:
			currentTab = "Nested Spawning";
			break;

			case 3:
			currentTab = "Resources";
			break;

			case 4:
			currentTab = "Input";
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}

		EditorGUI.BeginChangeCheck();

		switch (currentTab)
		{
			case "Collision":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(onlyOnce);

						EditorGUILayout.PropertyField(useTagOnTrigger);
						if (useTagOnTrigger.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								tagName.stringValue = EditorGUILayout.TagField("Tag :", tagName.stringValue);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
			break;

			case "Spawning":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(prefabToSpawn);

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

			case "Nested Spawning":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(spawnInsideParent);
						if (spawnInsideParent.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								EditorGUILayout.PropertyField(spawnInsideCollidingObject);
								if (!spawnInsideCollidingObject.boolValue)
								{
									EditorGUILayout.PropertyField(parent);
								}
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
						EditorGUILayout.PropertyField(requireInput);

						if (requireInput.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
								inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
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

		if (!displayDebugInfo.boolValue) return;
		
		if (prefabToSpawn.arraySize == 0)
		{
			EditorGUILayout.BeginVertical(UIHelper.warningStyle);
			{
				EditorGUILayout.LabelField("No Prefab to spawn set !! Please add one", EditorStyles.boldLabel);
			}
			EditorGUILayout.EndVertical();
		}

		if (!requireInput.boolValue || inputName.stringValue != "") return;
		
		EditorGUILayout.BeginVertical(UIHelper.warningStyle);
		{
			EditorGUILayout.LabelField("No Input name set ! Either add one or disable Require Input", EditorStyles.boldLabel);
		}
		EditorGUILayout.EndVertical();
	}
}
