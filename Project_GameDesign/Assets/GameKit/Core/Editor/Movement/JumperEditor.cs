using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Jumper)), CanEditMultipleObjects]
public class JumperEditor : Editor
{
	int toolBarTab;
	string currentTab;
	
	private SerializedProperty jumpForce;
	private SerializedProperty coyoteTimeDuration;
	private SerializedProperty jumpLayerMask;
	private SerializedProperty fallGravityMultiplier;
	private SerializedProperty jumpEndYVelocity;
	private SerializedProperty lowJumpGravityMultiplier;

	private SerializedProperty jumpAmount;
	private SerializedProperty airJumpForce;
	private SerializedProperty resetVelocityOnAirJump;

	private SerializedProperty useCollisionCheck;
	private SerializedProperty collisionOffset;
	private SerializedProperty collisionCheckRadius;
	private SerializedProperty characterHeight;

	private SerializedProperty jumpFX;
	private SerializedProperty airJumpFX;
	private SerializedProperty FXOffset;
	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty jumpTriggerName;
	private SerializedProperty airJumpTriggerName;
	
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty jumpInputName;

	private void OnEnable ()
	{
		jumpForce = serializedObject.FindProperty("jumpForce");
		coyoteTimeDuration = serializedObject.FindProperty("coyoteTimeDuration");
		jumpLayerMask = serializedObject.FindProperty("jumpLayerMask");

		jumpEndYVelocity = serializedObject.FindProperty("jumpEndYVelocity");
		fallGravityMultiplier = serializedObject.FindProperty("fallGravityMultiplier");
		lowJumpGravityMultiplier = serializedObject.FindProperty("lowJumpGravityMultiplier");

		jumpAmount = serializedObject.FindProperty("jumpAmount");
		airJumpForce = serializedObject.FindProperty("airJumpForce");
		resetVelocityOnAirJump = serializedObject.FindProperty("resetVelocityOnAirJump");

		useCollisionCheck = serializedObject.FindProperty("useCollisionCheck");
		collisionOffset = serializedObject.FindProperty("collisionOffset");
		collisionCheckRadius = serializedObject.FindProperty("collisionCheckRadius");
		characterHeight = serializedObject.FindProperty("characterHeight");

		jumpFX = serializedObject.FindProperty("jumpFX");
		airJumpFX = serializedObject.FindProperty("airJumpFX");
		FXOffset = serializedObject.FindProperty("FXOffset");
		timeBeforeDestroyFX = serializedObject.FindProperty("timeBeforeDestroyFX");

		animator = serializedObject.FindProperty("animator");
		jumpTriggerName = serializedObject.FindProperty("jumpTriggerName");
		airJumpTriggerName = serializedObject.FindProperty("airJumpTriggerName");
		
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		jumpInputName = serializedObject.FindProperty("jumpInputName");
	}

	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Jump", "Air Jump", "Collision", "FX", "Animation" }, GUILayout.MinHeight(25));
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
			currentTab = "Jump";
			break;

			case 1:
			currentTab = "Air Jump";
			break;

			case 2:
			currentTab = "Collision";
			break;

			case 3:
			currentTab = "FX";
			break;

			case 4:
			currentTab = "Animation";
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
			case "Jump":
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
				jumpInputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
				
				EditorGUILayout.PropertyField(jumpForce);
				EditorGUILayout.PropertyField(coyoteTimeDuration);
				EditorGUILayout.PropertyField(jumpLayerMask);

				EditorGUILayout.Separator();

				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.LabelField("Better Jump", EditorStyles.boldLabel);
					EditorGUILayout.PropertyField(jumpEndYVelocity);
					EditorGUILayout.PropertyField(fallGravityMultiplier);
					EditorGUILayout.PropertyField(lowJumpGravityMultiplier);
				}
				EditorGUILayout.EndVertical();
				
			}
			EditorGUILayout.EndVertical();
			break;

			case "Air Jump":

			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				EditorGUILayout.IntSlider(jumpAmount, 1, 10);

				if (jumpAmount.intValue > 1)
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(airJumpForce);
						EditorGUILayout.PropertyField(resetVelocityOnAirJump);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "Collision":
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				EditorGUILayout.PropertyField(useCollisionCheck);
				if (useCollisionCheck.boolValue)
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(collisionOffset);
						EditorGUILayout.PropertyField(collisionCheckRadius);
						EditorGUILayout.PropertyField(characterHeight);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "FX":
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				EditorGUILayout.PropertyField(jumpFX);
				EditorGUILayout.PropertyField(airJumpFX);

				if ((jumpFX.propertyType == SerializedPropertyType.ObjectReference && jumpFX.objectReferenceValue != null) || 
				    (airJumpFX.propertyType == SerializedPropertyType.ObjectReference && airJumpFX.objectReferenceValue != null))
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(FXOffset);
						EditorGUILayout.PropertyField(timeBeforeDestroyFX);
					}
					EditorGUILayout.EndVertical();
				}

			}
			EditorGUILayout.EndVertical();
			break;

			case "Animation":
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				EditorGUILayout.PropertyField(animator);

				if(animator.propertyType == SerializedPropertyType.ObjectReference && animator.objectReferenceValue != null)
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(jumpTriggerName);
						EditorGUILayout.PropertyField(airJumpTriggerName);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
