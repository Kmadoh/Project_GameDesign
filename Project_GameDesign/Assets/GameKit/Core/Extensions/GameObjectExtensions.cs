using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
	//Finds game object in one simple, static, wrapper
	public static Transform GetObject(string objectName) => GameObject.Find(objectName).transform;

	/// <summary>
	/// Find any game object in scene regardless of active status
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static GameObject FindGameObjectsAll(string name) => Resources.FindObjectsOfTypeAll<GameObject>().First(x => x.name == name);
	
}
