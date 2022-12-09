using UnityEditor;

[CustomEditor(typeof(SpawnOnDestroy)), CanEditMultipleObjects]
public class SpawnOnDestroyEditor : Editor
{
	private SerializedProperty objectToSpawn;
	private SerializedProperty spawnInsideSameParent;
	private void OnEnable ()
	{
		objectToSpawn = serializedObject.FindProperty("objectToSpawn");
		spawnInsideSameParent = serializedObject.FindProperty("spawnInsideSameParent");
	}

	public override void OnInspectorGUI ()
	{
		InitGUI();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(objectToSpawn);

			if (objectToSpawn.Exists())
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(spawnInsideSameParent);
				}
				EditorGUILayout.EndVertical();
			}
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