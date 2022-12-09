using UnityEditor;

[CustomEditor(typeof(CameraShakeOnTrigger)), CanEditMultipleObjects]
public class CameraShakeOnTriggerEditor : Editor
{
	private SerializedProperty targetToShake;
	private SerializedProperty useTag;
	private SerializedProperty tagName;
	
	private SerializedProperty intensity;
	private SerializedProperty shakeDuration;
	private SerializedProperty triggerOnce;

	private void OnEnable ()
	{
		targetToShake = serializedObject.FindProperty("targetToShake");
		useTag = serializedObject.FindProperty("useTag");
		tagName = serializedObject.FindProperty("tagName");

		intensity = serializedObject.FindProperty("intensity");
		shakeDuration = serializedObject.FindProperty("shakeDuration");
		triggerOnce = serializedObject.FindProperty("triggerOnce");

	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(targetToShake);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(triggerOnce);
				EditorGUILayout.BeginHorizontal(UIHelper.subStyle2);
				{
					EditorGUILayout.PropertyField(useTag);
					if(useTag.boolValue)
					{
						tagName.stringValue = EditorGUILayout.TagField(tagName.stringValue);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(intensity);
				EditorGUILayout.PropertyField(shakeDuration);
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
