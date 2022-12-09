using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FirstPersonCamera)), CanEditMultipleObjects]
public class FirstPersonCameraEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	private int toolBarTab;
	private string currentTab;
	
	[Tooltip("Used to check public variables from the target class")]
	private FirstPersonCamera myObject;

	private SerializedProperty transformToRotateWithCamera;
	private SerializedProperty mouseXInputName;
	private SerializedProperty mouseYInputName;
	
	private SerializedProperty mouseSensitivityX;
	private SerializedProperty mouseSensitivityY;
	
	private SerializedProperty xInputChoiceIndex;
	private SerializedProperty yInputChoiceIndex;
	
	private SerializedProperty minXAxisClamp;
	private SerializedProperty maxXAxisClamp;
	
	private SerializedProperty displayDebugInfo;



	private void OnEnable ()
	{
		myObject = (FirstPersonCamera)target;

		transformToRotateWithCamera = serializedObject.FindProperty("transformToRotateWithCamera");
		
		mouseXInputName = serializedObject.FindProperty("mouseXInputName");
		mouseYInputName = serializedObject.FindProperty("mouseYInputName");
		
		mouseSensitivityX = serializedObject.FindProperty("mouseSensitivityX");
		mouseSensitivityY = serializedObject.FindProperty("mouseSensitivityY");
		
		xInputChoiceIndex = serializedObject.FindProperty("xInputChoiceIndex");
		yInputChoiceIndex = serializedObject.FindProperty("yInputChoiceIndex");
		
		minXAxisClamp = serializedObject.FindProperty("minXAxisClamp");
		maxXAxisClamp = serializedObject.FindProperty("maxXAxisClamp");
		
		maxXAxisClamp = serializedObject.FindProperty("maxXAxisClamp");
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");

	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();
		
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(transformToRotateWithCamera);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
			{
				toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Input", "Sensitivity", "X Angle Clamp"}, GUILayout.MinHeight(25));

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
		EditorGUILayout.EndVertical();

		currentTab = toolBarTab switch
		{
			0 => "Input",
			1 => "Sensitivity",
			2 => "X Angle Clamp",
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
			case "Input": 
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					xInputChoiceIndex.intValue = EditorGUILayout.Popup("Mouse X : ", xInputChoiceIndex.intValue, Helper.GetInputAxes());
					mouseXInputName.stringValue = Helper.GetInputAxes()[xInputChoiceIndex.intValue];
					
					yInputChoiceIndex.intValue = EditorGUILayout.Popup("Mouse Y : ", yInputChoiceIndex.intValue, Helper.GetInputAxes());
					mouseYInputName.stringValue = Helper.GetInputAxes()[yInputChoiceIndex.intValue];
				}
				EditorGUILayout.EndVertical();
				break;
			
			case "Sensitivity":
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(mouseSensitivityX);
					EditorGUILayout.PropertyField(mouseSensitivityY);
				}
				EditorGUILayout.EndVertical();
				break;

			case "X Angle Clamp":
				EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
				{
					EditorGUILayout.LabelField("X Angle Clamp : ", GUILayout.MaxWidth(50f));

					minXAxisClamp.floatValue = EditorGUILayout.FloatField(minXAxisClamp.floatValue, GUILayout.MaxWidth(50f));
					EditorGUILayout.MinMaxSlider(ref myObject.minXAxisClamp, ref myObject.maxXAxisClamp, -90f, 90f);
					maxXAxisClamp.floatValue = EditorGUILayout.FloatField(maxXAxisClamp.floatValue, GUILayout.MaxWidth(50f));
				}
				EditorGUILayout.EndHorizontal();
			break;
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}