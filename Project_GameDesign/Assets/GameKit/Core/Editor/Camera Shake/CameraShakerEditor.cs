using UnityEditor;

[CustomEditor(typeof(CameraShaker)), CanEditMultipleObjects]
public class CameraShakerEditor : Editor
{
	private SerializedProperty shakeOnXAxis;
	private SerializedProperty shakeOnYAxis;
	private SerializedProperty shakeOnZAxis;

	private SerializedProperty intensityCurveX;
	private SerializedProperty intensityCurveY;
	private SerializedProperty intensityCurveZ;


	private void OnEnable ()
	{
		shakeOnXAxis = serializedObject.FindProperty("shakeOnXAxis");
		shakeOnYAxis = serializedObject.FindProperty("shakeOnYAxis");
		shakeOnZAxis = serializedObject.FindProperty("shakeOnZAxis");

		intensityCurveX = serializedObject.FindProperty("intensityCurveX");
		intensityCurveY = serializedObject.FindProperty("intensityCurveY");
		intensityCurveZ = serializedObject.FindProperty("intensityCurveZ");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(shakeOnXAxis);

			if(shakeOnXAxis.boolValue)
			{
				EditorGUILayout.PropertyField(intensityCurveX);
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(shakeOnYAxis);

			if (shakeOnYAxis.boolValue)
			{
				EditorGUILayout.PropertyField(intensityCurveY);
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(shakeOnZAxis);

			if (shakeOnZAxis.boolValue)
			{
				EditorGUILayout.PropertyField(intensityCurveZ);
			}
		}
		EditorGUILayout.EndHorizontal();


		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}

}