using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackOnInput)), CanEditMultipleObjects]
public class AttackOnInputEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;

	private AttackOnInput myObject;
	
	private SerializedProperty attackLayerMask;
	private SerializedProperty attackAngle;
	private SerializedProperty attackRange;
	private SerializedProperty damageDelay;
	private SerializedProperty attackCooldown;
	private SerializedProperty timer;

	private SerializedProperty attackDamage;
	private SerializedProperty attackKnockback;
	private SerializedProperty attackUpwardsKnockback;
	private SerializedProperty hitFX;

	private SerializedProperty attackTriggerParameterName;
	private SerializedProperty animator;
	
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;
	
	private void OnEnable ()
	{
		myObject = (AttackOnInput)target;

		attackLayerMask = serializedObject.FindProperty("attackLayerMask");
		attackAngle = serializedObject.FindProperty("attackAngle");
		attackRange = serializedObject.FindProperty("attackRange");
		damageDelay = serializedObject.FindProperty("damageDelay");
		attackCooldown = serializedObject.FindProperty("attackCooldown");
		timer = serializedObject.FindProperty("timer");

		attackDamage = serializedObject.FindProperty("attackDamage");
		attackKnockback = serializedObject.FindProperty("attackKnockback");
		attackUpwardsKnockback = serializedObject.FindProperty("attackUpwardsKnockback");
		hitFX = serializedObject.FindProperty("hitFX");

		attackTriggerParameterName = serializedObject.FindProperty("attackTriggerParameterName");
		animator = serializedObject.FindProperty("animator");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
	}

	public override void OnInspectorGUI ()
	{
		LayerMaskUtils.GetLayerNames(ref Helper.layers);
		UIHelper.InitializeStyles();
		
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			if(displayDebugInfo.boolValue)
			{
				if (attackCooldown.floatValue > 0f)
				{
					EditorGUI.ProgressBar(new Rect(20, 20, EditorGUIUtility.currentViewWidth - 40, 20),
						1f - (timer.floatValue / attackCooldown.floatValue),
						timer.floatValue > 0f ? "Cooldown" : "Can attack now !");
				}

				EditorGUILayout.Space(30f);
			}

			EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "General", "Hit", "Animation", "FX"}, GUILayout.MinHeight(25));

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
				1 => "Hit",
				2 => "Animation",
				3 => "FX",
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
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					inputChoiceIndex.intValue = EditorGUILayout.Popup("Input", inputChoiceIndex.intValue, Helper.GetInputAxes());
					inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];

					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField("Attack Layer Mask");
						attackLayerMask.intValue = EditorGUILayout.MaskField(attackLayerMask.intValue, Helper.layers);
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.PropertyField(attackAngle);
					EditorGUILayout.PropertyField(attackRange);
					EditorGUILayout.PropertyField(damageDelay);
					EditorGUILayout.PropertyField(attackCooldown);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Hit":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(attackDamage);
					EditorGUILayout.PropertyField(attackKnockback);
					EditorGUILayout.PropertyField(attackUpwardsKnockback);
				}
				EditorGUILayout.EndVertical();
				break;

				case "Animation":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(animator);

					if (animator.Exists())
					{
						EditorGUILayout.PropertyField(attackTriggerParameterName);
					}
				}
				EditorGUILayout.EndVertical();
				break;

				case "FX":
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					EditorGUILayout.PropertyField(hitFX);
				}
				EditorGUILayout.EndVertical();
				break;
			}

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
			
			// Can be used to display contextual error messages
			#region DebugMessages

			switch (toolBarTab)
			{
				case 0:

				break;

				case 1:

				break;

				case 2:

				break;
			}
			#endregion
		}
		EditorGUILayout.EndVertical();
	}

	private void OnSceneGUI ()
	{
		if(displayDebugInfo.boolValue)
		{
			UIHelper.DrawArc(attackAngle.floatValue, attackRange.floatValue, myObject.transform);
		}
	}
}
