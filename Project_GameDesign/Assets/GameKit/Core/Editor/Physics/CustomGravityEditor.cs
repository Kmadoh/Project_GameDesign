using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGravity)), CanEditMultipleObjects]
public class CustomGravityEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private SerializedProperty baseGravityForce;
	private SerializedProperty secondaryGravityForce;
	private SerializedProperty maxVelocity;

	private SerializedProperty invertOnInput;

	private SerializedProperty instantGravityChangeOnInput;
	private SerializedProperty invertJumpingDirection;

	private SerializedProperty onlyWhenGrounded;
	private SerializedProperty collisionCheckDistance;

	private SerializedProperty invertAnimationType;

	private SerializedProperty transformToInvert;
	private SerializedProperty invertedRotation;
	private SerializedProperty normalRotation;

	private SerializedProperty animator;
	private SerializedProperty gravityChangeBoolName;
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;

	private void OnEnable ()
	{
		baseGravityForce = serializedObject.FindProperty("baseGravityForce");
		secondaryGravityForce = serializedObject.FindProperty("secondaryGravityForce");
		maxVelocity = serializedObject.FindProperty("maxVelocity");

		invertOnInput = serializedObject.FindProperty("invertOnInput");

		instantGravityChangeOnInput = serializedObject.FindProperty("instantGravityChangeOnInput");
		invertJumpingDirection = serializedObject.FindProperty("invertJumpingDirection");

		onlyWhenGrounded = serializedObject.FindProperty("onlyWhenGrounded");
		collisionCheckDistance = serializedObject.FindProperty("collisionCheckDistance");

		invertAnimationType = serializedObject.FindProperty("invertAnimationType");

		transformToInvert = serializedObject.FindProperty("transformToInvert");
		invertedRotation = serializedObject.FindProperty("invertedRotation");
		normalRotation = serializedObject.FindProperty("normalRotation");

		animator = serializedObject.FindProperty("animator");
		gravityChangeBoolName = serializedObject.FindProperty("gravityChangeBoolName");
		
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
	}

	private void OnValidate()
	{
		UIHelper.InitializeStyles();
	}

	public override void OnInspectorGUI ()
	{   
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Gravity Inversion", "Forces", "Ground Check", "Animation" }, GUILayout.MinHeight(25));
			}
			EditorGUILayout.EndHorizontal();

			currentTab = toolBarTab switch
			{
				0 => "Gravity Inversion",
				1 => "Forces",
				2 => "Ground Check",
				3 => "Animation",
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
				case "Forces":
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(baseGravityForce);
					if (invertOnInput.boolValue)
						EditorGUILayout.PropertyField(secondaryGravityForce);
					EditorGUILayout.PropertyField(maxVelocity);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Gravity Inversion":
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(invertOnInput);

					if (invertOnInput.boolValue)
					{
						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
							inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
							
							EditorGUILayout.PropertyField(instantGravityChangeOnInput);
							EditorGUILayout.PropertyField(invertJumpingDirection);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "Ground Check":
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(onlyWhenGrounded);

					if (onlyWhenGrounded.boolValue)
					{
						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							EditorGUILayout.PropertyField(collisionCheckDistance);
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "Animation":

				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(invertAnimationType);
					if (invertAnimationType.intValue == (int)CustomGravity.InvertAnimationType.Rotation)
					{
						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							EditorGUILayout.PropertyField(transformToInvert);
							EditorGUILayout.PropertyField(invertedRotation);
							EditorGUILayout.PropertyField(normalRotation);
						}
						EditorGUILayout.EndVertical();
					}
					else if (invertAnimationType.intValue == (int)CustomGravity.InvertAnimationType.Animation)
					{
						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							EditorGUILayout.PropertyField(animator);
							if (animator.Exists())
							{
								EditorGUILayout.PropertyField(gravityChangeBoolName);
							}
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

			EditorGUILayout.Space();

			#region DebugMessages

			if (invertAnimationType.intValue == (int) CustomGravity.InvertAnimationType.Animation)
			{
				if (!animator.Exists())
				{
					EditorGUILayout.BeginVertical(UIHelper.warningStyle);
					{
						EditorGUILayout.LabelField("No Animator found, please add one !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}
				if (gravityChangeBoolName.stringValue == "")
				{
					EditorGUILayout.BeginVertical(UIHelper.warningStyle);
					{
						EditorGUILayout.LabelField("Parameter not set for animation !", EditorStyles.boldLabel);
					}
					EditorGUILayout.EndVertical();
				}
			}
			#endregion
		}
		EditorGUILayout.EndVertical();
	}
}