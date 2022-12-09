using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetAnimBoolOnInput)), CanEditMultipleObjects]
public class SetAnimBoolOnInputEditor : Editor
{
	private SerializedProperty revertOn;
	private SerializedProperty inputName;
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty bools;

	private bool showBools = true;
	
	private void OnEnable ()
	{
		revertOn = serializedObject.FindProperty("revertOn");
		inputName = serializedObject.FindProperty("inputName");
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		bools = serializedObject.FindProperty("bools");
	}

	private void AddBool ()
	{
		bools.InsertArrayElementAtIndex(bools.arraySize);
	}

	private void RemoveBool (int i)
	{
		bools.DeleteArrayElementAtIndex(i);
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			inputChoiceIndex.intValue = EditorGUILayout.Popup("Input ", inputChoiceIndex.intValue, Helper.GetInputAxes());
			inputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
			EditorGUILayout.PropertyField(revertOn);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.BeginHorizontal();
			{
				if (!showBools)
				{
					if (GUILayout.Button(" Show Booleans (" + bools.arraySize + ")", UIHelper.redButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showBools = true;
					}
				}
				else
				{
					if (GUILayout.Button(" Hide Booleans (" + bools.arraySize + ")", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
					{
						showBools = false;
					}
				}
				if (GUILayout.Button(" Add Boolean ", UIHelper.greenButtonStyle, GUILayout.MaxHeight(20f)))
				{
					AddBool();

					if (showBools == false)
					{
						showBools = true;
					}
				}
			}
			EditorGUILayout.EndHorizontal();


			if (showBools && bools.arraySize > 0)
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					for (int i = 0; i < bools.arraySize; i++)
					{
						EditorGUILayout.BeginHorizontal(UIHelper.subStyle2);
						{
							EditorGUILayout.LabelField("Animator", GUILayout.MaxWidth(60f));
							SerializedProperty anim = bools.GetArrayElementAtIndex(i).FindPropertyRelative("animator");
							anim.objectReferenceValue = (Animator)EditorGUILayout.ObjectField(anim.objectReferenceValue, 
								typeof(Animator), true, GUILayout.MaxWidth(200f));
							
							EditorGUILayout.LabelField("Parameter Name", GUILayout.MaxWidth(100f));
							SerializedProperty boolParameterName = bools.GetArrayElementAtIndex(i).FindPropertyRelative("boolParameterName");
							boolParameterName.stringValue = EditorGUILayout.TextField(boolParameterName.stringValue);
							
							EditorGUILayout.LabelField("Value", GUILayout.MaxWidth(60f));
							SerializedProperty valueSetOnInput = bools.GetArrayElementAtIndex(i).FindPropertyRelative("valueSetOnInput");
							valueSetOnInput.boolValue = EditorGUILayout.Toggle(valueSetOnInput.boolValue);

							if (GUILayout.Button("X", UIHelper.redButtonStyle))
							{
								RemoveBool(i);
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