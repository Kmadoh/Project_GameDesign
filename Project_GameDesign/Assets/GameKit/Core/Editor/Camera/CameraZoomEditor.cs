using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraZoom)), CanEditMultipleObjects]
public class CameraZoomEditor : Editor
{
	[Tooltip("Used to check public variables from the target class")]
	CameraZoom myObject;

	private SerializedProperty usedCamera;
	private SerializedProperty zoomCurve;

	private SerializedProperty sensitivity;
	private SerializedProperty smoothSpeed;
	
	private SerializedProperty minZoomFOV;
	private SerializedProperty maxZoomFOV;
	
	private SerializedProperty inputChoiceIndex;
	private SerializedProperty zoomInputName;

	private void OnEnable ()
	{

		myObject = (CameraZoom) target;

		zoomCurve = serializedObject.FindProperty("zoomCurve");
		zoomCurve = serializedObject.FindProperty("zoomCurve");

		sensitivity = serializedObject.FindProperty("sensitivity");
		smoothSpeed = serializedObject.FindProperty("smoothSpeed");
		
		minZoomFOV = serializedObject.FindProperty("minZoomFOV");
		maxZoomFOV = serializedObject.FindProperty("maxZoomFOV");

		usedCamera = serializedObject.FindProperty("usedCamera");
		inputChoiceIndex = serializedObject.FindProperty("inputChoiceIndex");
		zoomInputName = serializedObject.FindProperty("zoomInputName");
	}

	public override void OnInspectorGUI ()
	{
		UIHelper.InitializeStyles();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();

		EditorGUILayout.BeginVertical(UIHelper.mainStyle);
		{
			EditorGUILayout.PropertyField(usedCamera);

			if(usedCamera.objectReferenceValue != null)
			{
				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					inputChoiceIndex.intValue = EditorGUILayout.Popup("Zoom Axis : ", inputChoiceIndex.intValue, Helper.GetInputAxes());
					zoomInputName.stringValue = Helper.GetInputAxes()[inputChoiceIndex.intValue];
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(zoomCurve);
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
				{
					EditorGUILayout.LabelField("Zoom FOV : ", GUILayout.MaxWidth(100f));
					minZoomFOV.floatValue = EditorGUILayout.FloatField(minZoomFOV.floatValue, GUILayout.MaxWidth(50f));
					EditorGUILayout.MinMaxSlider(ref myObject.minZoomFOV, ref myObject.maxZoomFOV, 5f, 120f);
					maxZoomFOV.floatValue = EditorGUILayout.FloatField(maxZoomFOV.floatValue, GUILayout.MaxWidth(50f));
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal(UIHelper.subStyle1);
				{
					EditorGUILayout.PropertyField(sensitivity);
					EditorGUILayout.PropertyField(smoothSpeed);
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
