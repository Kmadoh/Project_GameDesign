using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mover)), CanEditMultipleObjects]
public class MoverEditor : Editor
{
	int toolBarTab;
	string currentTab;
	
	private SerializedProperty movementType;

	private SerializedProperty useXAxis;
	private SerializedProperty useYAxis;
	private SerializedProperty useZAxis;

	private SerializedProperty requiresInput;
	private SerializedProperty xInputName;
	private SerializedProperty yInputName;
	private SerializedProperty zInputName;

	private SerializedProperty xSpeed;
	private SerializedProperty ySpeed;
	private SerializedProperty zSpeed;
	private SerializedProperty maximumVelocity;
	private SerializedProperty lookAtDirection;
	private SerializedProperty isMovementLocal;

	private SerializedProperty useWallAvoidance;
	private SerializedProperty wallAvoidanceSize;
	private SerializedProperty wallAvoidanceOffset;
	private SerializedProperty wallAvoidanceLayers;

	private SerializedProperty animator;
	private SerializedProperty movementParticleSystem;
	private SerializedProperty minVelocityToToggle;

	private SerializedProperty xDirParameterName;
	private SerializedProperty yDirParameterName;
	private SerializedProperty zDirParameterName;

	private SerializedProperty keepOrientationOnIdle;

	private SerializedProperty trackSpeed;
	private SerializedProperty speedParameterName;

	private SerializedProperty animateWhenGroundedOnly;
	private SerializedProperty collisionCheckRadius;
	private SerializedProperty collisionOffset;
	private SerializedProperty groundedParameterName;
	private SerializedProperty groundLayerMask;

	private SerializedProperty useGravity;
	
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty xInputChoiceIndex;
	private SerializedProperty yInputChoiceIndex;
	private SerializedProperty zInputChoiceIndex;

	private void OnEnable ()
	{
		movementType = serializedObject.FindProperty("movementType");

		useXAxis = serializedObject.FindProperty("useXAxis");
		useYAxis = serializedObject.FindProperty("useYAxis");
		useZAxis = serializedObject.FindProperty("useZAxis");

		requiresInput = serializedObject.FindProperty("requiresInput");
		xInputName = serializedObject.FindProperty("xInputName");
		yInputName = serializedObject.FindProperty("yInputName");
		zInputName = serializedObject.FindProperty("zInputName");

		xSpeed = serializedObject.FindProperty("xSpeed");
		ySpeed = serializedObject.FindProperty("ySpeed");
		zSpeed = serializedObject.FindProperty("zSpeed");
		maximumVelocity = serializedObject.FindProperty("maximumVelocity");
		lookAtDirection = serializedObject.FindProperty("lookAtDirection");
		isMovementLocal = serializedObject.FindProperty("isMovementLocal");

		useWallAvoidance = serializedObject.FindProperty("useWallAvoidance");
		wallAvoidanceSize = serializedObject.FindProperty("wallAvoidanceSize");
		wallAvoidanceOffset = serializedObject.FindProperty("wallAvoidanceOffset");
		wallAvoidanceLayers = serializedObject.FindProperty("wallAvoidanceLayers");

		animator = serializedObject.FindProperty("animator");
		movementParticleSystem = serializedObject.FindProperty("movementParticleSystem");
		minVelocityToToggle = serializedObject.FindProperty("minVelocityToToggle");

		xDirParameterName = serializedObject.FindProperty("xDirParameterName");
		yDirParameterName = serializedObject.FindProperty("yDirParameterName");
		zDirParameterName = serializedObject.FindProperty("zDirParameterName");

		keepOrientationOnIdle = serializedObject.FindProperty("keepOrientationOnIdle");
		trackSpeed = serializedObject.FindProperty("trackSpeed");
		speedParameterName = serializedObject.FindProperty("speedParameterName");

		animateWhenGroundedOnly = serializedObject.FindProperty("animateWhenGroundedOnly");
		collisionCheckRadius = serializedObject.FindProperty("collisionCheckRadius");
		collisionOffset = serializedObject.FindProperty("collisionOffset");
		groundedParameterName = serializedObject.FindProperty("groundedParameterName");
		groundLayerMask = serializedObject.FindProperty("groundLayerMask");

		useGravity = serializedObject.FindProperty("useGravity");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		
		xInputChoiceIndex = serializedObject.FindProperty("xInputChoiceIndex");
		yInputChoiceIndex = serializedObject.FindProperty("yInputChoiceIndex");
		zInputChoiceIndex = serializedObject.FindProperty("zInputChoiceIndex");
	}


	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(movementType);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
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
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Axis Constraints", "Input", "Movement", "Animation" });

			currentTab = toolBarTab switch
			{
				0 => "Axis Constraints",
				1 => "Input",
				2 => "Movement",
				3 => "Animation",
				_ => currentTab
			};

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				GUI.FocusControl(null);
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				switch (currentTab)
				{
					case "Axis Constraints":
						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							EditorGUILayout.PropertyField(useXAxis);
							EditorGUILayout.PropertyField(useYAxis);
							EditorGUILayout.PropertyField(useZAxis);
						}
						EditorGUILayout.EndVertical();
						break;

