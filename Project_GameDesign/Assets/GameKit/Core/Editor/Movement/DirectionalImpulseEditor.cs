using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DirectionalImpulse)), CanEditMultipleObjects]
public class DirectionalImpulseEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;
	
	private SerializedProperty directionInputType;
	private SerializedProperty horizontalIndex;
	private SerializedProperty horizontalAxis;
	private SerializedProperty horizontalAxisName;

	private SerializedProperty verticalIndex;
	private SerializedProperty verticalAxis;
	private SerializedProperty verticalAxisName;

	private SerializedProperty impulseInputName;

	private SerializedProperty depthAxis;

	private SerializedProperty resetVelocityOnImpulse;
	private SerializedProperty impulseForce;

	private SerializedProperty useCooldown;
	private SerializedProperty cooldown;

	private SerializedProperty minimalHeightToImpulse;
	private SerializedProperty heightAxis;
	private SerializedProperty groundDetectionLayerMask;
	private SerializedProperty collisionOffset;

	private SerializedProperty requireResources;
	private SerializedProperty resourceManager;
	private SerializedProperty resourceIndex;
	private SerializedProperty resourceCostOnUse;

	private SerializedProperty preventMovingAfterImpulse;
	private SerializedProperty mover;
	private SerializedProperty movePreventionDuration;

	private SerializedProperty fXFeedbackType;
	private SerializedProperty impulseParticleSystem;
	//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
	//private SerializedProperty impulseVFX;
	//private SerializedProperty impulseEventName;
	private SerializedProperty spawnedFX;
	private SerializedProperty fxOffset;
	private SerializedProperty timeBeforeDestroyFX;

	private SerializedProperty animator;
	private SerializedProperty dashTriggerName;
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty impulseIndex;

	private void OnEnable ()
	{
		directionInputType = serializedObject.FindProperty("directionInputType");
		horizontalAxis = serializedObject.FindProperty("horizontalAxis");
		horizontalIndex = serializedObject.FindProperty("horizontalIndex");
		horizontalAxisName = serializedObject.FindProperty("horizontalAxisName");
		
		impulseIndex = serializedObject.FindProperty("impulseIndex");
		impulseInputName = serializedObject.FindProperty("impulseInputName");

		verticalAxis = serializedObject.FindProperty("verticalAxis");
		verticalIndex = serializedObject.FindProperty("verticalIndex");
		verticalAxisName = serializedObject.FindProperty("verticalAxisName");

		impulseInputName = serializedObject.FindProperty("impulseInputName");
		depthAxis = serializedObject.FindProperty("depthAxis");

		resetVelocityOnImpulse = serializedObject.FindProperty("resetVelocityOnImpulse");
		impulseForce = serializedObject.FindProperty("impulseForce");

		useCooldown = serializedObject.FindProperty("useCooldown");
		cooldown = serializedObject.FindProperty("cooldown");

		minimalHeightToImpulse = serializedObject.FindProperty("minimalHeightToImpulse");
		heightAxis = serializedObject.FindProperty("heightAxis");
		groundDetectionLayerMask = serializedObject.FindProperty("groundDetectionLayerMask");
		collisionOffset = serializedObject.FindProperty("collisionOffset");

		requireResources = serializedObject.FindProperty("requireResources");
		resourceManager = serializedObject.FindProperty("resourceManager");
		resourceIndex = serializedObject.FindProperty("resourceIndex");
		resourceCostOnUse = serializedObject.FindProperty("resourceCostOnUse");

		preventMovingAfterImpulse = serializedObject.FindProperty("preventMovingAfterImpulse");
		mover = serializedObject.FindProperty("mover");
		movePreventionDuration = serializedObject.FindProperty("movePreventionDuration");

		impulseParticleSystem = serializedObject.FindProperty("impulseParticleSystem");

		//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
		//impulseVFX = soTarget.FindProperty("impulseVFX");
		//impulseEventName = soTarget.FindProperty("impulseEventName");

		fXFeedbackType = serializedObject.FindProperty("fXFeedbackType");
		spawnedFX = serializedObject.FindProperty("spawnedFX");
		fxOffset = serializedObject.FindProperty("fxOffset");
		timeBeforeDestroyFX = serializedObject.FindProperty("timeBeforeDestroyFX");

		horizontalIndex = serializedObject.FindProperty("horizontalIndex");
		
		animator = serializedObject.FindProperty("animator");
		
		dashTriggerName = serializedObject.FindProperty("dashTriggerName");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");

	}

	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
			{

				toolBarTab = GUILayout.SelectionGrid(toolBarTab, new string[] { "Input", "Force", "Cooldown", "Collision", "Resources", "Movement Prevention", "FX", "Animation" }, 4, GUILayout.MinHeight(40));

				if (displayDebugInfo.boolValue)
				{
					if (GUILayout.Button("Debug ON", UIHelper.greenButtonStyle, GUILayout.MaxHeight(40f), GUILayout.MaxWidth(100f)))
					{
						displayDebugInfo.boolValue = !displayDebugInfo.boolValue;
					}
				}
				else
				{
					if (GUILayout.Button("Debug OFF", UIHelper.redButtonStyle, GUILayout.MaxHeight(40f), GUILayout.MaxWidth(100f)))
					{
						displayDebugInfo.boolValue = !displayDebugInfo.boolValue;
					}
				}
			}
			EditorGUILayout.EndHorizontal();


			switch (toolBarTab)
			{
				case 0:
				currentTab = "Input";
				break;

				case 1:
				currentTab = "Force";
				break;

				case 2:
				currentTab = "Cooldown";
				break;

				case 3:
				currentTab = "Collision";
				break;

				case 4:
				currentTab = "Resources";
				break;

				case 5:
				currentTab = "Movement";
				break;

				case 6:
				currentTab = "FX";
				break;

				case 7:
				currentTab = "Animation";
				break;
			}

			//Apply modified properties to avoid data loss
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				GUI.FocusControl(null);
			}

			EditorGUI.BeginChangeCheck();

			switch (currentTab)
			{
				case "Input":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					impulseIndex.intValue = EditorGUILayout.Popup("Impulse Input : ", impulseIndex.intValue, Helper.GetInputAxes());
					impulseInputName.stringValue = Helper.GetInputAxes()[impulseIndex.intValue];

					EditorGUILayout.Space();

					EditorGUILayout.PropertyField(directionInputType);
					
					switch (directionInputType.intValue)
					{
						case (int)DirectionalImpulse.DirectionInputType.Axis:
						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							EditorGUILayout.PropertyField(horizontalAxis);
							if (horizontalAxis.intValue != (int)DirectionalImpulse.Axis.None)
							{
								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									horizontalIndex.intValue = EditorGUILayout.Popup("Horizontal Axis : ", horizontalIndex.intValue, Helper.GetInputAxes());
									horizontalAxisName.stringValue = Helper.GetInputAxes()[horizontalIndex.intValue];
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							EditorGUILayout.PropertyField(verticalAxis);
							if (verticalAxis.intValue != (int)DirectionalImpulse.Axis.None)
							{
								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									verticalIndex.intValue = EditorGUILayout.Popup("Vertical Axis : ", verticalIndex.intValue, Helper.GetInputAxes());
									verticalAxisName.stringValue = Helper.GetInputAxes()[verticalIndex.intValue];
								}
								EditorGUILayout.EndVertical();
							}
						}
						EditorGUILayout.EndVertical();
						break;

						case (int)DirectionalImpulse.DirectionInputType.MousePos:
						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							EditorGUILayout.PropertyField(depthAxis);
						}
						EditorGUILayout.EndVertical();
						break;
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "Force":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(resetVelocityOnImpulse);
					EditorGUILayout.PropertyField(impulseForce);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Cooldown":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(useCooldown);

						if (useCooldown.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(cooldown);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "Collision":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(minimalHeightToImpulse);

						if (minimalHeightToImpulse.floatValue > 0)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(heightAxis);
								EditorGUILayout.PropertyField(groundDetectionLayerMask);
								EditorGUILayout.PropertyField(collisionOffset);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "Resources":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(requireResources);

						if (requireResources.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
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
				EditorGUILayout.EndVertical();
				break;

				case "Movement":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(preventMovingAfterImpulse);

						if (preventMovingAfterImpulse.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(mover);
								EditorGUILayout.PropertyField(movePreventionDuration);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "FX":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(fXFeedbackType);
						switch (fXFeedbackType.intValue)
						{
							case (int)Helper.FXFeedbackType.Instantiate:
								{
									EditorGUILayout.BeginVertical(UIHelper.subStyle2);
									{
										EditorGUILayout.PropertyField(spawnedFX);
										if (spawnedFX.propertyType == SerializedPropertyType.ObjectReference && spawnedFX.objectReferenceValue == null)
										{
											EditorGUILayout.PropertyField(fxOffset);
											EditorGUILayout.PropertyField(timeBeforeDestroyFX);
										}
									}
									EditorGUILayout.EndVertical();
								}
							break;

							case (int)Helper.FXFeedbackType.ParticleSystem:
								{
									EditorGUILayout.BeginVertical(UIHelper.subStyle2);
									{
										EditorGUILayout.PropertyField(impulseParticleSystem);
									}
									EditorGUILayout.EndVertical();
								}
							break;

							case (int)Helper.FXFeedbackType.VFXGraph:
								{
									EditorGUILayout.BeginVertical(UIHelper.subStyle2);
									{
										//UNCOMMENT ONLY IF USING VFX GRAPH PACKAGE
										//EditorGUILayout.PropertyField(impulseVFX);
										//EditorGUILayout.PropertyField(impulseEventName);

										//Delete those 3 lines if using VFX Graph package !
										EditorGUILayout.BeginVertical(UIHelper.warningStyle);
										{
											EditorGUILayout.LabelField("VFX Not working, please uncomment lines of code in DirectionalImpulse and DirectionalImpulseEditor scripts", EditorStyles.boldLabel);
										}
										EditorGUILayout.EndVertical();
									}
									EditorGUILayout.EndVertical();
								}
								break;
						}

						
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;

				case "Animation":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.BeginVertical(UIHelper.subStyle1);
					{
						EditorGUILayout.PropertyField(animator);

						if (animator.propertyType == SerializedPropertyType.ObjectReference && animator.objectReferenceValue != null)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle2);
							{
								EditorGUILayout.PropertyField(dashTriggerName);
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
				break;
			}

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			EditorGUILayout.Space();

			// Can be used to display contextual error messages
			#region DebugMessages
			if (preventMovingAfterImpulse.boolValue && (mover.propertyType == SerializedPropertyType.ObjectReference && mover.objectReferenceValue == null))
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Mover Component not found ! If you want to prevent movement, start by adding a mover !", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}

			if (fXFeedbackType.intValue == (int)Helper.FXFeedbackType.Instantiate && (spawnedFX.propertyType == SerializedPropertyType.ObjectReference && spawnedFX.objectReferenceValue == null))
			{
				EditorGUILayout.BeginVertical(UIHelper.warningStyle);
				{
					EditorGUILayout.LabelField("Spawned FX not found ! Please reference a prefab to spawn from the Project folder", EditorStyles.boldLabel);
				}
				EditorGUILayout.EndVertical();
			}
			#endregion
		}
		EditorGUILayout.EndVertical();
	}
}