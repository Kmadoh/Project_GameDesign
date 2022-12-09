using UnityEditor;

[CustomEditor(typeof(PlaySound)), CanEditMultipleObjects]
public class PlaySoundEditor : Editor
{
	private SerializedProperty soundToPlay;
	private SerializedProperty playOnInput;
	
	private SerializedProperty inputName;
	private SerializedProperty inputChoiceIndex;

	private void OnEnable ()
	{
		soundToPlay = serializedObject.FindProperty("soundToPlay");
		playOnInput = serializedObject.FindProperty("playOnInput");
		
		inputName = serializedObject.FindProperty("inputName");
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
	}

	public override void OnInspectorGUI ()
	{
		InitGUI();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(soundToPlay);

			if (soundToPlay.Exists())
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(playOnInput);
					
					if (playOnInput.boolValue)
					{
						InputSelectionSlider();
					}
				}
				EditorGUILayout.EndVertical();
			}
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
	
	private void InputSelectionSlider()
	{
		EditorGUILayout.BeginVertical(UIHelper.subStyle1);
		{
			inputChoiceIndex.intValue = EditorGUILayout.Popup("Input Axis : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
			inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
		}
		EditorGUILayout.EndVertical();
	}
}