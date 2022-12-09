using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using Object = UnityEngine.Object;

public static class Helper
{
	public enum FXFeedbackType {ParticleSystem, VFXGraph, Instantiate, None};
	public enum Axis { X, Y, Z };

	public static string[] layers = new string[LayerMaskUtils.GetLayerCount()];
	#region Curved Lerp
	/////// Float ///////
	/// 
	/// <summary>
	/// Returns a float lerped between two values according to an AnimationCurve.
	/// </summary>
	public static float CurvedLerp (float minValue, float maxValue, AnimationCurve curve, float t)
	{
		float curveEvaluate = curve.Evaluate(t);

		float lerpedValue = Mathf.Lerp(minValue, maxValue, curveEvaluate);

		return lerpedValue;
	}
	
	/// <summary>
	/// Returns a float lerped between two values according to an AnimationCurve.
	/// </summary>
	public static float CurvedLerp (float minValue, float maxValue, float t)
	{
		AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		float curveEvaluate = animCurve.Evaluate(t);

		float lerpedValue = Mathf.Lerp(minValue, maxValue, curveEvaluate);

		return lerpedValue;
	}
	#endregion

	/// <summary>
	/// Returns a Vector3 corresponding to Input Direction on two axis
	/// </summary>
	/// <param name="hInputName">Horizontal Input Name used in the Input Manager</param>
	/// <param name="vInputName">Vertical Input Name used in the Input Manager</param>
	/// <returns>Normalized movement direction</returns>
	public static Vector3 GetMovementDirection (string hInputName, string vInputName)
	{
		Vector3 moveDir = Vector3.zero;

		moveDir.x = Input.GetAxis(hInputName);
		moveDir.y = Input.GetAxis(vInputName);

		if (moveDir.magnitude > 1f)
		{
			moveDir.Normalize();
		}
		
		return moveDir;
	}
	
	/// <summary>
	/// Returns a Vector3 corresponding to Input Direction on two axis
	/// </summary>
	/// <param name="hInputName">Horizontal Input Name used in the Input Manager</param>
	/// <param name="vInputName">Vertical Input Name used in the Input Manager</param>
	/// <param name="horizontalAxis">Which axis is considered horizontal (Usually it's x)</param>
	/// <param name="verticalAxis">Which axis is considered up ? (Usually it's y)</param>
	/// <returns>Normalized movement direction</returns>
	public static Vector3 GetMovementDirection (string hInputName, string vInputName,  Axis horizontalAxis, Axis verticalAxis)
	{
		Vector3 moveDir = Vector3.zero;

		if (horizontalAxis == verticalAxis)
		{
			Debug.LogWarning("Horizontal and Vertical Axes are the same !");
		}

		switch (horizontalAxis)
		{
			case Axis.X:
				moveDir.x = Input.GetAxis(hInputName);
				break;
			case Axis.Y:
				moveDir.y = Input.GetAxis(hInputName);
				break;
			case Axis.Z:
				moveDir.z = Input.GetAxis(hInputName);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(horizontalAxis), horizontalAxis, null);
		}
		
		switch (verticalAxis)
		{
			case Axis.X:
				moveDir.x = Input.GetAxis(vInputName);
				break;
			case Axis.Y:
				moveDir.y = Input.GetAxis(vInputName);
				break;
			case Axis.Z:
				moveDir.z = Input.GetAxis(vInputName);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(horizontalAxis), horizontalAxis, null);
		}

		if (moveDir.magnitude > 1f)
		{
			moveDir.Normalize();
		}
		
		return moveDir;
	}
	
	/// <summary>
	/// Returns a Vector3 corresponding to Input Direction on three axis
	/// </summary>
	/// <param name="xInputName">X Input Name used in the Input Manager</param>
	/// <param name="yInputName">Y Input Name used in the Input Manager</param>
	/// <param name="zInputName">Z Input Name used in the Input Manager</param>
	/// <returns>Normalized movement direction</returns>
	public static Vector3 GetMovementDirection (string xInputName, string yInputName, string zInputName)
	{
		Vector3 moveDir = Vector3.zero;

		moveDir.x = Input.GetAxis(xInputName);
		moveDir.y = Input.GetAxis(yInputName);
		moveDir.z = Input.GetAxis(zInputName);
		
		if (moveDir.magnitude > 1f)
		{
			moveDir.Normalize();
		}
		
		return moveDir;
	}

	#if UNITY_EDITOR
	
	/// <summary>
	/// Gets all Input names contained in the Input Manager menu.
	/// </summary>
	/// <returns>Returns an array containing scenes added in build Settings.</returns>
	public static string[] GetInputAxes()
	{
		Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
		
		SerializedObject obj = new SerializedObject(inputManager);
 
		SerializedProperty axisArray = obj.FindProperty("m_Axes");

		if (axisArray.arraySize == 0) return null;
		
		string[] axisNames = new string[axisArray.arraySize];
		
		for( int i = 0; i < axisArray.arraySize; ++i )
		{
			SerializedProperty axis = axisArray.GetArrayElementAtIndex(i);
 
			string name = axis.FindPropertyRelative("m_Name").stringValue;

			axisNames[i] = name;
		}

		return axisNames;
	}

	/// <summary>
	/// Gets all scene names contained in build settings. Be careful not to leave blank ones (Mostly deleted scenes).
	/// </summary>
	/// <returns>Returns an array containing scenes added in build Settings.</returns>
	public static string[] GetSceneNames()
	{
		int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

		if (sceneCount == 0)
		{
			//Debug.LogError("No Scene Added in build ! Go to Build Settings and att at least one.");
			return new string[] {"No Scene Setup, please add one in Build Settings"};
		}
		string[] scenes = new string[sceneCount];
		for( int i = 0; i < sceneCount; i++ )
		{
			scenes[i] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );
		}

		return scenes;
	}
