using UnityEditor;

[CustomEditor(typeof(PlayAnimOnTrigger)), CanEditMultipleObjects]
public class PlayAnimOnTriggerEditor : Editor
{
	private SerializedProperty animator;
	private SerializedProperty triggerName;
	private SerializedProperty useTag;
	private SerializedProperty tagName;
	private SerializedProperty triggerOnce;
	
	private void OnEnable ()
	{
		animator = serializedObject.FindProperty("animator");
		triggerName = serializedObject.FindProperty("triggerName");
		useTag = serializedObject.FindProperty("useTag");
		tagName = serializedObject.FindProperty("tagName");
		triggerOnce = serializedObject.FindProperty("triggerOnce");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(animator);
			if(animator.objectReferenceValue != null)
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(triggerName);
					EditorGUILayout.PropertyField(triggerOnce);
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(useTag);

					if(useTag.boolValue)
					{
						tagName.stringValue = EditorGUILayout.TagField(tagName.stringValue);
					}
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndVertical();
		
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		EditorGUILayout.Space();
	}
}
