using UnityEditor;

[CustomEditor(typeof(ScoreOverTime)), CanEditMultipleObjects]
public class ScoreOverTimeEditor : Editor
{
	private SerializedProperty scoreManager;
	private SerializedProperty scoreModificationAmount;
	
	private SerializedProperty scoreModificationFrequency;
	
	private void OnEnable ()
	{
		scoreManager = serializedObject.FindProperty("scoreManager");
		
		scoreModificationAmount = serializedObject.FindProperty("scoreModificationAmount");
		scoreModificationFrequency = serializedObject.FindProperty("scoreModificationFrequency");
	}

	public override void OnInspectorGUI ()
	{
		InitGUI();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(scoreManager);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(scoreModificationAmount);
			EditorGUILayout.PropertyField(scoreModificationFrequency);
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
}