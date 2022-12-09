using UnityEngine;
using UnityEditor;

//Change all Dummyclass references into the class name
[CustomEditor(typeof(ClickToMove)), CanEditMultipleObjects]
public class ClickToMoveEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private SerializedProperty lockOnX;
	private SerializedProperty lockOnY;
	private SerializedProperty lockOnZ;
	
	private SerializedProperty moveSpeed;
	private SerializedProperty lookTowardsDestination;
	
	private SerializedProperty offset;
	private SerializedProperty clickMask;
	private SerializedProperty clickMarker;
	
	private SerializedProperty animator;
	private SerializedProperty isMovingParameterName;
	private SerializedProperty trackFacingDirection;
	private SerializedProperty hParameterName;
	private SerializedProperty vParameterName;

	private SerializedProperty displayDebugInfo;

	private void OnEnable ()
	{
		lockOnX = serializedObject.FindProperty("lockOnX");
		lockOnY = serializedObject.FindProperty("lockOnY");
		lockOnZ = serializedObject.FindProperty("lockOnZ");
		
		moveSpeed = serializedObject.FindProperty("moveSpeed");
		lookTowardsDestination = serializedObject.FindProperty("lookTowardsDestination");
		
		offset = serializedObject.FindProperty("offset");
		clickMask = serializedObject.FindProperty("clickMask");
		clickMarker = serializedObject.FindProperty("clickMarker");
		
		animator = serializedObject.FindProperty("animator");
		isMovingParameterName = serializedObject.FindProperty("isMovingParameterName");
		trackFacingDirection = serializedObject.FindProperty("trackFacingDirection");
		hParameterName = serializedObject.FindProperty("hParameterName");
		vParameterName = serializedObject.FindProperty("vParameterName");
		
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
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
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Lock", "Movement", "Click","Animation" }, GUILayout.MinHeight(25));

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
			0 => "Lock",
			1 => "Movement",
			2 => "Click",
			3 => "Animation",
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
			case "Lock":
			{
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(lockOnX);
					EditorGUILayout.PropertyField(lockOnY);
					EditorGUILayout.PropertyField(lockOnZ);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Movement":
			{
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(moveSpeed);
					EditorGUILayout.PropertyField(lookTowardsDestination);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Click":
			{
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(offset);
					EditorGUILayout.PropertyField(clickMask);
					EditorGUILayout.PropertyField(clickMarker);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case "Animation":
			{
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(animator);

					if (animator.Exists())
					{
						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							EditorGUILayout.PropertyField(isMovingParameterName);
							EditorGUILayout.PropertyField(trackFacingDirection);

							if (trackFacingDirection.boolValue)
							{
								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									EditorGUILayout.PropertyField(hParameterName);
									EditorGUILayout.PropertyField(vParameterName);
								}
								EditorGUILayout.EndVertical();
							}				
						}
						EditorGUILayout.EndVertical();
					}
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