using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseDummyClass)), CanEditMultipleObjects]
public class BaseTemplate : Editor
{
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;
	
	private SerializedProperty displayDebugInfo;
	
	private SerializedProperty minFloatValue;
	private SerializedProperty maxFloatValue;

	private SerializedProperty sceneChoiceIndex;
	private SerializedProperty sceneToLoad;

	private void OnEnable ()
	{
		displayDebugInfo = serializedObject.FindProperty("displayDebugInfo");
		
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
		
		minFloatValue = serializedObject.FindProperty("minFloatValue");
		maxFloatValue = serializedObject.FindProperty("maxFloatValue");		
		
		sceneChoiceIndex = serializedObject.FindProperty("sceneChoiceIndex");
		sceneToLoad = serializedObject.FindProperty("sceneToLoad");
	}

	public override void OnInspectorGUI ()
	{
		InitGUI();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(displayDebugInfo);
			
		}
		EditorGUILayout.EndVertical();
		
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}

	private void InitGUI()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
	}
	
	public void Slider()
	{
		EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
		{
			Vector2 minMaxValue = new Vector2(minFloatValue.floatValue, maxFloatValue.floatValue);
			EditorGUILayout.LabelField("Min Max Slider : ", GUILayout.MaxWidth(100f));
			minFloatValue.floatValue = EditorGUILayout.FloatField(minFloatValue.floatValue, GUILayout.MaxWidth(50f));
				
			EditorGUILayout.MinMaxSlider(ref minMaxValue.x, ref minMaxValue.y, -10f, 10f);

			maxFloatValue.floatValue = EditorGUILayout.FloatField(maxFloatValue.floatValue, GUILayout.MaxWidth(50f));

			minFloatValue.floatValue = minMaxValue.x;
			maxFloatValue.floatValue = minMaxValue.y;
		}
		EditorGUILayout.EndHorizontal();
	}

	private void SceneSelectionSlider()
	{
		EditorGUILayout.BeginVertical(UIHelper.subStyle1);
		{
			sceneChoiceIndex.intValue = EditorGUILayout.Popup("Scene To Load : ", sceneChoiceIndex.intValue, Helper.GetSceneNames());
			sceneToLoad.stringValue = Helper.GetSceneNames()[sceneChoiceIndex.intValue];
		}
		EditorGUILayout.EndVertical();
	}
	
	private void InputSelectionSlider()
	{
		EditorGUILayout.BeginVertical(UIHelper.subStyle1);
		{
			inputChoiceIndex.intValue = EditorGUILayout.Popup("Input Axis : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
			inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
		}
		EditorGUILayout.EndVertical();
	}

	private void HandleDebugModeButton()
	{
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
}