					case "Input":
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(requiresInput);
						if (requiresInput.boolValue && (useXAxis.boolValue || useYAxis.boolValue || useZAxis.boolValue))
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								if (useXAxis.boolValue)
								{
									xInputChoiceIndex.intValue = EditorGUILayout.Popup("X Axis Input : ", xInputChoiceIndex.intValue, Helper.GetInputAxes());
									xInputName.stringValue = Helper.GetInputAxes()[xInputChoiceIndex.intValue];
								}
								if (useYAxis.boolValue)
								{
									yInputChoiceIndex.intValue = EditorGUILayout.Popup("Y Axis Input : ", yInputChoiceIndex.intValue, Helper.GetInputAxes());
									yInputName.stringValue = Helper.GetInputAxes()[yInputChoiceIndex.intValue];
								}
								if (useZAxis.boolValue)
								{
									zInputChoiceIndex.intValue = EditorGUILayout.Popup("Z Axis Input : ", zInputChoiceIndex.intValue, Helper.GetInputAxes());
									zInputName.stringValue = Helper.GetInputAxes()[zInputChoiceIndex.intValue];
								}
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
					break;

					case "Movement":
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{

						EditorGUILayout.PropertyField(isMovementLocal);

						if (useXAxis.boolValue)
						{
							EditorGUILayout.PropertyField(xSpeed);
						}
						if (useYAxis.boolValue)
						{
							EditorGUILayout.PropertyField(ySpeed);
						}
						if (useZAxis.boolValue)
						{
							EditorGUILayout.PropertyField(zSpeed);
						}
						
						switch (movementType.intValue)
						{
							case (int)Mover.MovementType.Rigidbody:
								EditorGUILayout.PropertyField(maximumVelocity);
								break;
							case (int)Mover.MovementType.Controller:
								EditorGUILayout.PropertyField(useGravity);
								break;
							case (int)Mover.MovementType.Translation:
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						EditorGUILayout.PropertyField(lookAtDirection);

						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							EditorGUILayout.PropertyField(useWallAvoidance);
							if (useWallAvoidance.boolValue)
							{
								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									EditorGUILayout.PropertyField(wallAvoidanceSize);
									EditorGUILayout.PropertyField(wallAvoidanceOffset);
									EditorGUILayout.PropertyField(wallAvoidanceLayers);
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndVertical();
					break;

					case "Animation":

					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(animator);
						if (animator.propertyType == SerializedPropertyType.ObjectReference && animator.objectReferenceValue != null)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								if (useXAxis.boolValue)
								{
									EditorGUILayout.PropertyField(xDirParameterName);
								}
								if (useYAxis.boolValue)
								{
									EditorGUILayout.PropertyField(yDirParameterName);
								}
								if (useZAxis.boolValue)
								{
									EditorGUILayout.PropertyField(zDirParameterName);
								}
							}
							EditorGUILayout.EndVertical();

							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(keepOrientationOnIdle);
							}
							EditorGUILayout.EndVertical();


							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(trackSpeed);
								if (trackSpeed.boolValue)
								{
									EditorGUILayout.BeginVertical(UIHelper.subStyle2);
									{
										EditorGUILayout.PropertyField(speedParameterName);
									}
									EditorGUILayout.EndVertical();
								}
							}
							EditorGUILayout.EndVertical();

							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(animateWhenGroundedOnly);
								if (animateWhenGroundedOnly.boolValue)
								{
									EditorGUILayout.BeginVertical(UIHelper.subStyle2);
									{
										EditorGUILayout.PropertyField(collisionCheckRadius);
										EditorGUILayout.PropertyField(collisionOffset);
										EditorGUILayout.PropertyField(groundedParameterName);
										EditorGUILayout.PropertyField(groundLayerMask);
									}
									EditorGUILayout.EndVertical();
								}
							}
							EditorGUILayout.EndVertical();
						}

						EditorGUILayout.PropertyField(movementParticleSystem);
						if (movementParticleSystem.Exists())
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(minVelocityToToggle);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
					
					break;
				}
			}
			EditorGUILayout.EndVertical();

		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		EditorGUILayout.Space();

		#region DebugMessages

		switch (toolBarTab)
		{
			case 0:
			if (!useXAxis.boolValue && !useYAxis.boolValue && !useZAxis.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Add at least one axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 1:
			if (xInputName.stringValue == "" && useXAxis.boolValue && requiresInput.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for X Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (yInputName.stringValue == "" && useYAxis.boolValue && requiresInput.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for Y Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (zInputName.stringValue == "" && useZAxis.boolValue && requiresInput.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Input name not set for Z Axis !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 2:
			if (xSpeed.floatValue == 0 && useXAxis.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("X Speed is equal to 0. Either disable useXAxis or set a speed to X Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (ySpeed.floatValue == 0 && useYAxis.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Y Speed is equal to 0. Either disable useYAxis or set a speed to Y Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			if (zSpeed.floatValue == 0 && useZAxis.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Z Speed is equal to 0. Either disable useZAxis or set a speed to Z Axis", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			break;

			case 3:

			break;
		}

		#endregion
	}
}
