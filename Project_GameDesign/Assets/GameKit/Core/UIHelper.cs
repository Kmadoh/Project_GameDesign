using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public static class UIHelper
{
	public static GUIStyle mainStyle;
	public static GUIStyle subStyle1;
	public static GUIStyle subStyle2;
	public static GUIStyle subStyle3;
	public static GUIStyle buttonStyle;
	public static GUIStyle redButtonStyle;
	public static GUIStyle greenButtonStyle;
	public static GUIStyle warningStyle;
	public static GUIStyle headerStyle;
	
	private static Texture2D MakeTex (int width, int height, Color col)
	{
		Color[] pix = new Color[width * height];

		for (int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();

		return result;
	}

	public static void InitializeStyles ()
	{
		 GUISkin gameKitSkin = Resources.Load<GUISkin>("GameKit/UI/GameKitSkinB");

		 mainStyle = gameKitSkin.GetStyle("MainStyle");
		 subStyle1 = gameKitSkin.GetStyle("SubStyle1");
		 subStyle2 = gameKitSkin.GetStyle("SubStyle2");
		 subStyle3 = gameKitSkin.GetStyle("SubStyle3");
		 buttonStyle = gameKitSkin.GetStyle("ButtonStyle");
		 redButtonStyle = gameKitSkin.GetStyle("RedButton");
		 greenButtonStyle = gameKitSkin.GetStyle("GreenButton");
		 warningStyle = gameKitSkin.GetStyle("WarningStyle");
		 headerStyle = gameKitSkin.GetStyle("HeaderStyle");
	}
	#if UNITY_EDITOR


	public static void PreShotDirty(string undoName, Object target)
	{
		if (EditorApplication.isPlaying) return;
		
		Undo.RecordObject(target, undoName);
	}

	public static void DirtyStuff(Object target)
	{
		if (EditorApplication.isPlaying) return;
		
		EditorUtility.SetDirty(target);
		PrefabUtility.RecordPrefabInstancePropertyModifications(target);
		UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
	}

	public static bool Exists(this SerializedProperty property)
	{
		bool isNotNull = property.propertyType == SerializedPropertyType.ObjectReference &&
		              property.objectReferenceValue != null;

		return isNotNull;
	}
	
	public static void DrawArc (float angle, float range, Transform t)
	{
		Handles.color = Color.blue;
		Vector3 effectAngleA = (-angle / 2).DirFromAngle(t);
		Vector3 effectAngleB = (angle / 2).DirFromAngle(t);

		Vector3 tPosition = t.position;
		Handles.DrawWireArc(tPosition, Vector3.up, effectAngleA, angle, range);
		
		Handles.DrawLine(tPosition, tPosition + effectAngleA * range);
		Handles.DrawLine(tPosition, tPosition + effectAngleB * range);
	}
#endif
}