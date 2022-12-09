using UnityEditor;

[CustomEditor(typeof(SceneHandler)), CanEditMultipleObjects]
public class SceneHandlerEditor : Editor
{
	private SerializedProperty fadeBetweenScenes;
	private SerializedProperty fadeTriggerName;
	
	private SerializedProperty animator;
	private SerializedProperty fadeImage;
	
	private void OnEnable ()
	{
		fadeBetweenScenes = serializedObject.FindProperty("fadeBetweenScenes");		
		fadeTriggerName = serializedObject.FindProperty("fadeTriggerName");
		
		animator = serializedObject.FindProperty("animator");
		fadeImage = serializedObject.FindProperty("fadeImage");
	}

	public override void OnInspectorGUI ()
	{
		InitGUI();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(fadeBetweenScenes);

			if (fadeBetweenScenes.boolValue)
			{
				EditorGUILayout.PropertyField(animator);

				if (animator.Exists())
				{
					EditorGUILayout.PropertyField(fadeTriggerName);
					EditorGUILayout.PropertyField(fadeImage);
				}
			}
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.LabelField(SceneHandler.isLoadingScene
				? "A scene is currently loading !"
				: "No Scene loading currently");
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