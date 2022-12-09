using UnityEditor;

[CustomEditor(typeof(CameraShakeOnInputHold)), CanEditMultipleObjects]
public class CameraShakeOnInputHoldEditor : Editor
{
	private SerializedProperty shakerTransform;

	private SerializedProperty intensity;
	private SerializedProperty frequency;
	private SerializedProperty smoothTime;
	
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;

	private void OnEnable ()
	{
		shakerTransform = serializedObject.FindProperty("shakerTransform");

		intensity = serializedObject.FindProperty("intensity");
		frequency = serializedObject.FindProperty("frequency");
		smoothTime = serializedObject.FindProperty("smoothTime");
		
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(shakerTransform);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
				inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(intensity);
				EditorGUILayout.PropertyField(frequency);
				EditorGUILayout.PropertyField(smoothTime);
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
