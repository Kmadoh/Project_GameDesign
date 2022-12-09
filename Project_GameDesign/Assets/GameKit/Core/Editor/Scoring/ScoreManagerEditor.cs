using UnityEditor;

[CustomEditor(typeof(ScoreManager)), CanEditMultipleObjects]
public class ScoreManagerEditor : Editor
{
	private SerializedProperty score;
	private SerializedProperty initialScore;
	
	private SerializedProperty scoreTitle;
	
	private SerializedProperty scoreText;


	private void OnEnable ()
	{
		score = serializedObject.FindProperty("score");
		initialScore = serializedObject.FindProperty("initialScore");
		scoreTitle = serializedObject.FindProperty("scoreTitle");
		scoreText = serializedObject.FindProperty("scoreText");
	}

	public override void OnInspectorGUI ()
	{
		InitGUI();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.LabelField("Current Score : " + score.intValue.ToString());
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(initialScore);
			EditorGUILayout.PropertyField(scoreTitle);
			EditorGUILayout.PropertyField(scoreText);
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