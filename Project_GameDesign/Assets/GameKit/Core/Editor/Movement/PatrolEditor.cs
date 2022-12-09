using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Patrol))]
public class PatrolEditor : Editor
{
	private SerializedProperty patrolPoints;
	private SerializedProperty areCoordinatesLocal;
	private SerializedProperty moveSpeed;
	private SerializedProperty minDistance;
	private SerializedProperty loop;

	private SerializedProperty lookAtTargetPos;
	private SerializedProperty smoothLookAt;
	private SerializedProperty rotateSpeed;
	private SerializedProperty lockXRotation;
	private SerializedProperty lockYRotation;
	private SerializedProperty lockZRotation;

	bool showPatrolPoints = true;

	private void OnEnable ()
	{
		patrolPoints = serializedObject.FindProperty("patrolPoints");
		areCoordinatesLocal = serializedObject.FindProperty("areCoordinatesLocal");
		moveSpeed = serializedObject.FindProperty("moveSpeed");
		minDistance = serializedObject.FindProperty("minDistance");
		loop = serializedObject.FindProperty("loop");

		lookAtTargetPos = serializedObject.FindProperty("lookAtTargetPos");
		smoothLookAt = serializedObject.FindProperty("smoothLookAt");
		rotateSpeed = serializedObject.FindProperty("rotateSpeed");
		lockXRotation = serializedObject.FindProperty("lockXRotation");
		lockYRotation = serializedObject.FindProperty("lockYRotation");
		lockZRotation = serializedObject.FindProperty("lockZRotation");

	}

	private void AddPatrolPoint()
	{		
		UIHelper.PreShotDirty("AddPatrolPoint", target);
		
		patrolPoints.InsertArrayElementAtIndex(patrolPoints.arraySize);

		if (patrolPoints.arraySize == 1)
		{
			SerializedProperty pointColor = patrolPoints.GetArrayElementAtIndex(0).FindPropertyRelative("pointColor");
			pointColor.colorValue = Color.green;
		}
		
		SerializedProperty pointName = patrolPoints.GetArrayElementAtIndex(patrolPoints.arraySize-1).FindPropertyRelative("pointName");
		SerializedProperty pointPosition = patrolPoints.GetArrayElementAtIndex(patrolPoints.arraySize-1).FindPropertyRelative("pointPosition");

		pointName.stringValue = "Patrol Point " + patrolPoints.arraySize;
		pointPosition.vector3Value = areCoordinatesLocal.boolValue ? Selection.transforms[0].localPosition : Selection.transforms[0].position;

		if(!showPatrolPoints)
		{
			showPatrolPoints = true;
		}
		
		UIHelper.DirtyStuff(target);
	}

	private void DuplicatePatrolPoint (int index)
	{
		patrolPoints.InsertArrayElementAtIndex(index+1);

		SerializedProperty pointName = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointName");
		SerializedProperty pointColor = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointColor");
		SerializedProperty pointPosition = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointPosition");
		SerializedProperty waitTimeAtPoint = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("waitTimeAtPoint");
		
		SerializedProperty pointNameB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("pointName");
		SerializedProperty pointColorB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("pointColor");
		SerializedProperty pointPositionB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("pointPosition");
		SerializedProperty waitTimeAtPointB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("waitTimeAtPoint");
		
		pointColorB.colorValue = pointColor.colorValue;
		pointNameB.stringValue = pointName.stringValue + " (Copy)";
		pointPositionB.vector3Value = pointPosition.vector3Value;
		waitTimeAtPointB.floatValue = waitTimeAtPoint.floatValue;
	}

	private void MovePatrolPointUp (int index)
	{
		if (index >= patrolPoints.arraySize - 1) return;
		
		Patrol.PatrolPoint patrol = new Patrol.PatrolPoint();
			
		SerializedProperty pointName = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointName");
		SerializedProperty pointColor = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointColor");
		SerializedProperty pointPosition = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointPosition");
		SerializedProperty waitTimeAtPoint = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("waitTimeAtPoint");
			
		SerializedProperty pointNameB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("pointName");
		SerializedProperty pointColorB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("pointColor");
		SerializedProperty pointPositionB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("pointPosition");
		SerializedProperty waitTimeAtPointB = patrolPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("waitTimeAtPoint");

		patrol.pointColor = pointColorB.colorValue;
		patrol.pointName = pointNameB.stringValue;
		patrol.pointPosition = pointPositionB.vector3Value;
		patrol.waitTimeAtPoint = waitTimeAtPointB.floatValue;

		pointColorB.colorValue = pointColor.colorValue;
		pointNameB.stringValue = pointName.stringValue;
		pointPositionB.vector3Value = pointPosition.vector3Value;
		waitTimeAtPointB.floatValue = waitTimeAtPoint.floatValue;

		pointColor.colorValue = patrol.pointColor;
		pointName.stringValue = patrol.pointName;
		pointPosition.vector3Value = patrol.pointPosition;
		waitTimeAtPoint.floatValue = patrol.waitTimeAtPoint;
	}

