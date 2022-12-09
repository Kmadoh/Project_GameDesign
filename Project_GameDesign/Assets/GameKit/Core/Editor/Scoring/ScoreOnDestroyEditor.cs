using UnityEditor;

[CustomEditor(typeof(ScoreOnDestroy)), CanEditMultipleObjects]
public class ScoreOnDestroyEditor : Editor
{
	private SerializedProperty scoreModificationAmount;
	private SerializedProperty scoreManager;


	private void OnEnable ()
	{
		scoreModificationAmount = serializedObject.FindProperty("scoreModificationAmount");
		scoreManager = serializedObject.FindProperty("scoreManager");
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