using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetAnimTriggerOnInput)), CanEditMultipleObjects]
public class SetAnimTriggerOnInputEditor : Editor
{
	private SerializedProperty playOnce;
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty inputName;
	
	private SerializedProperty triggers;

	private bool showTriggers = true;

	private void OnEnable ()
	{
		playOnce = serializedObject.FindProperty("playOnce");
		
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		inputName = serializedObject.FindProperty("inputName");
		
		triggers = serializedObject.FindProperty("triggers");
	}

	private void AddTrigger ()
	{
		triggers.InsertArrayElementAtIndex(triggers.arraySize);
	}

	private void RemoveTrigger (int i)
	{
		triggers.DeleteArrayElementAtIndex(i);
	}
	

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			inputChoiceIndex.intValue = EditorGUILayout.Popup("Input : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
			inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
			EditorGUILayout.PropertyField(playOnce);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginHorizontal();
			{
				if (!showTriggers)
				{
					if (GUILayout.Button(" Show Triggers (" + triggers.arraySize + ")", UIHelper.redButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showTriggers = true;
					}
				}
				else
				{
					if (GUILayout.Button(" Hide Triggers (" + triggers.arraySize + ")", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showTriggers = false;
					}
				}
				if (GUILayout.Button(" Add Trigger ", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
				{
					AddTrigger();

					if (showTriggers == false)
					{
						showTriggers = true;
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			if (showTriggers && triggers.arraySize > 0)
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					for (int i = 0; i < triggers.arraySize; i++)
					{
						EditorGUILayout.BeginHorizontal(UIHelper.subStyle2);
						{
							EditorGUILayout.LabelField("Animator", GUILayout.MaxWidth(60f));
							SerializedProperty anim = triggers.GetArrayElementAtIndex(i).FindPropertyRelative("animator");
							anim.objectReferenceValue = (Animator)EditorGUILayout.ObjectField(anim.objectReferenceValue, typeof(Animator), true, GUILayout.MaxWidth(200f));
							
							EditorGUILayout.LabelField("Parameter Name", GUILayout.MaxWidth(100f));
							SerializedProperty triggerParameterName = triggers.GetArrayElementAtIndex(i).FindPropertyRelative("triggerParameterName");
							triggerParameterName.stringValue = EditorGUILayout.TextField(triggerParameterName.stringValue);

							if (GUILayout.Button("X", UIHelper.redButtonStyle))
							{
								RemoveTrigger(i);
							}
						}
						EditorGUILayout.EndHorizontal();
					}
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
}