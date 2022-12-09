using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Follow)), CanEditMultipleObjects]
public class FollowEditor : Editor
{
	int toolBarTab;
	string currentTab;

	private SerializedProperty _target;

	private SerializedProperty followOnXAxis;
	private SerializedProperty followOnYAxis;
	private SerializedProperty followOnZAxis;
	private SerializedProperty smoothTime;
	private SerializedProperty maxDistance;

	private SerializedProperty shareOrientation;
	private SerializedProperty rotateOnXAxis;
	private SerializedProperty rotateOnYAxis;
	private SerializedProperty rotateOnZAxis;
	private SerializedProperty rotateSpeed;

	private void OnEnable ()
	{
		_target = serializedObject.FindProperty("target");

		followOnXAxis = serializedObject.FindProperty("followOnXAxis");
		followOnYAxis = serializedObject.FindProperty("followOnYAxis");
		followOnZAxis = serializedObject.FindProperty("followOnZAxis");
		smoothTime = serializedObject.FindProperty("smoothTime");
		maxDistance = serializedObject.FindProperty("maxDistance");

		shareOrientation = serializedObject.FindProperty("shareOrientation");
		rotateOnXAxis = serializedObject.FindProperty("rotateOnXAxis");
		rotateOnYAxis = serializedObject.FindProperty("rotateOnYAxis");
		rotateOnZAxis = serializedObject.FindProperty("rotateOnZAxis");
		rotateSpeed = serializedObject.FindProperty("rotateSpeed");
	}


	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.Space();

		if(_target.propertyType == SerializedPropertyType.ObjectReference && _target.objectReferenceValue == null)
		{
			EditorGUILayout.BeginVertical(UIHelper.warningStyle);
			{
				EditorGUILayout.PropertyField(_target);
			}
			EditorGUILayout.EndVertical();
		}
		else
		{
			EditorGUILayout.BeginVertical(UIHelper.mainStyle);
			{
				EditorGUILayout.PropertyField(_target);
			}
			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			toolBarTab = GUILayout.Toolbar(toolBarTab, new string[] { "Position", "Direction" });

			currentTab = toolBarTab switch
			{
				0 => "Position",
				1 => "Direction",
				_ => currentTab
			};

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				GUI.FocusControl(null);
			}

			EditorGUI.BeginChangeCheck();

			switch (currentTab)
			{
				case "Position":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							EditorGUILayout.PropertyField(followOnXAxis);
							EditorGUILayout.PropertyField(followOnYAxis);
							EditorGUILayout.PropertyField(followOnZAxis);
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(UIHelper.subStyle1);
						{
							EditorGUILayout.PropertyField(smoothTime);
							EditorGUILayout.PropertyField(maxDistance);
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndVertical();
				}
				break;

				case "Direction":
				{
					EditorGUILayout.BeginVertical(UIHelper.mainStyle);
					{
						EditorGUILayout.PropertyField(shareOrientation);
						if (shareOrientation.boolValue)
						{
							EditorGUILayout.BeginVertical(UIHelper.subStyle1);
							{
								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									EditorGUILayout.PropertyField(rotateOnXAxis);
									EditorGUILayout.PropertyField(rotateOnYAxis);
									EditorGUILayout.PropertyField(rotateOnZAxis);
								}
								EditorGUILayout.EndVertical();

								EditorGUILayout.BeginVertical(UIHelper.subStyle2);
								{
									EditorGUILayout.PropertyField(rotateSpeed);
								}
								EditorGUILayout.EndVertical();
							}
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndVertical();
				}
				break;
			}

		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		if (_target.propertyType == SerializedPropertyType.ObjectReference && _target.objectReferenceValue == null)
		{
			EditorGUILayout.BeginVertical(UIHelper.warningStyle);
			{
				EditorGUILayout.LabelField("No target set !", EditorStyles.boldLabel);
			}
			EditorGUILayout.EndVertical();
		}
	}
}