using UnityEditor;

[CustomEditor(typeof(ScoreOnCollide)), CanEditMultipleObjects]
public class ScoreOnCollideEditor : Editor
{
	private SerializedProperty scoreModificationAmount;
	private SerializedProperty triggerOnlyOnce;
	
	private SerializedProperty useTag;
	
	private SerializedProperty tagName;
	private SerializedProperty scoreManager;


	private void OnEnable ()
	{
		scoreModificationAmount = serializedObject.FindProperty("scoreModificationAmount");
		triggerOnlyOnce = serializedObject.FindProperty("triggerOnlyOnce");
		
		useTag = serializedObject.FindProperty("useTag");
		tagName = serializedObject.FindProperty("tagName");
		
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
			EditorGUILayout.PropertyField(triggerOnlyOnce);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(useTag);
			EditorGUILayout.PropertyField(tagName);
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