#endif
	/// <summary>
	/// Returns a if target GameObject is on the ground (Round check shape)
	/// </summary>
	/// <param name="from">Pivot position</param>
	/// <param name="t">Transform used for local space conversion</param>
	/// <param name="radius">Range of the ground check</param>
	/// <param name="wallAvoidanceLayers">Which Layers are considered as Ground ?</param>
	/// <param name="offset">Offset from transform pivot.</param>
	/// <returns>Is the object on the ground ?</returns>
	public static bool GroundCheck (Vector3 from, Vector3 offset, Transform t, float radius, LayerMask wallAvoidanceLayers)
	{
		Vector3 checkPos = from + offset.WorldToLocalSpace(t);
		return Physics.CheckSphere(checkPos, radius, wallAvoidanceLayers);
	}


	/*/// <summary>
	/// Checks for wall collision. Used to prevent movement against walls
	/// </summary>
	/// <param name="t">Transform used for reference</param>
	/// <param name="offset">Offset from GameObject's pivot point</param>
	/// <param name="halfExtents">Size of the check box</param>
	/// <param name="wallAvoidanceLayers">Which layers are considered as Walls ?</param>
	/// <returns>Is the object facing a wall ?</returns>
	public static bool WallCheck (this Transform t, Vector3 offset, Vector3 halfExtents, LayerMask wallAvoidanceLayers)
	{
		return Physics.CheckBox(t.position + t.TransformDirection(offset), halfExtents, t.rotation, wallAvoidanceLayers, QueryTriggerInteraction.Ignore);
	}*/

	/// <summary>
	/// Converts an angle in degrees into a direction, depending on specified transform
	/// </summary>
	/// <param name="t">Transform used for reference</param>
	/// <param name="angleInDegrees">Offset from GameObject's pivot point</param>
	/// <returns>Is the object facing a wall ?</returns>
	public static Vector3 DirFromAngle (this float angleInDegrees, Transform t)
	{
		angleInDegrees += t.eulerAngles.y;

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	
	/// <summary>
	/// Converts an angle in degrees into a direction
	/// </summary>
	/// <param name="angleInDegrees">Angle in degrees</param>
	/// <returns>Direction converted from target angle</returns>
	public static Vector3 DirFromAngle (float angleInDegrees)
	{
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}


	private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
	/// <summary>
	/// A more efficient take on the WaitForSeconds used in Coroutines.
	/// </summary>
	/// <param name="time">Wait time</param>
	/// <returns></returns>
	public static WaitForSeconds GetWait(float time)
	{
		if (WaitDictionary.TryGetValue(time, out WaitForSeconds wait))
		{
			return wait;
		}

		WaitDictionary[time] = new WaitForSeconds(time);
		return WaitDictionary[time];
	}

	private static PointerEventData _eventDataCurrentPosition;
	private static List<RaycastResult> _results;

	/// <summary>
	/// Checks if cursor is over UI
	/// </summary>
	/// <returns>Is cursor over UI ?</returns>
	public static bool IsOverUi ()
	{
		_eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
		_results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
		return _results.Count > 0;
	}
}



