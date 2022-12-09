using UnityEditor;

[CustomEditor(typeof(CameraShakeOnDestroy)), CanEditMultipleObjects]
public class CameraShakeOnDestroyEditor : Editor
{
	private SerializedProperty shakeDuration;
	private SerializedProperty intensity;
	private SerializedProperty targetToShake;
	private SerializedProperty sceneHandler;

	private void OnEnable ()
	{
		shakeDuration = serializedObject.FindProperty("shakeDuration");
		intensity = serializedObject.FindProperty("intensity");
		targetToShake = serializedObject.FindProperty("targetToShake");
		sceneHandler = serializedObject.FindProperty("sceneHandler");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(targetToShake);
				EditorGUILayout.PropertyField(sceneHandler);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(UIHelper.subStyle1);
			{
				EditorGUILayout.PropertyField(shakeDuration);
				EditorGUILayout.PropertyField(intensity);
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
