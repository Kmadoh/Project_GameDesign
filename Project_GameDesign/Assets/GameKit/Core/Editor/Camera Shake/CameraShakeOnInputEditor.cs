using UnityEditor;

[CustomEditor(typeof(CameraShakeOnInput))]
public class CameraShakeOnInputEditor : Editor
{
	private SerializedProperty shakeDuration;
	private SerializedProperty intensity;

	private SerializedProperty useCooldown;
	private SerializedProperty cooldown;

	private SerializedProperty onlyOnce;
	private SerializedProperty targetToShake;
	
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;

	private void OnEnable ()
	{
		targetToShake = serializedObject.FindProperty("targetToShake");

		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
		
		shakeDuration = serializedObject.FindProperty("shakeDuration");
		intensity = serializedObject.FindProperty("intensity");

		useCooldown = serializedObject.FindProperty("useCooldown");
		cooldown = serializedObject.FindProperty("cooldown");

		onlyOnce = serializedObject.FindProperty("onlyOnce");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{

			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(targetToShake);
				EditorGUILayout.PropertyField(onlyOnce);
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
				EditorGUILayout.PropertyField(shakeDuration);
				EditorGUILayout.PropertyField(intensity);

			}
			EditorGUILayout.EndVertical();

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

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