	private void MovePatrolPointDown (int index)
	{
		if (index <= 0) return;
		
		Patrol.PatrolPoint patrol = new Patrol.PatrolPoint();

		SerializedProperty pointName = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointName");
		SerializedProperty pointColor = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointColor");
		SerializedProperty pointPosition = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("pointPosition");
		SerializedProperty waitTimeAtPoint = patrolPoints.GetArrayElementAtIndex(index).FindPropertyRelative("waitTimeAtPoint");
		
		patrol.pointColor = pointColor.colorValue;
		patrol.pointName = pointName.stringValue;
		patrol.pointPosition = pointPosition.vector3Value;
		patrol.waitTimeAtPoint = waitTimeAtPoint.floatValue;
		
		SerializedProperty pointNameB = patrolPoints.GetArrayElementAtIndex(index - 1).FindPropertyRelative("pointName");
		SerializedProperty pointColorB = patrolPoints.GetArrayElementAtIndex(index - 1).FindPropertyRelative("pointColor");
		SerializedProperty pointPositionB = patrolPoints.GetArrayElementAtIndex(index - 1).FindPropertyRelative("pointPosition");
		SerializedProperty waitTimeAtPointB = patrolPoints.GetArrayElementAtIndex(index - 1).FindPropertyRelative("waitTimeAtPoint");

		pointColor.colorValue = pointColorB.colorValue;
		pointName.stringValue = pointNameB.stringValue;
		pointPosition.vector3Value = pointPositionB.vector3Value;
		waitTimeAtPoint.floatValue = waitTimeAtPointB.floatValue;

		pointColorB.colorValue = patrol.pointColor;
		pointNameB.stringValue = patrol.pointName;
		pointPositionB.vector3Value = patrol.pointPosition;
		waitTimeAtPointB.floatValue = patrol.waitTimeAtPoint;
	}

	private void RemovePatrolPoint (int index)
	{
		patrolPoints.DeleteArrayElementAtIndex(index);
	}

	public override void OnInspectorGUI ()
	{
		//Initializing Custom GUI Styles
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.Space(5);
			EditorGUILayout.BeginHorizontal();
			{
				if (!showPatrolPoints)
				{
					if (GUILayout.Button(" Show Patrol Points (" + patrolPoints.arraySize + ")", UIHelper.redButtonStyle, GUILayout.MaxHeight(30f)))
					{
						showPatrolPoints = true;
					}
				}
				else
				{
					if (GUILayout.Button(" Hide Patrol Points (" + patrolPoints.arraySize + ")", UIHelper.greenButtonStyle, GUILayout.MaxHeight(30f)))
					{
						showPatrolPoints = false;
					}
				}
				if (GUILayout.Button(" Add Patrol Point ", UIHelper.greenButtonStyle, GUILayout.MaxHeight(30f)))
				{
					AddPatrolPoint();
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space(5);
			EditorGUILayout.EndVertical();

			if (showPatrolPoints)
			{
				EditorGUILayout.BeginVertical(UIHelper.mainStyle);
				{
					for (int i = 0; i < patrolPoints.arraySize; i++)
					{
						EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
						{
							SerializedProperty pointName = patrolPoints.GetArrayElementAtIndex(i).FindPropertyRelative("pointName");
							pointName.stringValue = EditorGUILayout.TextField(pointName.stringValue, GUILayout.MaxWidth(200f));
							
							SerializedProperty pointColor = patrolPoints.GetArrayElementAtIndex(i).FindPropertyRelative("pointColor");
							pointColor.colorValue = EditorGUILayout.ColorField(pointColor.colorValue, GUILayout.MaxWidth(100f));
							
							SerializedProperty pointPosition = patrolPoints.GetArrayElementAtIndex(i).FindPropertyRelative("pointPosition");
							pointPosition.vector3Value = EditorGUILayout.Vector3Field("", pointPosition.vector3Value, GUILayout.MaxWidth(170f));
							
							EditorGUILayout.LabelField("Wait ", GUILayout.MaxWidth(40f));
							SerializedProperty waitTimeAtPoint = patrolPoints.GetArrayElementAtIndex(i).FindPropertyRelative("waitTimeAtPoint");
							waitTimeAtPoint.floatValue = EditorGUILayout.FloatField(waitTimeAtPoint.floatValue, GUILayout.MaxWidth(40f));

							if (GUILayout.Button("Copy", UIHelper.greenButtonStyle, GUILayout.MaxWidth(40)))
							{
								DuplicatePatrolPoint(i);
							}

							if (i < patrolPoints.arraySize - 1)
							{
								if (GUILayout.Button("v", UIHelper.buttonStyle, GUILayout.MaxWidth(25)))
								{
									MovePatrolPointUp(i);
								}
							}

							if (i > 0)
							{
								if (GUILayout.Button("^", UIHelper.buttonStyle, GUILayout.MaxWidth(25)))
								{
									MovePatrolPointDown(i);
								}
							}

							if (GUILayout.Button("X", UIHelper.redButtonStyle, GUILayout.MaxWidth(25)))
							{
								RemovePatrolPoint(i);
							}

						}
						EditorGUILayout.EndHorizontal();
					}
				}
				EditorGUILayout.EndVertical();
			}
		}
		

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(lookAtTargetPos);
			if (lookAtTargetPos.boolValue)
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(smoothLookAt);
					if(smoothLookAt.boolValue)
					{
						EditorGUILayout.BeginVertical(UIHelper.subStyle2);
						{
							EditorGUILayout.PropertyField(rotateSpeed);
						}
						EditorGUILayout.EndVertical();
					}

					EditorGUILayout.BeginVertical(UIHelper.subStyle2);
					{
						EditorGUILayout.PropertyField(lockXRotation);
						EditorGUILayout.PropertyField(lockYRotation);
						EditorGUILayout.PropertyField(lockZRotation);
					}
					EditorGUILayout.EndVertical();
					
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(areCoordinatesLocal);
			EditorGUILayout.PropertyField(moveSpeed);
			EditorGUILayout.PropertyField(minDistance);
			EditorGUILayout.PropertyField(loop);
		}
		EditorGUILayout.EndVertical();
		
		serializedObject.ApplyModifiedProperties();
		if (EditorGUI.EndChangeCheck())
		{

		}
	}
}
