using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallJump)), CanEditMultipleObjects]
public class WallJumpEditor : Editor
{
	private int toolBarTab;
	private string currentTab;
	
	private SerializedProperty wallJumpForce;
	private SerializedProperty wallJumpLayerMask;
	private SerializedProperty invertLookDirectionOnWallJump;
	private SerializedProperty gravityDampenWhileOnWall;

	private SerializedProperty preventMovingAfterWallJump;
	private SerializedProperty movePreventionDuration;
	private SerializedProperty mover;

	private SerializedProperty resetJumpCountOnWallJump;
	private SerializedProperty jumper;

	private SerializedProperty halfExtents;
	private SerializedProperty collisionOffset;
	private SerializedProperty minimalHeightAllowedToWallJump;

	private SerializedProperty jumpFX;
	private SerializedProperty jumpFXSpawnPoint;

	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty wallJumpTriggerName;
	
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty wallJumpInputName;
	
	private void OnEnable ()
	{
		wallJumpForce = serializedObject.FindProperty("wallJumpForce");
		wallJumpLayerMask = serializedObject.FindProperty("wallJumpLayerMask");
		invertLookDirectionOnWallJump = serializedObject.FindProperty("invertLookDirectionOnWallJump");
		gravityDampenWhileOnWall = serializedObject.FindProperty("gravityDampenWhileOnWall");

		preventMovingAfterWallJump = serializedObject.FindProperty("preventMovingAfterWallJump");
		movePreventionDuration = serializedObject.FindProperty("movePreventionDuration");
		mover = serializedObject.FindProperty("mover");

		resetJumpCountOnWallJump = serializedObject.FindProperty("resetJumpCountOnWallJump");
		jumper = serializedObject.FindProperty("jumper");

		halfExtents = serializedObject.FindProperty("halfExtents");
		collisionOffset = serializedObject.FindProperty("collisionOffset");
		minimalHeightAllowedToWallJump = serializedObject.FindProperty("minimalHeightAllowedToWallJump");

		jumpFX = serializedObject.FindProperty("jumpFX");
		jumpFXSpawnPoint = serializedObject.FindProperty("jumpFXSpawnPoint");
		timeBeforeDestroyFX = serializedObject.FindProperty("timeBeforeDestroyFX");

		animator = serializedObject.FindProperty("animator");
		wallJumpTriggerName = serializedObject.FindProperty("wallJumpTriggerName");
		
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		wallJumpInputName = serializedObject.FindProperty("wallJumpInputName");
	}

	private void OnValidate()
	{
		UIHelper.InitializeStyles();
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Wall Jump", "Synergies", "Collision", "FX", "Animation" });
		EditorGUILayout.Space();

		currentTab = toolBarTab switch
		{
			0 => "Wall Jump",
			1 => "Synergies",
			2 => "Collision",
			3 => "FX",
			4 => "Animation",
			_ => currentTab
		};

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
			GUI.FocusControl(null);
		}

		EditorGUI.BeginChangeCheck();

		switch (currentTab)
		{
			case "Wall Jump":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
					wallJumpInputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
					
					EditorGUILayout.PropertyField(wallJumpForce);
					EditorGUILayout.PropertyField(wallJumpLayerMask);
					EditorGUILayout.PropertyField(invertLookDirectionOnWallJump);
					EditorGUILayout.PropertyField(gravityDampenWhileOnWall);
				}
				EditorGUILayout.EndVertical();
			break;

			case "Synergies":
				
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				EditorGUILayout.PropertyField(preventMovingAfterWallJump);

				if (preventMovingAfterWallJump.boolValue)
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(mover);
						EditorGUILayout.PropertyField(movePreventionDuration);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{

				EditorGUILayout.PropertyField(resetJumpCountOnWallJump);

				if (resetJumpCountOnWallJump.boolValue)
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(jumper);
					}
					EditorGUILayout.EndVertical();
				}
			}
			EditorGUILayout.EndVertical();
			break;

			case "Collision":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(halfExtents);
					EditorGUILayout.PropertyField(minimalHeightAllowedToWallJump);
					EditorGUILayout.PropertyField(collisionOffset);
				}
				EditorGUILayout.EndVertical();

				break;

			case "FX":
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				EditorGUILayout.PropertyField(jumpFX);

				if (jumpFX.Exists())
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(jumpFXSpawnPoint);
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

				if(animator.Exists())
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(wallJumpTriggerName);
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